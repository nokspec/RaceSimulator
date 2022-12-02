using System.ComponentModel;

namespace Model
{
	public class Competition
	{
		public List<IParticipant> Participants { get; set; }
		public Queue<Track> Tracks { get; set; }
		public BindingList<Track> TracksBList { get; set; }

		public Competition()
		{
			Participants = new();
			Tracks = new();
			TracksBList = new();
		}

		public BindingList<Track> Q2BindingList()
		{
			foreach (Track track in Tracks)
			{
				TracksBList.Add(track);
			}
			return TracksBList;
		}

		public Track NextTrack() { if (Tracks.Count > 0) return Tracks.Dequeue(); return null; }
	}
}
