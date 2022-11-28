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
		private static int XPosition;
		private static int YPosition;
		public static int SectionSize { get; set; }
		public static int ParticipantSize { get; set; }
		public static int TrackWidth { get; set; }
		public static int TrackHeight { get; set; }

		private static Graphics Graphics;

		private static Direction _direction;

		private static Race _race;

		#region graphics

		//Sections
		private const string _urlDefaultSection = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Sections\\";

		private const string _StartHorizontal = _urlDefaultSection + "StartHorizontal.png";
		private const string _StartVertical = _urlDefaultSection + "StartVertical.png";
		private const string _FinishHorizontal = _urlDefaultSection + "FinishHorizontal.png";
		private const string _FinishVertical = _urlDefaultSection + "FinishVertical.png";
		private const string _StraightHorizontal = _urlDefaultSection + "StraightHorizontal.png";
		private const string _StraightVertical = _urlDefaultSection + "StraightVertical.png";
		private const string _CornerNE = _urlDefaultSection + "CornerNE.png";
		private const string _CornerNW = _urlDefaultSection + "CornerNW.png";
		private const string _CornerSE = _urlDefaultSection + "CornerSE.png";
		private const string _CornerSW = _urlDefaultSection + "CornerSW.png";

		//Participants

		//North
		private const string _urlDefaultDriverNorth = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\North\\";

		private const string _BlueDriverN = _urlDefaultDriverNorth + "BlueNorth.png";
		private const string _GreenDriverN = _urlDefaultDriverNorth + "GreenNorth.png";
		private const string _GreyDriverN = _urlDefaultDriverNorth + "GreyNorth.png";
		private const string _RedDriverN = _urlDefaultDriverNorth + "RedNorth.png";
		private const string _YellowDriverN = _urlDefaultDriverNorth + "YellowNorth.png";
		private const string _FireDriverN = _urlDefaultDriverNorth + "FireNorth.png";

		//East
		private const string _urlDefaultDriverEast = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\East\\";

		private const string _BlueDriverE = _urlDefaultDriverEast + "BlueEast.png";
		private const string _GreenDriverE = _urlDefaultDriverEast + "GreenEast.png";
		private const string _GreyDriverE = _urlDefaultDriverEast + "GreyEast.png";
		private const string _RedDriverE = _urlDefaultDriverEast + "RedEast.png";
		private const string _YellowDriverE = _urlDefaultDriverEast + "YellowEast.png";
		private const string _FireDriverE = _urlDefaultDriverEast + "FireEast.png";

		//South
		private const string _urlDefaultDriverSouth = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\South\\";

		private const string _BlueDriverS = _urlDefaultDriverSouth + "BlueSouth.png";
		private const string _GreenDriverS = _urlDefaultDriverSouth + "GreenSouth.png";
		private const string _GreyDriverS = _urlDefaultDriverSouth + "GreySouth.png";
		private const string _RedDriverS = _urlDefaultDriverSouth + "RedSouth.png";
		private const string _YellowDriverS = _urlDefaultDriverSouth + "YellowSouth.png";
		private const string _FireDriverS = _urlDefaultDriverSouth + "FireSouth.png";

		//West
		private const string _urlDefaultDriverWest = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\West\\";

		private const string _BlueDriverW = _urlDefaultDriverWest + "BlueWest.png";
		private const string _GreenDriverW = _urlDefaultDriverWest + "GreenWest.png";
		private const string _GreyDriverW = _urlDefaultDriverWest + "GreyWest.png";
		private const string _RedDriverW = _urlDefaultDriverWest + "RedWest.png";
		private const string _YellowDriverW = _urlDefaultDriverWest + "YellowWest.png";
		private const string _FireDriverW = _urlDefaultDriverWest + "FireWest.png";

		//Broken
		private const string _Broken = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Broken.png";

		#endregion

		public enum Direction
		{
			Right = 1,
			Down = 2,
			Left = 3,
			Up = 4
		}

		public static void Initialize(Race race)
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
		/// Calls DeterminePosition to determine the position of a section.
		/// Calls MovePointerPosition() to move the pointer to the next position.
		/// </summary>
		/// <param name="track"></param>
		/// <returns></returns>
		public static BitmapSource DrawTrack(Track track)
		{
			//Start positions
			XPosition = 3;
			YPosition = 2;

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
								Graphics.DrawImage(ImageManager.CloneImage(_StartHorizontal), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Up or Direction.Down:
								Graphics.DrawImage(ImageManager.CloneImage(_StartVertical), SectionXPosition(), SectionYPosition());
								break;
						}
						break;
					case SectionType.Finish:
						switch (_direction)
						{
							case Direction.Right or Direction.Left:
								Graphics.DrawImage(ImageManager.CloneImage(_FinishHorizontal), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Up or Direction.Down:
								Graphics.DrawImage(ImageManager.CloneImage(_FinishVertical), SectionXPosition(), SectionYPosition());
								break;
						}
						break;
					case SectionType.Straight:
						switch (_direction)
						{
							case Direction.Right or Direction.Left:
								Graphics.DrawImage(ImageManager.CloneImage(_StraightHorizontal), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Up or Direction.Down:
								Graphics.DrawImage(ImageManager.CloneImage(_StraightVertical), SectionXPosition(), SectionYPosition());
								break;
						}
						break;
					case SectionType.RightCorner:
						switch (_direction)
						{
							case Direction.Right:
								Graphics.DrawImage(ImageManager.CloneImage(_CornerNE), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Down:
								Graphics.DrawImage(ImageManager.CloneImage(_CornerSE), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Left:
								Graphics.DrawImage(ImageManager.CloneImage(_CornerSW), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Up:
								Graphics.DrawImage(ImageManager.CloneImage(_CornerNW), SectionXPosition(), SectionYPosition());
								break;
						}
						break;
					case SectionType.LeftCorner:

						switch (_direction)
						{
							case Direction.Right:
								Graphics.DrawImage(ImageManager.CloneImage(_CornerSE), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Down:
								Graphics.DrawImage(ImageManager.CloneImage(_CornerSW), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Left:
								Graphics.DrawImage(ImageManager.CloneImage(_CornerNW), SectionXPosition(), SectionYPosition());
								break;
							case Direction.Up:
								Graphics.DrawImage(ImageManager.CloneImage(_CornerNE), SectionXPosition(), SectionYPosition());
								break;
						}
						break;
				}
				DrawParticipants(_direction, Graphics, section);
				DeterminePosition(section.SectionTypes, _direction);
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
			CheckParticipantSpeed(participant);

			Bitmap participantBitmap = ImageManager.GetImage(GetColorFileName(participant.TeamColors, currentDirection));
			g.DrawImage(participantBitmap, x, y, ParticipantSize, ParticipantSize);
		}
		#endregion

		/// <summary>
		/// Called by DrawSingleParticipant.
		/// Used to check if a participant is eligible to get the fire image.
		/// </summary>
		/// <param name="participant"></param>
		private static void CheckParticipantSpeed(IParticipant participant)
		{
			if (participant.CalculateSpeed() > 55)
			{
				participant.IsFireball = true;
			}
			else
			{
				participant.IsFireball = false;
			}
		}

		/// <summary>
		/// Draws the "broken" image.
		/// </summary>
		/// <param name="g"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private static void DrawBroken(Graphics g, int x, int y) { g.DrawImage(ImageManager.GetImage(_Broken), x, y, ParticipantSize, ParticipantSize); }

		private static string GetColorFileName(TeamColors color, Direction currentDirection)
		{
			return currentDirection switch
			{
				//East
				Direction.Right => color switch
				{
					TeamColors.Red => _RedDriverE,
					TeamColors.Green => _GreenDriverE,
					TeamColors.Yellow => _YellowDriverE,
					TeamColors.Blue => _BlueDriverE,
					TeamColors.Grey => _GreyDriverE,
					TeamColors.Fire => _FireDriverE,
					_ => throw new InvalidDirectionException(),
				},
				//South
				Direction.Down => color switch
				{
					TeamColors.Red => _RedDriverS,
					TeamColors.Green => _GreenDriverS,
					TeamColors.Yellow => _YellowDriverS,
					TeamColors.Blue => _BlueDriverS,
					TeamColors.Grey => _GreyDriverS,
					TeamColors.Fire => _FireDriverS,
					_ => throw new InvalidDirectionException(),
				},
				//West
				Direction.Left => color switch
				{
					TeamColors.Red => _RedDriverW,
					TeamColors.Green => _GreenDriverW,
					TeamColors.Yellow => _YellowDriverW,
					TeamColors.Blue => _BlueDriverW,
					TeamColors.Grey => _GreyDriverW,
					TeamColors.Fire => _FireDriverW,
					_ => throw new InvalidDirectionException(),
				},
				//North
				Direction.Up => color switch
				{
					TeamColors.Red => _RedDriverN,
					TeamColors.Green => _GreenDriverN,
					TeamColors.Yellow => _YellowDriverN,
					TeamColors.Blue => _BlueDriverN,
					TeamColors.Grey => _GreyDriverN,
					TeamColors.Fire => _FireDriverN,
					_ => throw new InvalidDirectionException(),
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
					XPosition++;
					break;
				case Direction.Down:
					YPosition++;
					break;
				case Direction.Left:
					XPosition--;
					break;
				case Direction.Up:
					YPosition--;
					break;
			}
		}

		/// <summary>
		/// Determines the position of the next section that has to be drawn.
		/// </summary>
		/// <param name="sectionType"></param>
		/// <param name="direction"></param>
		private static void DeterminePosition(SectionType sectionType, Direction direction)
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
				DeterminePosition(section.SectionTypes, _direction);

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
			return XPosition * SectionSize;
		}

		/// <summary>
		/// Calculate the position of the section on the Y-axis.
		/// </summary>
		/// <returns></returns>
		private static int SectionYPosition()
		{
			return YPosition * SectionSize;
		}
	}
}

