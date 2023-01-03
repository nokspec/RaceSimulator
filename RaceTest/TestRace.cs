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
			bool isRandomized = true;

			Data.CurrentRace?.StartPositions(Data.CurrentRace.Participants);

			//Check if there are participants that aren't randomized.
			foreach (IParticipant participant in Data.CurrentRace.Participants)
			{
				if (participant.Equipment.Speed == 0 || participant.Equipment.Quality == 0 || participant.Equipment.Performance == 0)
				{
					isRandomized = false;
				}
			}
			Assert.That(isRandomized, Is.True);
		}

		//amount of laps has to be 1 for the following tests.

		[Test]
		public void FinishedParticipantsShouldBeAdded()
		{
			Data.CurrentRace?.StartPositions(Data.CurrentRace.Participants);
			Data.CurrentRace?.Start();
			foreach (IParticipant participant in Data.CurrentRace.Participants)
			{
				Data.CurrentRace?.NextLap(participant);
				Data.CurrentRace?.NextLap(participant);
			}
			Data.CurrentRace.AddParticipantToFinishedParticipants();
			Assert.That(Data.CurrentRace.FinishedParticipants.Count, Is.EqualTo(Data.CurrentRace.Participants.Count));
		}

		[Test]
		public void ParticipantsShouldGetPoints()
		{
			//test if participants get points
			Data.CurrentRace?.StartPositions(Data.CurrentRace.Participants);
			Data.CurrentRace?.Start();
			foreach (IParticipant participant in Data.CurrentRace.Participants)
			{
				Data.CurrentRace?.NextLap(participant);
				Data.CurrentRace?.NextLap(participant);
			}
			Data.CurrentRace.AddParticipantToFinishedParticipants();
			
			Data.CurrentRace.CompetitionPointsDistribution(Data.CurrentRace.FinishedParticipants);

			Assert.That(Data.CurrentRace.FinishedParticipants[0].Points, Is.EqualTo(25));
			Assert.That(Data.CurrentRace.FinishedParticipants[1].Points, Is.EqualTo(18));
			Assert.That(Data.CurrentRace.FinishedParticipants[2].Points, Is.EqualTo(15));
		}
	}
}