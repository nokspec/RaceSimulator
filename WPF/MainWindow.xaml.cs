using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Controller;
using Model;

namespace WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent(); //gwn laten
			
			Data.Initialize();
			ImageManager.Initialize();
			Data.NextRaceEvent += OnNextRaceEvent;
			Data.NextRace();
			
		}

		public void OnNextRaceEvent(object sender, NextRaceEventArgs e)
		{
			ImageManager.ClearCache();
			VisualizationWPF.Initialize(e.Race);
			
			Data.CurrentRace.DriversChanged += OnDriversChanged;
		}

		public void OnDriversChanged(object source, DriversChangedEventArgs e)
		{
			this.ImageComponent.Dispatcher.BeginInvoke(
				DispatcherPriority.Render,
				new Action(() =>
				{
					this.ImageComponent.Source = null;
					this.ImageComponent.Source = VisualizationWPF.DrawTrack(e.Track);
				}));
		}
	}
}
