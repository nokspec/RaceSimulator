using Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Controller
{
	public class DataContext : INotifyPropertyChanged
	{
		private static readonly string UrlDefaultCar = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\West\\";

		public event PropertyChangedEventHandler? PropertyChanged;
		
		//Track name
		private string _trackName { get; set; }
		public string TrackName
		{
			get => _trackName;
			set { _trackName = value; OnPropertyChanged(); }
		}
		//Competition
		private List<ParticipantCompetitionData> _participantsCompetition => CreateParticipantCompetitionList(Data.Competition.Participants);
		public List<ParticipantCompetitionData> ParticipantsCompetition => _participantsCompetition; //Used in XAML

		//BindingList because the track names have to be removed after a race.
		private BindingList<Track> _competitionData { get; set; }
		public BindingList<Track> CompetitionData
		{
			get => _competitionData;
			set { _competitionData = value; OnPropertyChanged(); }
		} //Used in XAML

		//Race
		private List<ParticipantRaceData> _participantsRace => CreateParticipantRaceList(Data.Competition.Participants);
		public List<ParticipantRaceData> ParticipantsRace => _participantsRace; //Used in XAML

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
		public static string DisplayCarImage(TeamColors teamColor)
		{
			switch (teamColor)
			{
				case TeamColors.Blue:
					return UrlDefaultCar + "BlueWest.png";
				case TeamColors.Green:
					return UrlDefaultCar + "GreenWest.png";
				case TeamColors.Red:
					return UrlDefaultCar + "RedWest.png";
				case TeamColors.Yellow:
					return UrlDefaultCar + "YellowWest.png";
				case TeamColors.Grey:
					return UrlDefaultCar + "GreyWest.png";
				default:
					return UrlDefaultCar + "FireWest.png";
			}
		}
	}
}
