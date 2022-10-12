using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;
using static System.Collections.Specialized.BitVector32;
using Section = Model.Section;

namespace RaceSimulator_Project
{
	public static class Visualization
	{
		public static int X;
		public static int Y;
		public static int Positie;
		public static int OudePositie; //Is nodig voor de bochten.
		public static Section CurrentSection;
		private static Race Race;

		public static void Initialize(Race race)
		{
			CurrentSection = null; //When initializing there's no _currentSection.
			X = 0;
			Y = 0;
			Positie = 1;
			SetPositie(35, 30);

			Race = race;
			Data.CurrentRace.DriversChanged += OnDriversChanged;
		}

		//Event
		public static void OnDriversChanged(object source, DriversChangedEventArgs e)
		{
			Console.Clear();
			DrawTrack(e.Track);
		}

		#region graphics

		private static string[] _finishHorizontal =
		{
			"----",
			"  2#",
			" 1 #",
			"----"
		};

		private static string[] _finishVertical =
		{
			"|##|",
			"| 2|",
			"|1 |",
			"|  |"
		};

		private static string[] _straightStartHorizontal = //s om aan te duiden dat t start is
		{
			"----",
			"  2",
			" 1  ",
			"----"
		};

		private static string[] _straightStartVertical = //s om aan te duiden dat t start is
		{
			"|  |",
			"| 1| ",
			"|2 |",
			"|  |"
		};

		private static string[] _straightHorizontal =
		{
			"----",
			"  2 ",
			" 1  ",
			"----"
		};

		private static string[] _straightVertical =
		{
			"|  |",
			"| 1|",
			"|2 |",
			"|  |"
		};

		private static string[] _cornerNE =
		{
			"----",
			"  2|",
			" 1 |",
			"   |"
		};

		private static string[] _cornerSE =
		{
			"   |",
			"  2|",
			" 1 |",
			"----"
		};

		private static string[] _cornerSW =
		{
			"|   ",
			"|  2",
			"| 1 ",
			"----"
		};

		private static string[] _cornerNW =
		{
			"----",
			"|  2",
			"| 1 ",
			"|   "
		};


		#endregion //Sections string[]

		public static void PrintToConsole(string[] array, SectionData sectionData)
		{
			foreach (string s in array)
			{
				string newS = s;
				if (sectionData.Left != null)
				{
					newS = ReplaceString(s, sectionData.Left);
				}
				else if (sectionData.Right != null)
				{
					newS = ReplaceString(s, sectionData.Right);
				}
				newS = ReplaceString(s, sectionData.Left, sectionData.Right);
				SetPositie(0, 1);
				Console.WriteLine(newS);
			}
		}
		private static string ReplaceString(string input, IParticipant leftParticipant, IParticipant rightParticipant)
		{
			//return input.Replace("1", leftParticipant.Name[0].ToString()).Replace("2", rightParticipant.Name[0].ToString());
			return input.Replace("1", (leftParticipant?.Name?.Substring(0, 1)) ?? " ").Replace("2", (rightParticipant?.Name?.Substring(0, 1)) ?? " ");
		}

		private static string ReplaceString(string stringMetNummer, IParticipant participant)
		{
			participant.CurrentSection = CurrentSection; //Geef CurrentSection mee aan Participant.CurrentSection
			if (Race.GetSectionData(participant.CurrentSection).Left == participant)
			{
				return stringMetNummer.Replace("1", participant.Name[0].ToString());
			}
			else if (Race.GetSectionData(participant.CurrentSection).Right == participant)
			{
				return stringMetNummer.Replace("2", participant.Name[0].ToString());
			}
			return null;
		}

		public static void SetPositie(int xVerandering, int yVerandering)
		{
			X += xVerandering;
			Y += yVerandering;
			Console.SetCursorPosition(X, Y);
		}

