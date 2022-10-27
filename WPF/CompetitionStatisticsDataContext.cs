using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF
{
	public class CompetitionStatisticsDataContext : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		

		private void OnPropertyChanged()
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
		}
	}
}
