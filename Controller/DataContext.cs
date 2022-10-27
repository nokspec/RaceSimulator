using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public class DataContext : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		private string _trackName { get; set; }
		public string TrackName { get { return _trackName; } set { _trackName = value; OnPropertyChanged(); } } //data bind aan label in maiwindow.xaml.cs

		public DataContext()
		{
			Data.CurrentRace.RaceFinished += OnRaceFinished;
			TrackName = Data.CurrentRace.Track.Name;
		}
		public void OnDriversChanged(object sender, DriversChangedEventArgs e)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));

		}

		public void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
		}

		public void OnRaceFinished(object sender, NextRaceEventArgs e)
		{
			TrackName = Data.CurrentRace.Track.Name;
		}
	}
}
