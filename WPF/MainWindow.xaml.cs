﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
		private RaceStatistics _raceStatistics;
		private CompetitionStatistics _competitionStatistics;

		public MainWindow()
		{
			Initialize();
		}

		public void Initialize()
		{
			Data.Initialize();
			Data.NextRaceEvent += OnNextRaceEvent;
			ImageManager.Initialize();
			Data.NextRace();
			InitializeComponent();
		}

		public void OnNextRaceEvent(object sender, NextRaceEventArgs e)
		{
			ImageManager.ClearCache();
			VisualizationWPF.Initialize(e.Race);

			if (Data.CurrentRace != null) Data.CurrentRace.DriversChanged += OnDriversChanged!;
		}

		public void OnDriversChanged(object source, DriversChangedEventArgs e)
		{
		this.Dispatcher.BeginInvoke(
				DispatcherPriority.Render,
				new Action(() =>
				{
					this.ImageComponent.Source = null;
					this.ImageComponent.Source = VisualizationWPF.DrawTrack(e.Track);
				}));
		}

		private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void Race_Statistics_Click(object sender, RoutedEventArgs e)
		{
			//Initialize window
			_raceStatistics = new RaceStatistics();

			_raceStatistics.Show();
		}

		private void Competition_Statistics_Click(object sender, RoutedEventArgs e)
		{
			//Initialize window
			_competitionStatistics = new CompetitionStatistics();

			_competitionStatistics.Show();
		}
	}
}
