using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public class ParticipantCompetitionData
	{
		public string Name { get; private set; }
		public int Points { get; private set; }
		public string ImageSource { get; private set; }

		public ParticipantCompetitionData(IParticipant participant)
		{
			Name = participant.Name;
			Points = participant.Points;
			ImageSource = DataContext.UrlCarImage(participant.TeamColors);
		}

		/// <summary>
		/// Returns a list of Participants sorted by their points (descending).
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static List<ParticipantCompetitionData> SortByPoints(List<ParticipantCompetitionData> list)
		{
			return list.OrderByDescending(x => x.Points).ToList();
		}
	}
}