		#region drawtrack
		public static void DrawTrack(Track track)
		{
			foreach (Section section in track.Sections)
			{
				CurrentSection = section;
				switch (section.SectionTypes)
				{
					case SectionType.StartGrid:
						DetermineDirection(SectionType.StartGrid, track);
						if (Positie == 1 || Positie == 3)
							PrintToConsole(_straightStartHorizontal, Race.GetSectionData(section));
						else
						{
							PrintToConsole(_straightStartVertical, Race.GetSectionData(section));
						}
						break;

					case SectionType.Finish:
						DetermineDirection(SectionType.Finish, track);
						if (Positie == 1 || Positie == 3)
							PrintToConsole(_finishHorizontal, Race.GetSectionData(section));
						else
						{
							PrintToConsole(_finishVertical, Race.GetSectionData(section));
						}
						break;

					case SectionType.Straight:
						DetermineDirection(SectionType.Straight, track);
						if (Positie == 1 || Positie == 3)
							PrintToConsole(_straightHorizontal, Race.GetSectionData(section));
						else
						{
							PrintToConsole(_straightVertical, Race.GetSectionData(section));
						}
						break;

					case SectionType.RightCorner:
						DetermineDirection(SectionType.RightCorner, track);
						if (OudePositie == 1)
							PrintToConsole(_cornerNE, Race.GetSectionData(section));
						else if (OudePositie == 2)
							PrintToConsole(_cornerSE, Race.GetSectionData(section));
						else if (OudePositie == 3)
							PrintToConsole(_cornerSW, Race.GetSectionData(section));
						else if (OudePositie == 4)
							PrintToConsole(_cornerNW, Race.GetSectionData(section));
						break;

					case SectionType.LeftCorner:
						DetermineDirection(SectionType.LeftCorner, track);
						if (OudePositie == 3)
							PrintToConsole(_cornerNW, Race.GetSectionData(section));
						else if (OudePositie == 4)
							PrintToConsole(_cornerNE, Race.GetSectionData(section));
						else if (OudePositie == 1)
							PrintToConsole(_cornerSE, Race.GetSectionData(section));
						else if (OudePositie == 2)
							PrintToConsole(_cornerSW, Race.GetSectionData(section));
						break;
				}
			}
		}

		public static void DetermineDirection(SectionType sectionType, Track track)
		{
			switch (sectionType)
			{
				case SectionType.Finish:
					if (Positie == 1)
					{
						SetPositie(4, -4);
					}
					else if (Positie == 3)
					{
						SetPositie(0, -8);
					}
					else if (Positie == 2)
					{
						SetPositie(0, 0);
					}
					else if (Positie == 4)
					{
						SetPositie(0, -8);
					}
					break;

				case SectionType.StartGrid:
					if (Positie == 1)
					{
						SetPositie(4, -4);
					}
					else if (Positie == 3)
					{
						SetPositie(0, -8);
					}
					else if (Positie == 2)
					{
						SetPositie(0, 0);
					}
					else if (Positie == 4)
					{
						SetPositie(0, -8);
					}
					break;

				case SectionType.Straight:
					if (Positie == 1)
					{
						SetPositie(4, -4);
					}
					else if (Positie == 3)
					{
						SetPositie(-4, -4);
					}
					else if (Positie == 2)
					{
						SetPositie(0, 0);
					}
					else if (Positie == 4)
					{
						SetPositie(0, -8);
					}
					break;

				case SectionType.RightCorner:
					if (Positie == 1)
					{
						SetPositie(4, -4);
						Positie = 2;
						OudePositie = 1;
					}
					else if (Positie == 2)
					{
						SetPositie(0, 0);
						Positie = 3;
						OudePositie = 2;
					}
					else if (Positie == 3)
					{
						SetPositie(-4, -4);
						Positie = 4;
						OudePositie = 3;
					}
					else if (Positie == 4)
					{
						SetPositie(0, -8);
						Positie = 1;
						OudePositie = 4;
					}
					break;

				case SectionType.LeftCorner:
					if (Positie == 3)
					{
						SetPositie(-4, -4);
						Positie = 2;
						OudePositie = 3;
					}
					else if (Positie == 4)
					{
						SetPositie(0, -8);
						Positie = 3;
						OudePositie = 4;
					}
					else if (Positie == 1)
					{
						SetPositie(4, -4);
						Positie = 4;
						OudePositie = 1;
					}
					else if (Positie == 2)
					{
						SetPositie(0, 0);
						Positie = 1;
						OudePositie = 2;
					}
					break;
			}
		}
	}
	#endregion
}


/* string[] zonder 1's en 2's
 * private static string[] _finishHorizontal =
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
*/