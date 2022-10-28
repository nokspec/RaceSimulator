using Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Competition
	{
		public List<IParticipant> Participants { get; set; }
		public Queue<Track> Tracks { get; set; }

		//generic
		//iparticipant want we willen de data daaruit gebruiken
		public RWRaceData<IParticipant> RaceData { get; set; }
		//RWRaceData = data template
		
		public List<IParticipant> FinishedParticipants { get; set; }
		
		public Competition()
		{
			Participants = new List<IParticipant>();
			Tracks = new Queue<Track>();
			RaceData = new RWRaceData<IParticipant>();
		}

		public void FinalScore(List<IParticipant> FinishedParticipants)
		{
			/*//cast
			RaceData.Add((IParticipant)FinishedParticipants);
*/
			int count = 0;

			foreach (IParticipant participant in FinishedParticipants)
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
