namespace Model
{
	public class Driver : IParticipant
	{
		public string Name { get; set; }
		public int Points { get; set; }
		public IEquipment Equipment { get; set; }
		public TeamColors TeamColors { get; set; }
		public int LapsCount { get; set; }
		public bool Finished { get; set; }
		public Section CurrentSection { get; set; }
		public int MetersMoved { get; set; }
		public int SectionCount { get; set; }

		public Driver(string name, int points, IEquipment equipment, TeamColors teamColors)
		{
			Name = name;
			Points = points;
			Equipment = equipment;
			TeamColors = teamColors;
		}

		/// <summary>
		/// Calculates the speed of a driver.
		/// </summary>
		/// <returns></returns>
		public int CalculateSpeed()
		{
			return Equipment.Performance * Equipment.Speed;
		}
	}
}
