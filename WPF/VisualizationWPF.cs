using System.Drawing;
using Model;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using Controller;
using Microsoft.Win32;
using System;
using System.Windows.Threading;
using Track = Model.Track;

namespace WPF
{
	public static class VisualizationWPF
	{
		private static int _xPosition;
		private static int _yPosition;
		public static int SectionSize { get; set; }
		public static int ParticipantSize { get; set; }
		public static int TrackWidth { get; set; }
		public static int TrackHeight { get; set; }

		private static Graphics Graphics;

		private static Direction _direction;

		private static Race? _race;

		#region graphics

		//Sections
		private const string UrlDefaultSection = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Sections\\";

		private const string StartHorizontal = UrlDefaultSection + "StartHorizontal.png";
		private const string StartVertical = UrlDefaultSection + "StartVertical.png";
		private const string FinishHorizontal = UrlDefaultSection + "FinishHorizontal.png";
		private const string FinishVertical = UrlDefaultSection + "FinishVertical.png";
		private const string StraightHorizontal = UrlDefaultSection + "StraightHorizontal.png";
		private const string StraightVertical = UrlDefaultSection + "StraightVertical.png";
		private const string CornerNe = UrlDefaultSection + "CornerNE.png";
		private const string CornerNw = UrlDefaultSection + "CornerNW.png";
		private const string CornerSe = UrlDefaultSection + "CornerSE.png";
		private const string CornerSw = UrlDefaultSection + "CornerSW.png";

		//Participants

		//North
		private const string UrlDefaultDriverNorth = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\North\\";

		private const string BlueDriverN = UrlDefaultDriverNorth + "BlueNorth.png";
		private const string GreenDriverN = UrlDefaultDriverNorth + "GreenNorth.png";
		private const string GreyDriverN = UrlDefaultDriverNorth + "GreyNorth.png";
		private const string RedDriverN = UrlDefaultDriverNorth + "RedNorth.png";
		private const string YellowDriverN = UrlDefaultDriverNorth + "YellowNorth.png";
		private const string FireDriverN = UrlDefaultDriverNorth + "FireNorth.png";

		//East
		private const string UrlDefaultDriverEast = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\East\\";

		private const string BlueDriverE = UrlDefaultDriverEast + "BlueEast.png";
		private const string GreenDriverE = UrlDefaultDriverEast + "GreenEast.png";
		private const string GreyDriverE = UrlDefaultDriverEast + "GreyEast.png";
		private const string RedDriverE = UrlDefaultDriverEast + "RedEast.png";
		private const string YellowDriverE = UrlDefaultDriverEast + "YellowEast.png";
		private const string FireDriverE = UrlDefaultDriverEast + "FireEast.png";

		//South
		private const string UrlDefaultDriverSouth = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\South\\";

		private const string BlueDriverS = UrlDefaultDriverSouth + "BlueSouth.png";
		private const string GreenDriverS = UrlDefaultDriverSouth + "GreenSouth.png";
		private const string GreyDriverS = UrlDefaultDriverSouth + "GreySouth.png";
		private const string RedDriverS = UrlDefaultDriverSouth + "RedSouth.png";
		private const string YellowDriverS = UrlDefaultDriverSouth + "YellowSouth.png";
		private const string FireDriverS = UrlDefaultDriverSouth + "FireSouth.png";

		//West
		private const string UrlDefaultDriverWest = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\West\\";

		private const string BlueDriverW = UrlDefaultDriverWest + "BlueWest.png";
		private const string GreenDriverW = UrlDefaultDriverWest + "GreenWest.png";
		private const string GreyDriverW = UrlDefaultDriverWest + "GreyWest.png";
		private const string RedDriverW = UrlDefaultDriverWest + "RedWest.png";
		private const string YellowDriverW = UrlDefaultDriverWest + "YellowWest.png";
		private const string FireDriverW = UrlDefaultDriverWest + "FireWest.png";

		//Broken
		private const string Broken = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Broken.png";

		#endregion

		public enum Direction
		{
			Right = 1,
			Down = 2,
			Left = 3,
			Up = 4
		}

		public static void Initialize(Race? race)
		{
			_race = race;

			TrackHeight = TrackWidth = 5; //Set the track size

			_direction = Direction.Right;

			SectionSize = 80;
			ParticipantSize = 35;

			CalculateTrackSize();

			TrackWidth *= SectionSize;
			TrackHeight *= SectionSize;
		}

