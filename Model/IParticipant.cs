﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public interface IParticipant
	{
		string Name { get; set; }
		int Points { get; set; }
		IEquipment Equipment { get; set; }
		TeamColors TeamColors { get; set; }
		public int LapsCount { get; set; }
		public bool Finished { get; set; } //bool is standaard false.
		public Section CurrentSection { get; set; }
		public int GetMovementSpeed();
		public int MetersMoved { get; set; }
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
