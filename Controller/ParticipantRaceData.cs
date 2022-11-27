using Model;

namespace Controller
{
	public class ParticipantRaceData
	{
		public Section CurrentSection { get; set; }
		public int SectionCount { get; set; }
		public int BrokenCount { get; set; }
		public bool IsFinished { get; set; }
		public string Name { get; set; }
		public string ImageSource { get; set; }

		public ParticipantRaceData(IParticipant participant)
		{
			SectionCount = participant.SectionCount;
			CurrentSection = participant.CurrentSection;
			BrokenCount = participant.BrokenCount;
			IsFinished = participant.Finished;
				
			ImageSource = DataContext.UrlCarImage(participant.TeamColors);
			Name = participant.Name;
		}

		/// <summary>
		/// Returns a list of Participants sorted by their position.
		/// First by the amount of sections they've been through, secondly by the amount of laps they've driven.
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static List<ParticipantRaceData> SortByPosition(List<ParticipantRaceData> list)
		{
			return list.OrderByDescending(x => x.SectionCount).ToList();
		}
	}
}
