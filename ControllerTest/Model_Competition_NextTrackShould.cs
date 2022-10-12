using Controller;
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
			_competition.Tracks.Enqueue(new Track("Test", new SectionType[] { SectionType.Straight }));
			Track result = _competition.NextTrack();
			Assert.IsNotNull(result);
		}

		[Test]
		public void NextTrack_OneInQueue_RemoveTrackFromQueue()
		{
			_competition.Tracks.Enqueue(new Track("Test", new SectionType[] { SectionType.Straight }));
			_competition.NextTrack();
			Assert.AreEqual(0, _competition.Tracks.Count);
		}

		[Test]
		public void NextTrack_TwoInQueue_ReturnNextTrack()
		{
			{
				_competition.Tracks.Enqueue(new Track("Test1", new SectionType[] { SectionType.Straight }));
				_competition.Tracks.Enqueue(new Track("Test2", new SectionType[] { SectionType.Straight }));
				_competition.NextTrack();
				Track result = _competition.NextTrack();
				Assert.AreEqual("Test2", result.Name);
			}
		}

		/*[Test]
		public void NextTrack_OneInQueue_ReturnTrack()
		{
			_competition.Tracks.Enqueue(new Track("Turkey", Data.MakeRace("Turkey")));
			Track result = _competition.NextTrack();
			Assert.AreEqual("Turkey", result.Name);
		}

		[Test]
		public void NextTrack_OneInQueue_RemoveTrackFromQueue()
		{
			_competition.Tracks.Enqueue(new Track("Japan", Data.MakeRace("Japan")));
			Track result = _competition.NextTrack();
			result = _competition.NextTrack(); //niet nog een keer Track ervoor omdat result hierboven al defined is.
			Assert.IsNull(result);
		}
		

		[Test]
		public void NextTrack_TwoInQueue_ReturnNextTrack()
		{
			_competition.Tracks.Enqueue(new Track("Japan", Data.MakeRace("Japan")));
			Track result = _competition.NextTrack();
			Assert.That("Japan", Is.EqualTo("Japan"));
			_competition.Tracks.Enqueue(new Track("Turkey", Data.MakeRace("Turkey")));
			result = _competition.NextTrack(); //niet nog een keer Track ervoor omdat result hierboven al defined is.
			Assert.AreEqual("Turkey", "Turkey");
		}*/
	}
}
