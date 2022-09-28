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
			Competition = new Competition(); //initialize competition
			AddParticipants();
			AddTracks();
		}

		public static void AddParticipants()
		{
			Competition.Participants.Add(new Driver("Mike", 0, new Car(10, 10, 10, false), TeamColors.Red));
			Competition.Participants.Add(new Driver("Jan", 0, new Car(10, 10, 10, false), TeamColors.Green));
		}


		public static void AddTracks()
		{
			//Competition.Tracks.Enqueue(new Track("Rechtsom", MakeRace("Rechtsom")));
			//Competition.Tracks.Enqueue(new Track("Linksom", MakeRace("Linksom")));
			Competition.Tracks.Enqueue(new Track("TestAlles", MakeRace("TestAlles")));


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
					SectionType.StartGrid,
					SectionType.RightCorner,
					SectionType.Straight,
					SectionType.RightCorner,
					SectionType.Straight,
					SectionType.RightCorner,
					SectionType.Finish,
					SectionType.RightCorner
				};
				return trackBuilder;
			}
			else if (naam.Equals("Linksom"))
			{
				SectionType[] trackBuilder = new SectionType[]
				{

					SectionType.LeftCorner,
					SectionType.LeftCorner,
					SectionType.LeftCorner,
					SectionType.LeftCorner
				};
				return trackBuilder;
			}
			else if (naam.Equals("TestAlles"))
			{
				SectionType[] trackBuilder = new SectionType[]
				{
					SectionType.StartGrid,
					SectionType.Finish,
					SectionType.Straight,
					SectionType.RightCorner,
					SectionType.Straight,
					SectionType.LeftCorner,
					SectionType.Straight,
					SectionType.RightCorner,
					SectionType.Straight,
					SectionType.RightCorner,
					SectionType.Straight
				};
				return trackBuilder;
			}

			return null;
		}
	}
}
