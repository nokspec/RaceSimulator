using System;
using System.Collections.Generic;
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
			Competition competition = new Competition(); //initialiseer competition
			AddParticipants();
			AddTracks();
		}

		public static void AddParticipants()
		{
			Competition.Participants.Add(new Driver("Mike", 0, new Car(10, 10, 10, false), TeamColors.Red));
			Competition.Participants.Add(new Driver("Jan", 0, new Car(10, 10, 10, false), TeamColors.Green));
			Competition.Participants.Add(new Driver("Pieter", 0, new Car(10, 10, 10, false), TeamColors.Blue));
			Competition.Participants.Add(new Driver("Thomas", 0, new Car(10, 10, 10, false), TeamColors.Grey));
			Competition.Participants.Add(new Driver("Yasmine", 0, new Car(10, 10, 10, false), TeamColors.Yellow));
		}


		public static void AddTracks()
		{
			Competition.Tracks.Enqueue(new Track("Monza", new LinkedList<Section>()));
		}

		public static void NextRace()
		{
			if (Competition.NextTrack() != null)
			{
				CurrentRace = new Race(Competition.NextTrack(), Competition.Participants);
			}
		}
	}
}
