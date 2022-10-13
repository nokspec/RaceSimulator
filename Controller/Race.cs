using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using static Model.SectionData;
using static System.Collections.Specialized.BitVector32;
using Section = Model.Section;

namespace Controller
{
	public class Race
	{
		//Events
		public event EventHandler<DriversChangedEventArgs> DriversChanged;
		public event EventHandler RaceFinished;

		public Track Track { get; set; }
		public Section CurrentSection;
		public Section section;

		public List<IParticipant> Participants { get; set; }
		private Dictionary<Section, SectionData> _positions = new(); //Participants positions with left and right.

		//Timer
		public DateTime StartTime { get; set; }
		private Random _random;
		private System.Timers.Timer _timer; //Timer
		private static int TimerInterval = 500; //Bepaal timer interval.

		//Laps
		public static int amountOfLaps = 1; //Hier bepaal je hoeveel laps een race heeft.
		public static int LapsCount = -1; //met -1 beginnen omdat de participants bij de eerste lap eerst over de finish gaan.

		public Race(Track track, List<IParticipant> participants)
		{
			Track = track;
			Participants = participants;
			_random = new Random(DateTime.Now.Millisecond);
			StartPositions(Track, Participants);

			//Timer
			_timer = new System.Timers.Timer(TimerInterval); //Interval
			Start(); //Start timer
		}
		private void Start() //Start Timer
		{
			StartTime = DateTime.Now;
			_timer.Elapsed += OnTimedEvent; //Subscribe
			_timer.AutoReset = true;
			_timer.Start();
		}

		protected void OnTimedEvent(object sender, EventArgs e)
		{
			//Stop();
			CheckDriverMovement();
			//_timer.Start();
			DriversChanged?.Invoke(this, new DriversChangedEventArgs(Track)); //this is de huidige class.

			if (CheckRaceFinished()) //Als de race gefinished is.
			{
				RaceFinished?.Invoke(this, new EventArgs());
				CleanUp();
			}
		}

		public SectionData GetSectionData(Section section)
		{
			if (!_positions.ContainsKey(section))
			{
				_positions.Add(section, new SectionData());
			}
			return _positions[section];
		}

		public void RandomizeEquipment()
		{
			foreach (IParticipant participant in Participants)
			{
				participant.Equipment.Quality = _random.Next(0, 10);
				participant.Equipment.Performance = _random.Next(0, 10);
			}
		}

		private int CountLaps(IParticipant participant)
		{
			participant.LapsCount++;
			LapsCount++;
			return LapsCount;
		}

		private void NextLap(IParticipant participant, SectionData sectionData)
		{
			if (participant.LapsCount < amountOfLaps)
			{
				CountLaps(participant);
			}
			else if (participant.LapsCount == (amountOfLaps))
			{
				participant.Finished = true;
			}
		}

		private void CleanUp() //Race finished, clean everything up for next race.
		{
			_timer.Stop();
			DriversChanged = null;
			RaceFinished = null;

			foreach (IParticipant participant in Participants)
			{
				participant.Finished = false;
				participant.LapsCount = -1;
			}
			Console.WriteLine("cleanup done");
		}

		private void Stop()
		{
			_timer.Enabled = false;
		}

		private bool CheckRaceFinished()
		{
			int count = 0;
			foreach (IParticipant participant in Participants)
			{
				if (participant.Finished == true)
					count++;
			}
			if (count == Participants.Count)
				return true;
			else
			{
				return false;
			}
		}

		public void CheckDriverMovement()
		{
			foreach (IParticipant participant in Participants)
			{
				participant.MetersMoved += participant.GetMovementSpeed();/*participant.Equipment.Speed * participant.Equipment.Performance;*/

				if (participant.MetersMoved >= 100)
				{
					participant.MetersMoved += -100;
					MoveParticipants(participant);
				}
			}
		}

		public void MoveParticipants(IParticipant participant)
		{
			int i = 0;
			foreach (Section section in Track.Sections)
			{
				if (!participant.Finished)
				{
					SectionData sectionData = GetSectionData(section);
					if (participant == sectionData.Right) //Fix voor eerste participant die geskipt wordt.
					{
						participant.CurrentSection = section;
						LapsCount++;
					}

					if (section == participant.CurrentSection)
					{
						if (participant.Equipment.IsBroken == true)
						{
							return;
						}

						//SectionData sectionData = GetSectionData(section);


						if (sectionData.Right == participant)
						{
							sectionData.Right = null;
						}
						else if (sectionData.Left == participant)
						{
							sectionData.Left = null;
						}

						if (Track.Sections.Count <= (i + 1))
						{
							i = -1;
						}

						SectionData nextSectionData = GetSectionData(Track.Sections.ElementAt(i + 1));

						if (nextSectionData.Right == null)
						{
							nextSectionData.Right = participant;
						}
						else if (nextSectionData.Left == null)
						{
							nextSectionData.Left = participant;
						}

						participant.CurrentSection = Track.Sections.ElementAt(i + 1);
						CurrentSection = section; //Denk niet dat dit nodig is, maar staat er wel leuk.

						if (section.SectionTypes == SectionType.Finish && LapsCount >= 0) //They drove one lap.
						{
							NextLap(participant, nextSectionData);
						}

						if (section.SectionTypes == SectionType.Finish && LapsCount == -1) //First lap started.
						{
							LapsCount++;
						}
						return;
					}
					i++;
				}
			}
		}

		#region StartPositions
		public void StartPositions(Track track, List<IParticipant> participants) //Place participants on their start position.
		{
			int sectionsLength = Track.Sections.Count;
			int participantsRemaining = Participants.Count;

			int index = 0;

			for (int i = Track.Sections.Count; i > 0; i--)
			{
				var section = Track.Sections.ElementAt(i - 1);
				var sectionData = GetSectionData(section);
				if (section.SectionTypes == SectionType.StartGrid)
				{
					if (participantsRemaining != 0)
					{
						sectionData.Right = participants[index];
						index++;
						participantsRemaining--;
					}
					if (participantsRemaining != 0)
					{
						sectionData.Left = participants[index];
						index++;
						participantsRemaining--;
					}
					GetSectionData(section);
				}
				else if (section.SectionTypes == SectionType.Finish)
				{
					break;
				}
				else if (sectionsLength == 0) //If there's no more sections available.
				{
					break;
				}
				else //Go back one section and add the participants there.
				{
					if (participantsRemaining != 0)
					{
						sectionData.Right = participants[index];
						index++;
						participantsRemaining--;
					}
					if (participantsRemaining != 0)
					{
						sectionData.Left = participants[index];
						index++;
						participantsRemaining--;
					}
					GetSectionData(section);
				}
				sectionsLength--;
			}
		}
		#endregion start
	}
}


