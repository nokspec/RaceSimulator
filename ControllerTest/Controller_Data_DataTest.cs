using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;

namespace ControllerTest
{
	[TestFixture]
	public class Controller_Data_DataTest
	{
		[SetUp]
		public void Setup()
		{
			Data.Initialize();
			Data.NextRace();
			Data.CurrentRace.StartPositions(Data.CurrentRace.Track, Data.CurrentRace.Participants);
		}

		[Test]
		public void CurrentRaceNotNull()
		{
			Race result = Data.CurrentRace;
			Assert.IsNotNull(result);
		}

		[Test]
		public void ParticipantsNotNull()
		{
			List<IParticipant> result = Data.CurrentRace.Participants;
			Assert.IsNotNull(result);
		}

		[Test]
		public void TrackNotNull()
		{
			Track result = Data.CurrentRace.Track;
			Assert.IsNotNull(result);
		}

		[Test]
		public void AllParticipantsAdded()
		{
			List<IParticipant> result = Data.CurrentRace.Participants;
			Assert.AreEqual(3, result.Count);
		}

		[Test]
		public void AllSectionsAdded()
		{
			Track result = Data.CurrentRace.Track;
			Assert.AreEqual(16, result.Sections.Count);
		}

		[Test]
		public void SectionDataNotNull()
		{
			SectionData result = Data.CurrentRace.GetSectionData(Data.CurrentRace.Track.Sections.First.Value);
			Assert.IsNotNull(result);
		}
	}
}
