﻿using System;
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
		private CurrentRaceStatistics _currentRaceStatistics;
		private CompetitionStatistics _competitionStatistics;
		
		public MainWindow()
		{
			InitializeComponent(); 
			
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

		//Closes application
		private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void Race_Statistics_Click(object sender, RoutedEventArgs e)
		{
			// initialize window
			_currentRaceStatistics = new CurrentRaceStatistics();

			// link next race event
			//Data.NextRaceEvent += ((RaceStatisticsDataContext)_currentRaceStatistics.DataContext).OnNextRace;

			// send current race to data context to show data mid race
			//((RaceStatisticsDataContext)_currentRaceStatistics.DataContext).OnNextRace(null, new NextRaceEventArgs() { Race = Data.CurrentRace });

			// show window
			_currentRaceStatistics.Show();
		}

		private void Competition_Statistics_Click(object sender, RoutedEventArgs e)
		{
			// initialize window
			_competitionStatistics = new CompetitionStatistics();

			// link next race event
			//Data.NextRaceEvent += ((CompetitionStatisticsDataContext)_competitionStatistics.DataContext).OnNextRace;

			// send current race to data context to show data mid race
			//((CompetitionStatisticsDataContext)_competitionStatistics.DataContext).OnNextRace(null, new NextRaceEventArgs() { Race = Data.CurrentRace });

			// show window
			_competitionStatistics.Show();
		}
	}
}
