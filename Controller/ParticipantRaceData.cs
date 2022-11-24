using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public class ParticipantRaceData
	{
		public Section CurrentSection { get; set; }
		public int SectionCount { get; set; }
		public int LapCount { get; set; }
		public string Name { get; set; }
		public string ImageSource { get; set; }

		public ParticipantRaceData(IParticipant participant)
		{
			SectionCount = participant.SectionCount;
			LapCount = participant.LapsCount;
			CurrentSection = participant.CurrentSection;

			ImageSource = DataContext.UrlCarImage(participant.TeamColors);
			Name = participant.Name;
		}

		public static List<ParticipantRaceData> SortByPosition(List<ParticipantRaceData> list)
		{
			return list.OrderByDescending(x => x.SectionCount).ThenByDescending(x => x.LapCount).ToList();
		}
	}
}
