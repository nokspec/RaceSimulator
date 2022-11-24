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
		private static string _urlDefaultCar = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\North\\";

		public event PropertyChangedEventHandler? PropertyChanged;
		private string _trackName { get; set; }
		public string TrackName { get { return _trackName; } set { _trackName = value; OnPropertyChanged(); } } //data bind aan label in maiwindow.xaml.cs
		private List<ParticipantCompetitionData> _participantsCompetition => CreateParticipantCompetitionList(Data.Competition.Participants);
		public List<ParticipantCompetitionData> ParticipantsCompetition { get { return _participantsCompetition; } private set { } }
		private List<ParticipantRaceData> _participantsRace => CreateParticipantRaceList(Data.Competition.Participants);
		public List<ParticipantRaceData> ParticipantsRace { get { return _participantsRace; } private set { } }

		public DataContext()
		{
			Data.CurrentRace.DriversChanged += OnDriversChanged;
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
		private List<ParticipantCompetitionData> CreateParticipantCompetitionList(List<IParticipant> participants)
		{
			// Hier word een List gemaakt van ParticipantCompetitionData
			List<ParticipantCompetitionData> list = new List<ParticipantCompetitionData>();
			foreach (IParticipant participant in participants)
			{
				list.Add(new ParticipantCompetitionData(participant));
			}
			list = ParticipantCompetitionData.SortByPoints(list);
			return list;
		}

		private List<ParticipantRaceData> CreateParticipantRaceList(List<IParticipant> participants)
		{
			List<ParticipantRaceData> list = new List<ParticipantRaceData>();
			foreach (IParticipant participant in participants)
			{
				list.Add(new ParticipantRaceData(participant));
			}
			list = ParticipantRaceData.SortByPosition(list);
			return list;
		}

		public static string UrlCarImage(TeamColors teamColor)
		{
			switch(teamColor)
			{
				case TeamColors.Blue:
					return _urlDefaultCar + "BlueNorth.png";
				case TeamColors.Green:
					return _urlDefaultCar + "GreenNorth.png";
				case TeamColors.Red:
					return _urlDefaultCar + "RedNorth.png";
				case TeamColors.Yellow:
					return _urlDefaultCar + "YellowNorth.png";
				default:
					return _urlDefaultCar + "GreyNorth.png";
			}
		}
	}
}
