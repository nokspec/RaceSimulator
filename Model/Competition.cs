using System.ComponentModel;

namespace Model
{
	public class Competition
	{
		public List<IParticipant> Participants { get; set; }
		public Queue<Track> Tracks { get; set; }
		public BindingList<Track> TracksBindingList { get; set; }

		public Competition()
		{
			Participants = new List<IParticipant>();
			Tracks = new Queue<Track>();
			TracksBindingList = new BindingList<Track>();
		}

		public BindingList<Track> QToBindingList()
		{
			foreach (Track track in Tracks)
			{
				TracksBindingList.Add(track);
			}
			return TracksBindingList;
		}

		public Track NextTrack() { if (Tracks.Count > 0) return Tracks.Dequeue(); return null; }
	}
}
