using Controller;
using System.ComponentModel;


namespace WPF
{
	public class CurrentRaceStatisticsDataContext : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public Race CurrentRace { get; set; }
		

		private void OnPropertyChanged()
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
		}
	}
}
