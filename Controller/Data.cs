using Model;

namespace Controller
{
	public static class Data
	{
		public static Competition Competition;
		public static Race CurrentRace { get; set; }
		public static event EventHandler<NextRaceEventArgs> NextRaceEvent;

		public static void Initialize()
		{
			Competition = new Competition(); //Initialize competition.
			AddParticipants();
			AddTracks();
		}

		/// <summary>
		/// Adds participants to the competition.
		/// </summary>
		public static void AddParticipants()
		{
			Competition.Participants.Add(new Driver("Kees", 0, new Car(10, 10, 10, false), TeamColors.Red));
			Competition.Participants.Add(new Driver("Jan", 0, new Car(10, 10, 10, false), TeamColors.Green));
			Competition.Participants.Add(new Driver("Pieter", 0, new Car(10, 10, 10, false), TeamColors.Blue));
			//Competition.Participants.Add(new Driver("Uno", 0, new Car(10, 10, 10, false), TeamColors.Grey));
			//Competition.Participants.Add(new Driver("Hendrik", 0, new Car(10, 10, 10, false), TeamColors.Yellow));
			//Competition.Participants.Add(new Driver("Tokyo", 0, new Car(10, 10, 10, false), TeamColors.Red));
			//Competition.Participants.Add(new Driver("Naoki", 0, new Car(10, 10, 10, false), TeamColors.Green));
		}

		/// <summary>
		/// Adds tracks to the competition.
		/// </summary>
		public static void AddTracks()
		{
			Competition.Tracks.Enqueue(new Track("Rechtsom", MakeRace("Rechtsom")));
			//Competition.Tracks.Enqueue(new Track("Linksom", MakeRace("Linksom")));
			Competition.Tracks.Enqueue(new Track("TestAlles", MakeRace("TestAlles"))); 
			//Competition.Tracks.Enqueue(new Track("Test", MakeRace("Test"))); 
		}



		/// <summary>
		/// Gets called by OnFinishedRace.
		/// Sets the NextTrack and then checks if currentTrack isn't null. 
		/// If it isn't it will initialize and start the next race.
		/// </summary>
		public static void NextRace()
		{
			Track currentTrack = Competition.NextTrack();
			if (currentTrack != null) //If not null, create new race.
			{
				CurrentRace = new Race(currentTrack, Competition.Participants);

				CurrentRace.RaceFinished += OnFinishedRace;
				NextRaceEvent?.Invoke(null, new NextRaceEventArgs() { Race = CurrentRace });
				CurrentRace.Start();
			}
		}
		
		/// <summary>
		/// Calls NextRace() to initiate the next race.
		/// Is subscribed to CurrentRace.RaceFinished.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void OnFinishedRace(object sender, EventArgs e)
		{
			NextRace();
		} 
		
		public static SectionType[] MakeRace(string naam)
		{
			if (naam.Equals("Rechtsom"))
			{
				SectionType[] trackBuilder = new SectionType[]
				{
					SectionType.Finish,
					SectionType.Straight,
					SectionType.Straight,
					SectionType.RightCorner,
					SectionType.Straight,
					SectionType.Straight,
					SectionType.Straight,
					SectionType.RightCorner,
					SectionType.Straight,
					SectionType.Straight,
					SectionType.Straight,
					SectionType.RightCorner,
					SectionType.Straight,
					SectionType.Straight,
					SectionType.StartGrid,
					SectionType.RightCorner
				};
				return trackBuilder;
			}
			if (naam.Equals("Linksom"))
			{
				SectionType[] trackBuilder = new SectionType[]
				{
					SectionType.Finish,
					SectionType.Straight,
					SectionType.Straight,
					SectionType.LeftCorner,
					SectionType.Straight,
					SectionType.Straight,
					SectionType.Straight,
					SectionType.LeftCorner,
					SectionType.Straight,
					SectionType.Straight,
					SectionType.Straight,
					SectionType.LeftCorner,
					SectionType.Straight,
					SectionType.Straight,
					SectionType.StartGrid,
					SectionType.LeftCorner
				};
				return trackBuilder;
			}
			if (naam.Equals("TestAlles"))
			{
				SectionType[] trackBuilder = new SectionType[]
				{
					SectionType.Finish,
					SectionType.RightCorner,
					SectionType.LeftCorner,
					SectionType.Straight,
					SectionType.RightCorner,
					SectionType.Straight,
					SectionType.RightCorner,
					SectionType.Straight,
					SectionType.RightCorner,
					SectionType.LeftCorner,
					SectionType.Straight,
					SectionType.Straight,
					SectionType.RightCorner,
					SectionType.Straight,
					SectionType.RightCorner,
					SectionType.StartGrid
				};
				return trackBuilder;
			}
			return null;
		}
	}
}
