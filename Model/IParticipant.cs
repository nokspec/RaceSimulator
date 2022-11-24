namespace Model
{
	public interface IParticipant
	{
		string Name { get; set; }
		int Points { get; set; }
		IEquipment Equipment { get; set; }
		TeamColors TeamColors { get; set; }
		public int LapsCount { get; set; }
		public bool Finished { get; set; }
		public Section CurrentSection { get; set; }
		public int CalculateSpeed();
		public int MetersMoved { get; set; }
		public int SectionCount { get; set; }
	}
	
	public enum TeamColors
	{
		Red,
		Green,
		Yellow,
		Grey,
		Blue
	}
}
