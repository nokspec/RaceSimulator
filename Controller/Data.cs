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
		public static Competition competition;
		public static Race? CurrentRace { get; set; }


		public static void Initialize()
		{
			competition = new Competition(); //initialize competition
			AddParticipants();
			AddTracks();
		}

		public static void AddParticipants()
		{
			competition.Participants.Add(new Driver("Mike", 0, new Car(10, 10, 10, false), TeamColors.Red));
			competition.Participants.Add(new Driver("Jan", 0, new Car(10, 10, 10, false), TeamColors.Green));
			competition.Participants.Add(new Driver("Pieter", 0, new Car(10, 10, 10, false), TeamColors.Blue));
			competition.Participants.Add(new Driver("Thomas", 0, new Car(10, 10, 10, false), TeamColors.Grey));
			competition.Participants.Add(new Driver("Yasmine", 0, new Car(10, 10, 10, false), TeamColors.Yellow));
		}


		public static void AddTracks()
		{
			competition.Tracks.Enqueue(new Track("Monza", new LinkedList<Section>()));
			competition.Tracks.Enqueue(new Track("Spain", new LinkedList<Section>()));
		}

		public static void NextRace()
		{
			if (competition.NextTrack() != null)
			{
				CurrentRace = new Race(competition.NextTrack(), competition.Participants);
			}
		}
	}
}
