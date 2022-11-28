using System.ComponentModel.DataAnnotations;

namespace Model
{
	public interface IParticipant
	{
		string Name { get; set; }
		int Points { get; set; }
		IEquipment Equipment { get; set; }
		TeamColors TeamColors { get; set; }
		TeamColors TeamColorsSave { get; set; }
		public int LapsCount { get; set; }
		public bool Finished { get; set; }
		public Section CurrentSection { get; set; }
		public int CalculateSpeed();
		public int MetersMoved { get; set; }
		public int DistanceTravelled { get; set; }
		public int SectionCount { get; set; }
		public int BrokenCount { get; set; }

		public bool IsFireball
		{
			//TODO refactor
			get { return false; }
			set
			{
				if (TeamColors != TeamColors.Fire)
				{
					TeamColorsSave = TeamColors;
				}
				if (value)
				{
					TeamColors = TeamColors.Fire;
				}
				else
				{
					TeamColors = TeamColorsSave;
				}
			}
		}
	}

	public enum TeamColors
	{
		Red,
		Green,
		Yellow,
		Grey,
		Blue,
		Fire //If speed is above certain amount, driver gets fire image
	}
}

