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

		//declare direction
		private static Direction _direction;

		private static Race _race;

		#region graphics

		//sections
		private const string _StartHorizontal =
			"C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Sections\\StartHorizontal.png";

		private const string _StartVertical =
			"C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Sections\\StartVertical.png";

		private const string _FinishHorizontal =
			"C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Sections\\FinishHorizontal.png";

		private const string _FinishVertical =
			"C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Sections\\FinishVertical.png";

		private const string _StraightHorizontal =
			"C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Sections\\StraightHorizontal.png";

		private const string _StraightVertical =
			"C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Sections\\StraightVertical.png";

		private const string _CornerNE =
			"C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Sections\\CornerNE.png";

		private const string _CornerNW =
			"C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Sections\\CornerNW.png";

		private const string _CornerSE =
			"C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Sections\\CornerSE.png";

		private const string _CornerSW =
			"C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Sections\\CornerSW.png";

		//participants

		//north
		private const string _BluePlayerN = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\North\\BlueNorth.png";
		private const string _GreenPlayerN = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\North\\GreenNorth.png";
		private const string _GreyPlayerN = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\North\\GreyNorth.png";
		private const string _RedPlayerN = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\North\\RedNorth.png";
		private const string _YellowPlayerN = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\North\\YellowNorth.png";

		//east
		private const string _BluePlayerE = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\East\\BlueEast.png";
		private const string _GreenPlayerE = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\East\\GreenEast.png";
		private const string _GreyPlayerE = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\East\\GreyEast.png";
		private const string _RedPlayerE = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\East\\RedEast.png";
		private const string _YellowPlayerE = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\East\\YellowEast.png";

		//south
		private const string _BluePlayerS = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\South\\BlueSouth.png";
		private const string _GreenPlayerS = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\South\\GreenSouth.png";
		private const string _GreyPlayerS = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\South\\GreySouth.png";
		private const string _RedPlayerS = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\South\\RedSouth.png";
		private const string _YellowPlayerS = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\South\\YellowSouth.png";

		//west
		private const string _BluePlayerW = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\West\\BlueWest.png";
		private const string _GreenPlayerW = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\West\\GreenWest.png";
		private const string _GreyPlayerW = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\West\\GreyWest.png";
		private const string _RedPlayerW = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\West\\ReDWest.png";
		private const string _YellowPlayerW = "C:\\Users\\naoki\\OneDrive\\HBO-ICT\\Jaar 2\\C#\\RaceSimulator\\RaceSimulator_Solution\\WPF\\Images\\Participants\\West\\YellowWest.png";

		//broken
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

			//Hiermee bepaal je de grootte van de track (dit vind ik wel even big brain van mezelf met de single statement assign)
			TrackHeight = TrackWidth = 5;

			_direction = Direction.Right;

			SectionSize = 80; 
			ParticipantSize = 35; 
			
			CalculateTrackSize();

			TrackWidth *= SectionSize;
			TrackHeight *= SectionSize;
		}

		public static BitmapSource DrawTrack(Track track)
		{
			//start position
			XPosition = 4;
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
				DetermineDirection(section.SectionTypes, _direction);
				MovePointerPosition();
			}
			return ImageManager.CreateBitmapSourceFromGdiBitmap(bitmap);
		}

		public static void DrawParticipants(Direction currentDirection, Graphics g, Section section)
		{
			IParticipant rightParticipant = _race.GetSectionData(section).Right;
			IParticipant leftParticipant = _race.GetSectionData(section).Left;
			
			if (rightParticipant != null)
			{
				DrawSingleParticipant(rightParticipant, g, currentDirection, ParticipantXPosition(rightParticipant, section), ParticipantYPosition(rightParticipant, section)); // draw participant
				if (rightParticipant.Equipment.IsBroken)
					DrawBrokenImageOnCoord(g, ParticipantXPosition(rightParticipant, section), ParticipantYPosition(rightParticipant, section)); // draw broken image on top of participant if participant is broken.
			}
			
			if (leftParticipant != null)
			{
				DrawSingleParticipant(leftParticipant, g, currentDirection, ParticipantXPosition(leftParticipant, section), ParticipantYPosition(leftParticipant, section)); // draw participant
				if (leftParticipant.Equipment.IsBroken)
					DrawBrokenImageOnCoord(g, ParticipantXPosition(leftParticipant, section), ParticipantYPosition(leftParticipant, section)); // draw broken image on top of participant if participant is broken.
			}
		}

		private static void DrawSingleParticipant(IParticipant participant, Graphics g, Direction currentDirection, int x, int y)
		{
			Bitmap participantBitmap = ImageManager.GetImage(GetColorFileName(participant.TeamColors, currentDirection));
			g.DrawImage(participantBitmap, x, y, ParticipantSize, ParticipantSize);
		}
		private static void DrawBrokenImageOnCoord(Graphics g, int x, int y)
		{
			g.DrawImage(ImageManager.GetImage(_Broken), x, y, ParticipantSize, ParticipantSize);
		}

		//TODO: refactor
		private static string GetColorFileName(TeamColors color, Direction currentDirection)
		{
			return currentDirection switch
			{
				//east
				Direction.Right => color switch
				{
					TeamColors.Red => _RedPlayerE,
					TeamColors.Green => _GreenPlayerE,
					TeamColors.Yellow => _YellowPlayerE,
					TeamColors.Blue => _BluePlayerE,
					TeamColors.Grey => _GreyPlayerE,
				},
				//south
				Direction.Down => color switch
				{
					TeamColors.Red => _RedPlayerS,
					TeamColors.Green => _GreenPlayerS,
					TeamColors.Yellow => _YellowPlayerS,
					TeamColors.Blue => _BluePlayerS,
					TeamColors.Grey => _GreyPlayerS,
				},
				//west
				Direction.Left => color switch
				{
					TeamColors.Red => _RedPlayerW,
					TeamColors.Green => _GreenPlayerW,
					TeamColors.Yellow => _YellowPlayerW,
					TeamColors.Blue => _BluePlayerW,
					TeamColors.Grey => _GreyPlayerW,
				},
				//north
				Direction.Up => color switch
				{
					TeamColors.Red => _RedPlayerN,
					TeamColors.Green => _GreenPlayerN,
					TeamColors.Yellow => _YellowPlayerN,
					TeamColors.Blue => _BluePlayerN,
					TeamColors.Grey => _GreyPlayerN,
				}
			};
		}

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

		private static int SectionXPosition()
		{
			return XPosition * SectionSize;
		}

		private static int SectionYPosition()
		{
			return YPosition * SectionSize;
		}

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
	}
}

