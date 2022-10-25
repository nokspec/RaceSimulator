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

	/*	public string TrackName //get the Track.Name using lambda expression and set the value.
		{
			get => Data.CurrentRace.Track.Name;
		}*/

		public string TrackName { get; set; }

		public void OnDriversChanged(object sender, DriversChangedEventArgs e)
		{
			//TrackName = e.Track.Name;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TrackName)));
		}
	}
}
