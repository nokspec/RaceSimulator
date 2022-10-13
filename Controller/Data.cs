using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;


namespace Controller
{
	public static class Data
	{
		public static Competition Competition;
		public static Race CurrentRace { get; set; }


		public static void Initialize()
		{
			Competition = new Competition(); //Initialize competition.
			AddParticipants();
			AddTracks();
		}

		public static void AddParticipants()
		{
			Competition.Participants.Add(new Driver("Kees", 0, new Car(20, 10, 10, false), TeamColors.Red));
			Competition.Participants.Add(new Driver("Jan", 0, new Car(10, 10, 10, false), TeamColors.Green));
			//Competition.Participants.Add(new Driver("Pieter", 0, new Car(10, 10, 10, false), TeamColors.Blue));
			//Competition.Participants.Add(new Driver("Uno", 0, new Car(10, 10, 10, false), TeamColors.Yellow));
			//Competition.Participants.Add(new Driver("Hendrik", 0, new Car(10, 10, 10, false), TeamColors.Yellow));
			//Competition.Participants.Add(new Driver("Tokyo", 0, new Car(10, 10, 10, false), TeamColors.Yellow));
			//Competition.Participants.Add(new Driver("Naoki", 0, new Car(10, 10, 10, false), TeamColors.Yellow));
		}


		public static void AddTracks()
		{
			Competition.Tracks.Enqueue(new Track("Rechtsom", MakeRace("Rechtsom")));
			//Competition.Tracks.Enqueue(new Track("Linksom", MakeRace("Linksom")));
			//Competition.Tracks.Enqueue(new Track("TestAlles", MakeRace("TestAlles"))); 
		}

		public static void NextRace()
		{
			Track currentTrack = Competition.NextTrack();
			if (currentTrack != null)
			{
				CurrentRace = new Race(currentTrack, Competition.Participants);
			}
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
			else if (naam.Equals("Linksom"))
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
			else if (naam.Equals("TestAlles"))
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
