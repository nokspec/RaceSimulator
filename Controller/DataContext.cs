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
		public Track Track { get; set; }
		private List<ParticipantCompetitionData> _participantsCompetition => CreateParticipantCompetitionList(Data.Competition.Participants);

		public List<ParticipantCompetitionData> ParticipantsCompetition { get { return _participantsCompetition; } private set { } }

		private BindingList<Track> _competitionData { get; set; }
		public BindingList<Track> CompetitionData { get { return _competitionData; } set { _competitionData = value; OnPropertyChanged(); } }

		private List<ParticipantRaceData> _participantsRace => CreateParticipantRaceList(Data.Competition.Participants);

		public List<ParticipantRaceData> ParticipantsRace { get { return _participantsRace; } private set { } }


		public DataContext()
		{
			CreateTrackNameList();

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
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void OnRaceFinished(object sender, NextRaceEventArgs e)
		{
			CreateTrackNameList();

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
			List<ParticipantCompetitionData> list = (from participant in participants
													 select new ParticipantCompetitionData(participant)).ToList();
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
			List<ParticipantRaceData> list = (from participant in participants
											  select new ParticipantRaceData(participant)).ToList();
			list = ParticipantRaceData.SortByPosition(list);
			
			return list;
		}

		/// <summary>
		/// Creates a List of Tracks that are left to be driven on.
		/// </summary>
		private void CreateTrackNameList()
		{
			List<Track> competitionData = (from Track track in Data.Competition.Tracks
										   select track).ToList();
			CompetitionData = new BindingList<Track>(competitionData.ToList());
		}

		/// <summary>
		/// Used to display the image of the car on the competition & race statistics windows.
		/// Returns the url of the car image based on the team colors.
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
