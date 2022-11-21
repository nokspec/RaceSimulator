using Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Competition
	{
		public List<IParticipant> Participants { get; set; }
		public Queue<Track> Tracks { get; set; }
		public List<IParticipant> FinishedParticipants { get; set; }

		//generic
		public RWRaceData<ParticipantsPoints> RaceData { get; set; }


		public Competition()
		{
			Participants = new();
			Tracks = new();
		}

		public void FinalScore(List<IParticipant> FinishedParticipants)
		{
			//add points to finished participants
			int i = 0;
			foreach (IParticipant participant in FinishedParticipants)
			{
				i++;
				switch (i)
				{
					case 1:
						participant.Points += 10;
						break;
					case 2:
						participant.Points += 5;
						break;
					case 3:
						participant.Points += 3;
						break;
					case 4:
						participant.Points += 1;
						break;
				}
			}
		}

		public Track NextTrack()
		{
			if (Tracks.Count > 0)
			{
				return Tracks.Dequeue();
			}
			else
			{
				return null;
			}
		}
	}
}
