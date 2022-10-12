using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static Model.SectionData;
using static System.Collections.Specialized.BitVector32;
using Section = Model.Section;

namespace Controller
{
	public class Race
	{
		public event EventHandler ThresholdReached; //idk
		public event EventHandler<DriversChangedEventArgs> DriversChanged; //L5-5

		public Track Track { get; set; }
		public Section CurrentSection;
		public Section section;

		public List<IParticipant> Participants { get; set; }
		private Dictionary<Section, SectionData> _positions = new(); //Participants positions with left and right.

		public DateTime StartTime { get; set; }
		private Random _random;
		private System.Timers.Timer timer; //Timer

		public int amountOfLaps = 1; //Hier bepaal je hoeveel laps een race heeft.
		public int LapsCount = -1; //met -1 beginnen omdat de participants bij de eerste lap eerst over de finish gaan.

		public Race(Track track, List<IParticipant> participants)
		{
			Track = track;
			Participants = participants;
			_random = new Random(DateTime.Now.Millisecond);
			StartPositions(Track, Participants);

			//Timer
			timer = new System.Timers.Timer(2500); //Interval
			Start(); //Start timer
			timer.Elapsed += OnTimedEvent;
		}

		public void OnTimedEvent(object sender, EventArgs e)
		{
			//Stop();
			CheckForMoveDriver();
			//Start();
			DriversChanged?.Invoke(this, new DriversChangedEventArgs(Track)); //Dit is nodig om te laten refreshen.
		}

		public void Start() //Start Timer
		{
			StartTime = DateTime.Now;
			timer.Elapsed += OnTimedEvent; //Subscribe
			timer.AutoReset = true;
			timer.Enabled = true;
		}
		public void Stop() //Stop Timer
		{
			timer.Enabled = false;
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

		public int CountLaps(IParticipant participant)
		{
			participant.LapsCount++;
			LapsCount++;
			return LapsCount;
		}
		
		public void NextLap(IParticipant participant, SectionData sectionData)
		{
			if (participant.LapsCount <= amountOfLaps)
			{
				CountLaps(participant);
			}
			else if (participant.LapsCount == (amountOfLaps + 1))
			{
				participant.Finished = true;
			}
		}

		public void CheckForMoveDriver()
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
					if (section == participant.CurrentSection)
					{
						if (participant.Equipment.IsBroken == true)
						{
							return;
						}

						SectionData sectionData = GetSectionData(section);

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
			//Dit misschien globaal declareren.
			int sectionsLength = Track.Sections.Count;
			int participantsRemaining = Participants.Count; //Keep track of amount of participants remaining.

			int index = 0;

			for (int i = Track.Sections.Count; i > 0; i--)
			{
				var section = Track.Sections.ElementAt(i - 1); //Get the section at the current index. ElementAt > Element
				var sectionData = GetSectionData(section);
				if (section.SectionTypes == SectionType.StartGrid)
				{
					if (participantsRemaining != 0) //Avoid trying to add a non-existing participant to a section.
					{
						sectionData.Right = participants[index];
						index++; //Increase index.
						participantsRemaining--;
					}
					if (participantsRemaining != 0) //Avoid trying to add a non-existing participant to a section.
					{
						sectionData.Left = participants[index];
						index++; //Increase index.
						participantsRemaining--;
					}
					GetSectionData(section); //Add participants to _positions
				}
				else if (section.SectionTypes == SectionType.Finish)
				{
					break; //Race ended.
				}
				else if (sectionsLength == 0) //If there's no more sections available.
				{
					break; //No more space left for participants.
				}
				else //Go back one section and add the participants there.
				{
					if (participantsRemaining != 0) //Avoid trying to add a non-existing participant to a section.
					{
						sectionData.Right = participants[index];
						index++; //Increase index.
						participantsRemaining--;
					}
					if (participantsRemaining != 0) //Avoid trying to add a non-existing participant to a section.
					{
						sectionData.Left = participants[index];
						index++; //Increase index.
						participantsRemaining--;
					}
					GetSectionData(section); //Add participant(s) to _positions
				}
				sectionsLength--;
			}
		}
		#endregion start
	}
}


