using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using Model;
using static System.Collections.Specialized.BitVector32;
using Section = Model.Section;

namespace RaceSimulator_Project
{
	public static class Visualization
	{
		public static int X;
		public static int Y;
		public static int PositieAuto;

		public static void Initialize()
		{
			X = 0;
			Y = 0;
			PositieAuto = 1;

			SetPositie(35, 30);
		}

		#region graphics

		private static string[] _finishHorizontal =
		{
			"----",
			"  # ",
			"  # ",
			"----"
		};

		private static string[] _finishVertical =
		{
			"|--|",
			"| #|",
			"| #|",
			"|  |"
		};

		private static string[] _straightStartHorizontal =
		{
			"----",
			"  s ",
			"    ",
			"----"
		};

		private static string[] _straightStartVertical =
		{
			"|  |",
			"|s | ",
			"|  |",
			"|  |"
		};

		private static string[] _straightHorizontal =
		{
			"----",
			"    ",
			"    ",
			"----"
		};

		private static string[] _straightVertical =
		{
			"|  |",
			"|  |",
			"|  |",
			"|  |"
		};

		private static string[] _cornerNE =
		{
			"----",
			"   |",
			"   |",
			"   |"
		};

		private static string[] _cornerSE =
		{
			"   |",
			"   |",
			"   |",
			"----"
		};

		private static string[] _cornerSW =
		{
			"|   ",
			"|   ",
			"|   ",
			"----"
		};

		private static string[] _cornerNW =
		{
			"----",
			"|   ",
			"|   ",
			"|   "
		};


		#endregion //Sections string[]

		public static void PrintToConsole(string[] array)
		{
			foreach (string s in array)
			{
				SetPositie(0, 1);
				Console.WriteLine(s);
			}
		}

		public static void SetPositie(int xVerandering, int yVerandering)
		{
			X += xVerandering;
			Y += yVerandering;
			Console.SetCursorPosition(X, Y);
		}

		public static void DrawTrack(Track track)
		{

			foreach (Section section in track.Sections)
			{
				switch (section.SectionTypes)
				{
					case SectionType.StartGrid:
						DetermineDirection(SectionType.StartGrid, track);
						break;
					case SectionType.Finish:
						DetermineDirection(SectionType.Finish, track);
						break;
					case SectionType.Straight:
						DetermineDirection(SectionType.Straight, track);
						break;
					case SectionType.LeftCorner:
						DetermineDirection(SectionType.LeftCorner, track);
						break;
					case SectionType.RightCorner:
						DetermineDirection(SectionType.RightCorner, track);
						break;
				}
			}
		}

		public static void DetermineDirection(SectionType sectionType, Track track)
		{

			switch (sectionType)
			{
				case SectionType.Finish:

					if (PositieAuto == 1)
					{
						SetPositie(4, -4);
						PrintToConsole(_finishHorizontal);
					}
					else if (PositieAuto == 3)
					{
						SetPositie(0, -8);
						PrintToConsole(_finishHorizontal);
					}

					else if (PositieAuto == 2)
					{
						SetPositie(0, 0);
						PrintToConsole(_finishVertical);
					}

					else if (PositieAuto == 4)
					{
						SetPositie(0, -8);
						PrintToConsole(_finishVertical);
					}

					break;

				case SectionType.StartGrid:


					if (PositieAuto == 1)
					{
						SetPositie(4, 1);
						PrintToConsole(_straightStartHorizontal);
					}
					else if (PositieAuto == 3)
					{
						SetPositie(0, -8);
						PrintToConsole(_straightStartHorizontal);
					}
					else if (PositieAuto == 2)
					{
						SetPositie(0, 4);
						PrintToConsole(_straightStartVertical);
					}
					else if (PositieAuto == 4)
					{
						SetPositie(0, -8);
						PrintToConsole(_straightStartVertical);
					}

					break;

				case SectionType.Straight:

					if (PositieAuto == 1)
					{
						SetPositie(4, -4);
						PrintToConsole(_straightHorizontal);
					}
					else if (PositieAuto == 3)
					{
						SetPositie(-4, -4);
						PrintToConsole(_straightHorizontal);
					}
					else if (PositieAuto == 2)
					{
						SetPositie(0, 0);
						PrintToConsole(_straightVertical);
					}
					else if (PositieAuto == 4)
					{
						SetPositie(0, -8);
						PrintToConsole(_straightVertical);

					}


					break;

				case SectionType.RightCorner:
					if (PositieAuto == 1)
					{
						SetPositie(4, -4);
						PositieAuto = 2;
						PrintToConsole(_cornerNE);
					}
					else if (PositieAuto == 2)
					{
						SetPositie(0, 0);
						PositieAuto = 3;
						PrintToConsole(_cornerSE);
					}
					else if (PositieAuto == 3)
					{
						SetPositie(-4, -4);
						PositieAuto = 4;
						PrintToConsole(_cornerSW);
					}
					else if (PositieAuto == 4)
					{
						SetPositie(0, -8);
						PositieAuto = 1;
						PrintToConsole(_cornerNW);
					}

					break;

				case SectionType.LeftCorner:

					if (PositieAuto == 3)
					{
						SetPositie(-4, -4);
						PositieAuto = 2;
						PrintToConsole(_cornerNW);
					}
					else if (PositieAuto == 4)
					{
						SetPositie(0, -8);
						PositieAuto = 3;
						PrintToConsole(_cornerNE);
					}
					else if (PositieAuto == 1)
					{
						SetPositie(4, -4);
						PositieAuto = 4;
						PrintToConsole(_cornerSE); //nw
					}
					else if (PositieAuto == 2)
					{
						SetPositie(0, 0);
						PositieAuto = 1;
						PrintToConsole(_cornerSW);
					}

					break;
			}
		}
	}
}


