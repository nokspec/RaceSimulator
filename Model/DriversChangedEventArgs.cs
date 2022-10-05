using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class DriversChangedEventArgs : EventArgs //Inherit EventArgs
	{
		public Track Track { get; set; }

		public DriversChangedEventArgs(Track track)
		{
			Track = track;

		}
	}
}
