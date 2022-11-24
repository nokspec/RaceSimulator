using Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Controller
{
	public class DataContext : INotifyPropertyChanged
	{
		private static string _urlDefaultCar = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\West\\";

		public event PropertyChangedEventHandler? PropertyChanged;
		private string _trackName { get; set; }
		public string TrackName { get { return _trackName; } set { _trackName = value; OnPropertyChanged(); } }
		private List<ParticipantCompetitionData> _participantsCompetition => CreateParticipantCompetitionList(Data.Competition.Participants);

		//Used in XAML
		public List<ParticipantCompetitionData> ParticipantsCompetition { get { return _participantsCompetition; } private set { } }
		private List<ParticipantRaceData> _participantsRace => CreateParticipantRaceList(Data.Competition.Participants);
		
		//Used in XAML
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
			Data.CurrentRace.DriversChanged += OnDriversChanged;
			Data.CurrentRace.RaceFinished += OnRaceFinished;
		}

		/// <summary>
		/// Creates a list of ParticipantCompetitionData objects from a list of participants
		/// </summary>
		/// <param name="participants"></param>
		/// <returns></returns>
		private List<ParticipantCompetitionData> CreateParticipantCompetitionList(List<IParticipant> participants)
		{
			List<ParticipantCompetitionData> list = new List<ParticipantCompetitionData>();
			foreach (IParticipant participant in participants)
			{
				list.Add(new ParticipantCompetitionData(participant));
			}
			list = ParticipantCompetitionData.SortByPoints(list);
			return list;
		}

		/// <summary>
		/// Creates a list of ParticipantRaceData objects from a list of IParticipant objects
		/// </summary>
		/// <param name="participants"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Used to display the image of the car on the competition & race statistics windows.
		/// Returns the url of the car image based on the team colors
		/// </summary>
		/// <param name="teamColor"></param>
		/// <returns></returns>
		public static string UrlCarImage(TeamColors teamColor)
		{
			switch (teamColor)
			{
				case TeamColors.Blue:
					return _urlDefaultCar + "BlueWest.png";
				case TeamColors.Green:
					return _urlDefaultCar + "GreenWest.png";
				case TeamColors.Red:
					return _urlDefaultCar + "RedWest.png";
				case TeamColors.Yellow:
					return _urlDefaultCar + "YellowWest.png";
				default:
					return _urlDefaultCar + "GreyWest.png";
			}
		}
	}
}
