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
		public static int XPosition;
		public static int YPosition;
		public static int ImageSize { get; set; }
		public static int TrackWidth { get; set; }
		public static int TrackHeight { get; set; }

		//declare direction
		public static Direction direction;

		private static Race _race;
		public static Graphics Graphics { get; set; }

		#region graphics

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

			XPosition = 0;
			YPosition = 0;

			direction = Direction.Right;

			ImageSize = 80;

			CalculateTrackSize();

			TrackWidth += ImageSize;
			TrackHeight += ImageSize;
		}

		public static BitmapSource DrawTrack(Track track)
		{
			Bitmap bitmap = ImageManager.EmptyTrack(TrackWidth, TrackHeight);
			Graphics graphics = Graphics.FromImage(bitmap);

			foreach (Section section in track.Sections)
			{
				switch (section.SectionTypes)
				{
					case SectionType.StartGrid:
						if (direction is Direction.Right or Direction.Left)
						{
							graphics.DrawImage(ImageManager.CloneImage(_StartHorizontal), CalculateX(), CalculateY());
						}
						else if (direction is Direction.Up or Direction.Down)
						{
							graphics.DrawImage(ImageManager.CloneImage(_StartVertical), CalculateX(), CalculateY());
						}
						break;
					case SectionType.Finish:
						if (direction is Direction.Right or Direction.Left)
						{
							graphics.DrawImage(ImageManager.CloneImage(_FinishHorizontal), CalculateX(), CalculateY());
						}
						else if (direction is Direction.Up or Direction.Down)
						{
							graphics.DrawImage(ImageManager.CloneImage(_FinishVertical), CalculateX(), CalculateY());
						}
						break;
					case SectionType.Straight:
						if (direction is Direction.Right or Direction.Left)
						{
							graphics.DrawImage(ImageManager.CloneImage(_StraightHorizontal), CalculateX(), CalculateY());
						}
						else if (direction is Direction.Up or Direction.Down)
						{
							graphics.DrawImage(ImageManager.CloneImage(_StraightVertical), CalculateX(), CalculateY());
						}
						break;
					case SectionType.RightCorner:
						if (direction == Direction.Right)
						{
							graphics.DrawImage(ImageManager.CloneImage(_CornerNE), CalculateX(), CalculateY());
						}
						else if (direction == Direction.Down)
						{
							graphics.DrawImage(ImageManager.CloneImage(_CornerSE), CalculateX(), CalculateY());
						}
						else if (direction == Direction.Left)
						{
							graphics.DrawImage(ImageManager.CloneImage(_CornerSW), CalculateX(), CalculateY());
						}
						else if (direction == Direction.Up)
						{
							graphics.DrawImage(ImageManager.CloneImage(_CornerNW), CalculateX(), CalculateY());
						}

						break;
					case SectionType.LeftCorner:
						if (direction == Direction.Left)
						{
							graphics.DrawImage(ImageManager.CloneImage(_CornerNE), CalculateX(), CalculateY());
						}
						else if (direction == Direction.Up)
						{
							graphics.DrawImage(ImageManager.CloneImage(_CornerSE), CalculateX(), CalculateY());
						}
						else if (direction == Direction.Right)
						{
							graphics.DrawImage(ImageManager.CloneImage(_CornerSW), CalculateX(), CalculateY());
						}
						else if (direction == Direction.Down)
						{
							graphics.DrawImage(ImageManager.CloneImage(_CornerNW), CalculateX(), CalculateY());
						}

						break;
				}

				DetermineDirection(section.SectionTypes, direction);
				//MoveImagePointer();
			}
			return ImageManager.CreateBitmapSourceFromGdiBitmap(bitmap);
		}

		private static void MoveImagePointer()
		{
			if (direction == Direction.Right)
			{
				XPosition += 80;
			}
			else if (direction == Direction.Left)
			{
				XPosition -= 80;
			}
			else if (direction == Direction.Up)
			{
				YPosition -= 80;
			}
			else if (direction == Direction.Down)
			{
				YPosition += 80;
			}
		}

		private static void DetermineDirection(SectionType sectionType, Direction direction)
		{
			switch (sectionType)
			{
				case SectionType.Finish:
					direction = VisualizationWPF.direction;

					break;
				case SectionType.StartGrid:
					direction = VisualizationWPF.direction;

					break;
				case SectionType.Straight:
					direction = VisualizationWPF.direction;

					break;
				case SectionType.RightCorner:
					if (direction == Direction.Right)
						direction = Direction.Down;
					else if (direction == Direction.Up)
						direction = Direction.Left;
					else if (direction == Direction.Left)
						direction = Direction.Up;
					else if (direction == Direction.Down)
						direction = Direction.Right;
					break;
				case SectionType.LeftCorner:
					if (direction == Direction.Right)
						direction = Direction.Up;
					else if (direction == Direction.Down)
						direction = Direction.Left;
					else if (direction == Direction.Left)
						direction = Direction.Down;
					else if (direction == Direction.Up)
						direction = Direction.Right;
					break;

			}
		}

		private static void CalculateTrackSize()
		{
			TrackWidth = 5;
			TrackHeight = 5;
			foreach (Section section in _race.Track.Sections)
			{
				DetermineDirection(section.SectionTypes, direction);

				if (direction == Direction.Right)
					TrackWidth += 5;
				if (direction == Direction.Down)
					TrackHeight += 5;
			}
		}

		private static int CalculateX()
		{
			return XPosition * ImageSize;
		}

		private static int CalculateY()
		{
			return YPosition * ImageSize;
		}
	}
}

