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

		public List<IParticipant> Participants { get; set; }
		public List<IParticipant> FinishedParticipants { get; set; }

		private Dictionary<Section, SectionData> _positions;

		//Events
		public event EventHandler<DriversChangedEventArgs> DriversChanged;
		public event EventHandler<NextRaceEventArgs> RaceFinished;

		//Timer
		public DateTime StartTime { get; set; }
		private Random _random;
		private System.Timers.Timer _timer; //Timer
		private static int _timerInterval = 500; //Set timer interval

		//Laps
		public static int AmountOfLaps = 2; //Set amount of laps a race has.
		public static int LapsCount = -1; //Startgrid sits behind the finish, that's why the value is -1.

		public Race(Track track, List<IParticipant> participants)
		{
			Track = track;
			Participants = participants;
			_random = new Random(DateTime.Now.Millisecond);
			_positions = new();
			FinishedParticipants = new();
			StartPositions(Track, Participants);
			RandomizeEquipment();

			//Timer
			_timer = new System.Timers.Timer(_timerInterval); //Interval
			Start(); //Start timer
		}

		public void Start()
		{
			StartTime = DateTime.Now;
			_timer.Elapsed += OnTimedEvent; //Subscribe
			_timer.Start();
		}

		/// <summary>
		/// Gets called when a race finishes.
		/// Cleans everything for the next race.
		/// </summary>
		private void CleanUp()
		{
			_timer.Stop();
			DriversChanged = null;
			RaceFinished = null;
			CurrentSection = null;
			LapsCount = -1;
			AmountOfLaps = 0;

			FinishedParticipants.Clear();
			ResetParticipants();
		}

		/// <summary>
		/// Resets all participants values to default values.
		/// </summary>
		public void ResetParticipants()
		{
			foreach (IParticipant participant in Participants)
			{
				participant.MetersMoved = 0;
				participant.Finished = false;
				participant.LapsCount = -1;
				participant.SectionCount = 0;
				participant.CurrentSection = null;
			}
		}

		protected void OnTimedEvent(object sender, EventArgs e)
		{
			CheckDriverMovement();
			RandomizeFixing();
			RandomizeIsBroken();
			DriversChanged?.Invoke(this, new DriversChangedEventArgs(Track)); 
			if (CheckRaceFinished()) //If the race has finished
			{
				AddParticipantToFinishedParticipants();
				ReturnStandings();
				CompetitionPointsDistribution(FinishedParticipants);
				RaceFinished?.Invoke(this, new NextRaceEventArgs());
				CleanUp(); 
			}
		}

		public SectionData GetSectionData(Section section)
		{
			if (!_positions.ContainsKey(section)) _positions.Add(section, new SectionData());
			return _positions[section];
		}

		#region Randomizing participants
		/// <summary>
		/// Randomizes the equipment of the participants.
		/// </summary>
		private void RandomizeEquipment()
		{
			foreach (IParticipant participant in Participants)
			{
				participant.Equipment.Quality = _random.Next(1, 10);
				participant.Equipment.Performance = _random.Next(1, 10);
				participant.Equipment.Speed = _random.Next(4, 10); //Toegevoegd bij 5-7
			}
		}

		/// <summary>
		/// Randomizes if a participant's equipment is broken.
		/// </summary>
		private void RandomizeIsBroken()
		{
			List<IParticipant> DrivingParticipants = new();
			foreach (IParticipant participant in Participants)
			{
				if (participant.Equipment.IsBroken == false)
					DrivingParticipants.Add(participant);
			}

			foreach (IParticipant participant in DrivingParticipants)
			{
				double chanceCalculation = (11 - (participant.Equipment.Quality * 0.5)) * 0.0005;
				//NextDouble geeft waarde tussen 0.0 en 1.0.
				if (_random.NextDouble() < chanceCalculation)
				{
					participant.Equipment.IsBroken = true;
				}
			}
		}

		/// <summary>
		/// Randomizes if a participant's equipment is fixed.
		/// </summary>
		private void RandomizeFixing()
		{
			foreach (IParticipant participant in Participants.Where(p => p.Equipment.IsBroken))
			{
				if (_random.NextDouble() < 0.05) //5% kans
				{
					participant.Equipment.IsBroken = false;

					//Als speed groter is dan 5 wordt de speed verlaagd.
					if (participant.Equipment.Speed > 7)
						participant.Equipment.Speed--;

					//Dit wordt altijd uitgevoerd.
					if (participant.Equipment.Quality > 1)
						participant.Equipment.Quality--;
				}
			}
		}
		#endregion

		#region Points management

		/// <summary>
		/// Distributes points to participants based on their position.
		/// </summary>
		/// <param name="finishedParticipants"></param>
		public void CompetitionPointsDistribution(List<IParticipant> finishedParticipants)
		{
			int count = 0;
			foreach (IParticipant participant in finishedParticipants)
			{
				{
					count++;
					if (count == 1) participant.Points += 25;
					if (count == 2) participant.Points += 18;
					if (count == 3) participant.Points += 15;
					if (count == 4) participant.Points += 12;
					if (count == 5) participant.Points += 10;
					if (count == 6) participant.Points += 8;
					if (count == 7) participant.Points += 6;
					if (count == 8) participant.Points += 4;
					if (count == 9) participant.Points += 2;
					if (count == 10) participant.Points += 1;
				}
			}
		}

		/// <summary>
		/// Adds participant to FinishedParticipants list in order of their position.
		/// </summary>
		public void AddParticipantToFinishedParticipants()
		{
			foreach (IParticipant participant in Participants)
			{
				//als de participant gefinished is EN de participant zit nog niet in FinishedParticipants
				if (participant.Finished && !FinishedParticipants.Contains(participant)) FinishedParticipants.Add(participant);
			}
		}

		public List<IParticipant> ReturnStandings()
		{
			return FinishedParticipants;
		}
		#endregion

		#region Lap management
		/// <summary>
		/// Gets called by NextLap()
		/// Adds +1 to participant.LapsCount & _lapsCount. Returns _lapsCount.
		/// </summary>
		/// <param name="participant"></param>
		/// <returns></returns>
		private int CountLaps(IParticipant participant)
		{
			participant.LapsCount++;
			LapsCount++;
			return LapsCount;
		}

		/// <summary>
		/// Gets called by MoveParticipants() after the participants have completed one lap.
		/// Checks if a participant has finished and otherwise calls CountLaps
		/// </summary>
		/// <param name="participant"></param>
		/// <param name="sectionData"></param>
		private void NextLap(IParticipant participant, SectionData sectionData)
		{
			if (participant.LapsCount < AmountOfLaps)
			{
				CountLaps(participant);
			}
			else if (participant.LapsCount == (AmountOfLaps))
			{
				participant.Finished = true;
			}
		}

		/// <summary>
		/// Gets called by OnTimedEvent().
		/// Checks if all participants have finished.
		/// Returns a bool.
		/// </summary>
		/// <returns></returns>
		private bool CheckRaceFinished()
		{
			int count = 0;
			foreach (IParticipant participant in Participants)
			{
				if (participant.Finished == true) count++;
			}
			if (count == Participants.Count) return true;
			
			else return false;
			
		}
		#endregion

		#region Move participants
		
		/// <summary>
		/// Checks if a participant has moved to an other section. 
		/// If it has, MoveParticipants() is called.
		/// </summary>
		public void CheckDriverMovement()
		{
			foreach (IParticipant participant in Participants)
			{
				participant.MetersMoved += participant.CalculateSpeed();
				if (participant.MetersMoved >= 100)
				{
					participant.MetersMoved += -100;
					MoveParticipants(participant);
				}
			}
		}

		/// <summary>
		/// Gets called by CheckDriverMovement(). 
		/// Checks where the participant has to go and puts that participant in the next section.
		/// Calls AddParticipantToFinishedParticipants() after a participant finishes the race to add them to the List FinishedParticipants.
		/// When a participant finishes the race, they get removed from the track.
		/// </summary>
		/// <param name="participant"></param>
		public void MoveParticipants(IParticipant participant)
		{
			int i = 0;
			foreach (Section section in Track.Sections)
			{
				SectionData sectionData = GetSectionData(section);
				if (!participant.Finished)
				{
					if (participant == sectionData.Right || participant == sectionData.Left) //Make sure participants move
					{
						participant.CurrentSection = section;
						LapsCount++;
					}

					if (section == participant.CurrentSection)
					{
						//If participant is broken
						if (participant.Equipment.IsBroken == true) return;

						//Remove participant from currentsection
						if (sectionData.Right == participant)
						{
							participant.SectionCount++; //To display drivers in correct order on race statistics screen
							sectionData.Right = null;
						}

						else if (sectionData.Left == participant)
						{
							participant.SectionCount++;
							sectionData.Left = null;
						}

						if (Track.Sections.Count <= (i + 1)) i = -1;

						//Get SectionData of next Section.
						SectionData nextSectionData = GetSectionData(Track.Sections.ElementAt(i + 1));

						//If the next section has a free spot.
						if (nextSectionData.Right == null)
							nextSectionData.Right = participant;
						else if (nextSectionData.Left == null)
							nextSectionData.Left = participant;

						participant.CurrentSection = Track.Sections.ElementAt(i + 1);

						if (section.SectionTypes == SectionType.Finish && LapsCount >= 0) NextLap(participant, nextSectionData); //They have driven one lap.

						if (section.SectionTypes == SectionType.Finish && LapsCount == -1) LapsCount++; //First lap has started.	
						return;
					}
					i++;
				}
				else
				{
					//Adds all but the last participant to FinishedParticipants
					AddParticipantToFinishedParticipants();

					//Remove participant from track when finished
					if (sectionData.Right == participant)
						sectionData.Right = null;
					if (sectionData.Left == participant)
						sectionData.Left = null;
				}
			}
		}
		#endregion

		#region StartPositions
		/// <summary>
		/// Places the participants on the track, prior to the race beginning.
		/// </summary>
		/// <param name="track"></param>
		/// <param name="participants"></param>
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
					break;
				else if (sectionsLength == 0) //If there's no more sections available.
					break;
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

		/// <summary>
		/// Can be used when debugging with timer enabled.
		/// </summary>
		private void Stop()
		{
			_timer.Enabled = false;
		}
	}
}


