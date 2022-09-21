using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
			_competition.Tracks.Enqueue(new Track("Turkey", new LinkedList<Section>()));
			Track result = _competition.NextTrack();
			Assert.AreEqual("Turkey", result.Name);
		}

		[Test]
		public void NextTrack_OneInQueue_RemoveTrackFromQueue()
		{
			_competition.Tracks.Enqueue(new Track("Japan", new LinkedList<Section>()));
			Track result = _competition.NextTrack();
			result = _competition.NextTrack(); //niet nog een keer Track ervoor omdat result hierboven al defined is.
			Assert.IsNull(result);
		}

		[Test]
		public void NextTrack_TwoInQueue_ReturnNextTrack()
		{
			_competition.Tracks.Enqueue(new Track("Japan", new LinkedList<Section>()));
			Track result = _competition.NextTrack();
			Assert.AreEqual("Japan", result.Name);
			_competition.Tracks.Enqueue(new Track("Turkey", new LinkedList<Section>()));
			result = _competition.NextTrack(); //niet nog een keer Track ervoor omdat result hierboven al defined is.
			Assert.AreEqual("Turkey", result.Name);
		}
	}
}
