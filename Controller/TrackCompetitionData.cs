using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public class TrackCompetitionData
	{
		public Track Track { get; private set; }
		public string TrackName { get; private set; }

		public TrackCompetitionData(Track track)
		{
			TrackName = track.Name;
		}

		public static List<TrackCompetitionData> RemainingTracks(List<TrackCompetitionData> list)
		{
			return list.OrderByDescending(x => x.TrackName).ToList();
		}
	}
}
