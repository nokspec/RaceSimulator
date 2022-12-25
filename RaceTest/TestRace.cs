using Controller;
using Model;

namespace RaceTest
{
	[TestFixture]
	public class TestRace
	{
		[SetUp]
		public void Setup()
		{
			Data.Initialize();
			Data.NextRace();
		}

		[Test]
		public void ParticipantEquipmentShouldBeRandom()
		{
			bool IsRandomized = true;

			Data.CurrentRace.StartPositions(Data.CurrentRace.Participants);

			//Check if there are participants that aren't randomized.
			foreach (IParticipant participant in Data.CurrentRace.Participants)
			{
				if (participant.Equipment.Speed == 0 || participant.Equipment.Quality == 0 || participant.Equipment.Performance == 0)
				{
					IsRandomized = false;
				}
			}
			Assert.That(IsRandomized, Is.True);
		}
	}
}