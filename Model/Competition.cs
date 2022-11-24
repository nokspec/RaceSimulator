namespace Model
{
	public class Competition
	{
		public List<IParticipant> Participants { get; set; }
		public Queue<Track> Tracks { get; set; }
		public List<IParticipant> FinishedParticipants { get; set; }

		public Competition()
		{
			Participants = new List<IParticipant>();
			Tracks = new Queue<Track>();
		}

		public Track NextTrack() { if (Tracks.Count > 0) return Tracks.Dequeue(); else return null; }
	}
}
