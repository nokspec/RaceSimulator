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
		public Track Track { get; set; }
		public Section CurrentSection;
		public Section section;

		public List<IParticipant> Participants { get; set; }
		private Dictionary<Section, SectionData> _positions;

		//Events
		public event EventHandler<DriversChangedEventArgs> DriversChanged;
		public event EventHandler RaceFinished;

		//Timer
		public DateTime StartTime { get; set; }
		private Random _random;
		private System.Timers.Timer _timer; //Timer
		private static int TimerInterval = 500; //Bepaal timer interval. Nummer bepalen hier vind ik wat mooier dan in de constructor.

		//Laps
		public static int _amountOfLaps = 1; //Hier bepaal je hoeveel laps een race heeft.
		public static int _lapsCount = -1; //Stargrid staat achter finish, daarom -1.

		public Race(Track track, List<IParticipant> participants)
		{
			Track = track;
			Participants = participants;
			_random = new Random(DateTime.Now.Millisecond);
			_positions = new();
			StartPositions(Track, Participants);
			RandomizeEquipment();

			//Timer
			_timer = new System.Timers.Timer(TimerInterval); //Interval
			Start(); //Start timer
		}

		/*
		 * 
		 */ 
		public void Start() 
		{
			StartTime = DateTime.Now;
			_timer.Elapsed += OnTimedEvent; //Subscribe
			_timer.Start();
		}

		/*
		 * Race finished, clean everything up for next race.
		 */
		private void CleanUp()
		{
			_timer.Stop();
			DriversChanged = null;
			RaceFinished = null;
			CurrentSection = null;
			_lapsCount = -1;
			_amountOfLaps = 0;

			foreach (IParticipant participant in Participants)
			{
				participant.Finished = false;
				participant.LapsCount = -1;
			}
		}

		protected void OnTimedEvent(object sender, EventArgs e)
		{
			//Stop(); //debuggen
			CheckDriverMovement();
			//_timer.Start(); //debuggen
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
				participant.Equipment.Quality = _random.Next(1, 10);
				participant.Equipment.Performance = _random.Next(3, 10);
			}
		}

		/*
		 * Gets called by NextLap()
		 * If this function gets called it adds +1 to participant.LapsCount & _lapsCount. Returns _lapsCount.
		 */
		private int CountLaps(IParticipant participant)
		{
			participant.LapsCount++;
			_lapsCount++;
			return _lapsCount;
		}

		/*
		 * Gets called by MoveParticipants() after the participants have completed one lap.
		 * Checks if a participant has finished and otherwise calls CountLaps
		 */
		//TODO: Maybe refactor this. (if, else ipv if, else if)
		private void NextLap(IParticipant participant, SectionData sectionData) 
		{
			if (participant.LapsCount < _amountOfLaps)
			{
				CountLaps(participant);
			}
			else if (participant.LapsCount == (_amountOfLaps))
			{
				participant.Finished = true;
			}
		}

		/*
		 * Gets called by OnTimeEvent().
		 * Checks if all participants have finished. Returns a bool.
		 */
		private bool CheckRaceFinished() 
		{
			int count = 0;
			foreach (IParticipant participant in Participants)
			{
				if (participant.Finished == true)
					count++;
			}
			if (count == Participants.Count)
			{
				Console.WriteLine("Race finished"); //Print to show that a race has finished
				return true;
			}
			else
			{
				return false;
			}
		}

		#region moveparticipants
		/*
		 * Checks if a participant has moved section. If it has, MoveParticipants() is called.
		 */
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

		//TODO: Better explanation.
		/*
		 * Gets called by CheckDriverMovement(). Checks where the participant has to go and puts the participant in the next section.
		 */
		public void MoveParticipants(IParticipant participant)
		{
			int i = 0;
			foreach (Section section in Track.Sections)
			{
				SectionData sectionData = GetSectionData(section);
				if (!participant.Finished)
				{
					if (participant == sectionData.Right) //Fix voor eerste participant die geskipt wordt.
					{
						participant.CurrentSection = section;
						_lapsCount++;
					}

					if (section == participant.CurrentSection)
					{
						if (participant.Equipment.IsBroken == true)
						{
							return;
						}

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

						if (section.SectionTypes == SectionType.Finish && _lapsCount >= 0) //They drove one lap.
						{
							NextLap(participant, nextSectionData);
						}

						if (section.SectionTypes == SectionType.Finish && _lapsCount == -1) //First lap started.
						{
							_lapsCount++;
						}
						return;
					}
					i++;
				}
				else
				{
					if (sectionData.Right == participant)
					{
						sectionData.Right = null;
					}
					else
					{
						sectionData.Left = null;
					}
					//Verwijder participant van de track nadat hij gefinished is.
				}
			}
		}
		#endregion

		#region StartPositions
		/*
		 * Places the participants on the track, prior to the race beginning.
		 */
		public void StartPositions(Track track, List<IParticipant> participants)
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
				else
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

		private void Stop() //Voor debuggen.
		{
			_timer.Enabled = false;
		}
	}
}


