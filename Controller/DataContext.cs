using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public class DataContext : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		//public string TrackName => Data.CurrentRace.Track.Name;

		public void OnDriversChanged(object sender, DriversChangedEventArgs e)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
		}
	}
}
