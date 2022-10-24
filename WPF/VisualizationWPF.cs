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
		private static int SectionSize { get; set; }
		public static int TrackWidth { get; set; }
		public static int TrackHeight { get; set; }

		public static Graphics Graphics;

		//declare direction
		public static Direction _direction;

		private static Race _race;

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

			//Hiermee bepaal je de grootte van de track
			TrackHeight = 6;
			TrackWidth = 6;

			_direction = Direction.Right;

			ImageSize = 80;

			CalculateTrackSize();

			TrackWidth *= ImageSize;
			TrackHeight *= ImageSize;
		}

		public static BitmapSource DrawTrack(Track track)
		{
			XPosition = 4;
			YPosition = 4;

			Bitmap bitmap = ImageManager.EmptyTrack(TrackWidth, TrackHeight);
			Graphics = Graphics.FromImage(bitmap);

			foreach (Section section in track.Sections)
			{
				switch (section.SectionTypes)
				{
					case SectionType.StartGrid:
						switch (_direction)
						{
							case Direction.Right or Direction.Left:
								Graphics.DrawImage(ImageManager.CloneImage(_StartHorizontal), CalculateX(), CalculateY());
								break;
							case Direction.Up or Direction.Down:
								Graphics.DrawImage(ImageManager.CloneImage(_StartVertical), CalculateX(), CalculateY());
								break;
						}
						break;
					case SectionType.Finish:
						switch (_direction)
						{
							case Direction.Right or Direction.Left:
								Graphics.DrawImage(ImageManager.CloneImage(_FinishHorizontal), CalculateX(), CalculateY());
								break;
							case Direction.Up or Direction.Down:
								Graphics.DrawImage(ImageManager.CloneImage(_FinishVertical), CalculateX(), CalculateY());
								break;
						}
						break;
					case SectionType.Straight:
						switch (_direction)
						{
							case Direction.Right or Direction.Left:
								Graphics.DrawImage(ImageManager.CloneImage(_StraightHorizontal), CalculateX(), CalculateY());
								break;
							case Direction.Up or Direction.Down:
								Graphics.DrawImage(ImageManager.CloneImage(_StraightVertical), CalculateX(), CalculateY());
								break;
						}
						break;
					case SectionType.RightCorner:
						switch (_direction)
						{
							case Direction.Right:
								Graphics.DrawImage(ImageManager.CloneImage(_CornerNE), CalculateX(), CalculateY());
								break;
							case Direction.Down:
								Graphics.DrawImage(ImageManager.CloneImage(_CornerSE), CalculateX(), CalculateY());
								break;
							case Direction.Left:
								Graphics.DrawImage(ImageManager.CloneImage(_CornerSW), CalculateX(), CalculateY());
								break;
							case Direction.Up:
								Graphics.DrawImage(ImageManager.CloneImage(_CornerNW), CalculateX(), CalculateY());
								break;
						}
						break;
					case SectionType.LeftCorner:

						switch (_direction)
						{
							case Direction.Right:
								Graphics.DrawImage(ImageManager.CloneImage(_CornerSE), CalculateX(), CalculateY());
								break;
							case Direction.Down:
								Graphics.DrawImage(ImageManager.CloneImage(_CornerSW), CalculateX(), CalculateY());
								break;
							case Direction.Left:
								Graphics.DrawImage(ImageManager.CloneImage(_CornerNW), CalculateX(), CalculateY());
								break;
							case Direction.Up:
								Graphics.DrawImage(ImageManager.CloneImage(_CornerNE), CalculateX(), CalculateY());
								break;
						}
						break;
				}
				DetermineDirection(section.SectionTypes, _direction);
				MoveImagePointer();
			}
			return ImageManager.CreateBitmapSourceFromGdiBitmap(bitmap);
		}

		private static void MoveImagePointer()
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

