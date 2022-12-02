using Model;


namespace ControllerTest
{
	[TestFixture]
	public class Model_Competition_NextTrackShould
	{
		private Competition _competition;

		[SetUp]
		public void SetUp()
		{
			_competition = new Competition();
		}

		[Test]
		public void NextTrack_EmptyQueue_ReturnNull()
		{
			Track result = _competition.NextTrack();
			Assert.IsNull(result);
		}

		[Test]
		public void NextTrack_OneInQueue_ReturnTrack()
		{
			_competition.Tracks.Enqueue(new Track("Test", new SectionType[] { SectionType.Straight }));
			Track result = _competition.NextTrack();
			Assert.AreEqual("Test", result.Name);
		}

		[Test]
		public void NextTrack_OneInQueue_RemoveTrackFromQueue()
		{
			_competition.Tracks.Enqueue(new Track("Test", new SectionType[] { SectionType.Straight }));
			var result = _competition.NextTrack();
			result = _competition.NextTrack();
			Assert.IsNull(result);
		}

		[Test]
		public void NextTrack_TwoInQueue_ReturnNextTrack()
		{
			_competition.Tracks.Enqueue(new Track("Test1", new SectionType[] { SectionType.Straight }));
			_competition.Tracks.Enqueue(new Track("Test2", new SectionType[] { SectionType.Straight }));
			_competition.NextTrack();
			Track result = _competition.NextTrack();
			Assert.AreEqual("Test2", result.Name);
		}
	}
}