		#region Draw track
		/// <summary>
		/// Draws the track on the WPF window.
		/// Calls DrawParticipants() to draw the participants on the track.
		/// Calls DetermineDirection to determine the position of a section.
		/// Calls MovePointerPosition() to move the pointer to the next position.
		/// </summary>
		/// <param name="track"></param>
		/// <returns></returns>
		public static BitmapSource DrawTrack(Track track)
		{
			//Start positions
			_xPosition = 3;
			_yPosition = 2;

			Bitmap bitmap = ImageManager.CreateEmptyTrack(TrackWidth, TrackHeight);
			Graphics = Graphics.FromImage(bitmap);

			foreach (Section section in track.Sections)
			{
				switch (section.SectionTypes)
				{
					case SectionType.StartGrid:
						switch (_direction)
						{
							case Direction.Right or Direction.Left:
								Graphics.DrawImage(ImageManager.CloneImage(StartHorizontal), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Up or Direction.Down:
								Graphics.DrawImage(ImageManager.CloneImage(StartVertical), SectionXPosition(), SectionYPosition());
								break;
						}
						break;
					case SectionType.Finish:
						switch (_direction)
						{
							case Direction.Right or Direction.Left:
								Graphics.DrawImage(ImageManager.CloneImage(FinishHorizontal), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Up or Direction.Down:
								Graphics.DrawImage(ImageManager.CloneImage(FinishVertical), SectionXPosition(), SectionYPosition());
								break;
						}
						break;
					case SectionType.Straight:
						switch (_direction)
						{
							case Direction.Right or Direction.Left:
								Graphics.DrawImage(ImageManager.CloneImage(StraightHorizontal), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Up or Direction.Down:
								Graphics.DrawImage(ImageManager.CloneImage(StraightVertical), SectionXPosition(), SectionYPosition());
								break;
						}
						break;
					case SectionType.RightCorner:
						switch (_direction)
						{
							case Direction.Right:
								Graphics.DrawImage(ImageManager.CloneImage(CornerNe), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Down:
								Graphics.DrawImage(ImageManager.CloneImage(CornerSe), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Left:
								Graphics.DrawImage(ImageManager.CloneImage(CornerSw), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Up:
								Graphics.DrawImage(ImageManager.CloneImage(CornerNw), SectionXPosition(), SectionYPosition());
								break;
						}
						break;
					case SectionType.LeftCorner:

						switch (_direction)
						{
							case Direction.Right:
								Graphics.DrawImage(ImageManager.CloneImage(CornerSe), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Down:
								Graphics.DrawImage(ImageManager.CloneImage(CornerSw), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Left:
								Graphics.DrawImage(ImageManager.CloneImage(CornerNw), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Up:
								Graphics.DrawImage(ImageManager.CloneImage(CornerNe), SectionXPosition(), SectionYPosition());
								break;
						}
						break;
				}
				DrawParticipants(_direction, Graphics, section);
				DetermineDirection(section.SectionTypes, _direction);
				MovePointerPosition();
			}
			return ImageManager.CreateBitmapSourceFromGdiBitmap(bitmap);
		}
		#endregion

		#region Draw participant
		/// <summary>
		/// Draws participants on the track.
		/// Calls DrawSingleParticipant to actually draw the participant.
		/// If a participant breaks, a "broken" image is displayed on top of the participant.
		/// </summary>
		/// <param name="currentDirection"></param>
		/// <param name="g"></param>
		/// <param name="section"></param>
		public static void DrawParticipants(Direction currentDirection, Graphics g, Section section)
		{
			IParticipant rightParticipant = _race.GetSectionData(section).Right;
			IParticipant leftParticipant = _race.GetSectionData(section).Left;

			if (rightParticipant != null)
			{
				DrawSingleParticipant(rightParticipant, g, currentDirection, ParticipantXPosition(rightParticipant, section), ParticipantYPosition(rightParticipant, section));
				if (rightParticipant.Equipment.IsBroken)
					DrawBroken(g, ParticipantXPosition(rightParticipant, section), ParticipantYPosition(rightParticipant, section)); //Broken image sits on top of participant
			}

			if (leftParticipant != null)
			{
				DrawSingleParticipant(leftParticipant, g, currentDirection, ParticipantXPosition(leftParticipant, section), ParticipantYPosition(leftParticipant, section));
				if (leftParticipant.Equipment.IsBroken)
					DrawBroken(g, ParticipantXPosition(leftParticipant, section), ParticipantYPosition(leftParticipant, section)); //Broken image sits on top of participant
			}
		}

		/// <summary>
		/// Gets called by DrawParticipants to draw a single participant on their position.
		/// </summary>
		/// <param name="participant"></param>
		/// <param name="g"></param>
		/// <param name="currentDirection"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private static void DrawSingleParticipant(IParticipant participant, Graphics g, Direction currentDirection, int x, int y)
		{
			Race.CheckParticipantSpeed(participant);

			Bitmap participantBitmap = ImageManager.GetImage(GetColorUrl(participant.TeamColors, currentDirection));
			g.DrawImage(participantBitmap, x, y, ParticipantSize, ParticipantSize);
		}
		#endregion

		/// <summary>
		/// Draws the "broken" image.
		/// </summary>
		/// <param name="g"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private static void DrawBroken(Graphics g, int x, int y) { g.DrawImage(ImageManager.GetImage(Broken), x, y, ParticipantSize, ParticipantSize); }

		private static string GetColorUrl(TeamColors color, Direction currentDirection)
		{
			return currentDirection switch
			{
				//East
				Direction.Right => color switch
				{
					TeamColors.Red => RedDriverE,
					TeamColors.Green => GreenDriverE,
					TeamColors.Yellow => YellowDriverE,
					TeamColors.Blue => BlueDriverE,
					TeamColors.Grey => GreyDriverE,
					TeamColors.Fire => FireDriverE,
					_ => throw new InvalidTeamColorException(),
				},
				//South
				Direction.Down => color switch
				{
					TeamColors.Red => RedDriverS,
					TeamColors.Green => GreenDriverS,
					TeamColors.Yellow => YellowDriverS,
					TeamColors.Blue => BlueDriverS,
					TeamColors.Grey => GreyDriverS,
					TeamColors.Fire => FireDriverS,
					_ => throw new InvalidTeamColorException(),
				},
				//West
				Direction.Left => color switch
				{
					TeamColors.Red => RedDriverW,
					TeamColors.Green => GreenDriverW,
					TeamColors.Yellow => YellowDriverW,
					TeamColors.Blue => BlueDriverW,
					TeamColors.Grey => GreyDriverW,
					TeamColors.Fire => FireDriverW,
					_ => throw new InvalidTeamColorException(),
				},
				//North
				Direction.Up => color switch
				{
					TeamColors.Red => RedDriverN,
					TeamColors.Green => GreenDriverN,
					TeamColors.Yellow => YellowDriverN,
					TeamColors.Blue => BlueDriverN,
					TeamColors.Grey => GreyDriverN,
					TeamColors.Fire => FireDriverN,
					_ => throw new InvalidTeamColorException(),
				},
				_ => throw new InvalidDirectionException()
			};
		}

		#region Positioning
		/// <summary>
		/// Determines the position of the pointer.
		/// </summary>
		private static void MovePointerPosition()
		{
			switch (_direction)
			{
				case Direction.Right:
					_xPosition++;
					break;
				case Direction.Down:
					_yPosition++;
					break;
				case Direction.Left:
					_xPosition--;
					break;
				case Direction.Up:
					_yPosition--;
					break;
			}
		}

		/// <summary>
		/// Determines the direction of the next section that has to be drawn.
		/// </summary>
		/// <param name="sectionType"></param>
		/// <param name="direction"></param>
		private static void DetermineDirection(SectionType sectionType, Direction direction)
		{
			switch (sectionType)
			{
				case SectionType.Finish:
					break;
				case SectionType.StartGrid:
					break;
				case SectionType.Straight:
					break;
				case SectionType.RightCorner:
					if (direction == Direction.Right)
						_direction = Direction.Down;
					else if (direction == Direction.Up)
						_direction = Direction.Right;
					else if (direction == Direction.Left)
						_direction = Direction.Up;
					else if (direction == Direction.Down)
						_direction = Direction.Left;
					break;
				case SectionType.LeftCorner:
					if (direction == Direction.Right)
						_direction = Direction.Up;
					else if (direction == Direction.Down)
						_direction = Direction.Right;
					else if (direction == Direction.Left)
						_direction = Direction.Down;
					else if (direction == Direction.Up)
						_direction = Direction.Left;
					break;
			}
		}
		#endregion

		/// <summary>
		/// Calculate the size of the track.
		/// </summary>
		private static void CalculateTrackSize()
		{
			foreach (Section section in _race.Track.Sections)
			{
				DetermineDirection(section.SectionTypes, _direction);

				switch (_direction)
				{
					case Direction.Right:
						TrackWidth++;
						break;
					case Direction.Down:
						TrackHeight++;
						break;
				}
			}
		}

		/// <summary>
		/// Calculate the position of the participants on the X-axis.
		/// </summary>
		/// <param name="participant"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		private static int ParticipantXPosition(IParticipant participant, Section section)
		{
			IParticipant rightParticipant = _race.GetSectionData(section).Right;
			IParticipant leftParticipant = _race.GetSectionData(section).Left;

			if (participant == rightParticipant)
				return SectionXPosition() + (SectionSize / 2) - (ParticipantSize / 2) - (ParticipantSize / 2);
			if (participant == leftParticipant)
				return SectionXPosition() + (SectionSize / 2) - (ParticipantSize / 2) + (ParticipantSize / 2);
			return 0;
		}

		/// <summary>
		/// Calculate the position of the participants on the Y-axis.
		/// </summary>
		/// <param name="participant"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		private static int ParticipantYPosition(IParticipant participant, Section section)
		{
			IParticipant rightParticipant = _race.GetSectionData(section).Right;
			IParticipant leftParticipant = _race.GetSectionData(section).Left;

			if (participant == rightParticipant)
				return SectionYPosition() + (SectionSize / 2) - (ParticipantSize / 2) - (ParticipantSize / 2);
			if (participant == leftParticipant)
				return SectionYPosition() + (SectionSize / 2) - (ParticipantSize / 2) + (ParticipantSize / 2);
			return 0;
		}

		/// <summary>
		/// Calculate the position of the section on the X-axis.
		/// </summary>
		/// <returns></returns>
		private static int SectionXPosition()
		{
			return _xPosition * SectionSize;
		}

		/// <summary>
		/// Calculate the position of the section on the Y-axis.
		/// </summary>
		/// <returns></returns>
		private static int SectionYPosition()
		{
			return _yPosition * SectionSize;
		}
	}
}

