using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
	[TestFixture]

	public class RaceShould
	{
		private Race race;

		private List<IParticipant> participants;

		private IEquipment equipment;
		private Track track;

		[SetUp]
		public void SetUp()
		{
			track = new Track("Rechtsom", new[]
			{
				SectionType.Finish,
				SectionType.Straight,
				SectionType.Straight,
				SectionType.RightCorner,
				SectionType.Straight,
				SectionType.Straight,
				SectionType.Straight,
				SectionType.RightCorner,
				SectionType.Straight,
				SectionType.Straight,
				SectionType.Straight,
				SectionType.RightCorner,
				SectionType.Straight,
				SectionType.Straight,
				SectionType.StartGrid,
				SectionType.RightCorner
			});

			participants = new List<IParticipant>();
			equipment = new Car(0, 0, 0, false);
			participants.Add(new Driver("Henk", 0, equipment, TeamColors.Blue));
			participants.Add(new Driver("Piet", 0, equipment, TeamColors.Blue));
			participants.Add(new Driver("Klaas", 0, equipment, TeamColors.Blue));
			race = new Race(track, participants);
		}

		[Test]
		public void Race_GetSectionData_ShouldReturnObject()
		{
			Section section = track.Sections.First?.Value;

			var result = race.GetSectionData(section);

			Assert.IsInstanceOf<SectionData>(result);
			Assert.IsNotNull(result);
		}

		[Test] //Deze nog fixen.
		public void Race_PlaceParticipant_ShouldPlaceParticipantOnSection()
		{
			Section section = new Section(SectionType.StartGrid);
			IParticipant participant = new Driver("a", 0, equipment, TeamColors.Blue);

			race.StartPositions(track, participants);
			var result = race.GetSectionData(section).Right;

			Assert.AreEqual(participant, result);
		}
	}
}
