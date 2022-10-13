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
		public static int _position;
		public static int _oldPosition; //Is nodig voor de bochten.
		public static Section CurrentSection;
		private static Race _race;

		public static void Initialize(Race race)
		{
			CurrentSection = null; //When initializing there's no _currentSection.
			
			_race = race;
			Data.CurrentRace.DriversChanged += OnDriversChanged;
			ConsolePreparation();
		}

		public static void ConsolePreparation()
		{
			Console.Clear(); //Oude track weghalen.
			X = 0;
			Y = 0;
			_position = 1;
			SetPositie(35, 30);
		}

		//Event
		private static void OnDriversChanged(object source, DriversChangedEventArgs e)
		{
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
			" 1 s",
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
			"2  |",
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

		private static void PrintToConsole(string[] array, SectionData sectionData)
		{
			foreach (string s in array)
			{
				string newS = s;
				if (sectionData.Left != null)
					newS = ReplaceString(s, sectionData.Left);
				else if (sectionData.Right != null)
				{
					newS = ReplaceString(s, sectionData.Right);
				}
				newS = ReplaceString(s, sectionData.Left, sectionData.Right);
				SetPositie(0, 1); //Important
				Console.WriteLine(newS);
			}
		}

		private static void CheckParticipantFinished(IParticipant participant) //Dit nog uitwerken. Level 5-4
		{
			bool result = false;
			if (participant.Finished)
				result = true;
		}
		private static string ReplaceString(string input, IParticipant leftParticipant, IParticipant rightParticipant)
		{
			return input.Replace("1", (leftParticipant?.Name?.Substring(0, 1)) ?? " ").Replace("2", (rightParticipant?.Name?.Substring(0, 1)) ?? " ");
		}

		private static string ReplaceString(string input, IParticipant participant)
		{
			participant.CurrentSection = CurrentSection; //Geef CurrentSection mee aan Participant.CurrentSection
			if (_race.GetSectionData(participant.CurrentSection).Left == participant)
			{
				return input.Replace("1", participant.Name[0].ToString());
			}
			else if (_race.GetSectionData(participant.CurrentSection).Right == participant)
			{
				return input.Replace("2", participant.Name[0].ToString());
			}
			return null;
		}

		private static void SetPositie(int xVerandering, int yVerandering)
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
						if (_position == 1 || _position == 3)
							PrintToConsole(_straightStartHorizontal, _race.GetSectionData(section));
						else
						{
							PrintToConsole(_straightStartVertical, _race.GetSectionData(section));
						}
						break;

					case SectionType.Finish:
						DetermineDirection(SectionType.Finish, track);
						if (_position == 1 || _position == 3)
							PrintToConsole(_finishHorizontal, _race.GetSectionData(section));
						else
						{
							PrintToConsole(_finishVertical, _race.GetSectionData(section));
						}
						break;

					case SectionType.Straight:
						DetermineDirection(SectionType.Straight, track);
						if (_position == 1 || _position == 3)
							PrintToConsole(_straightHorizontal, _race.GetSectionData(section));
						else
						{
							PrintToConsole(_straightVertical, _race.GetSectionData(section));
						}
						break;

					case SectionType.RightCorner:
						DetermineDirection(SectionType.RightCorner, track);
						if (_oldPosition == 1)
							PrintToConsole(_cornerNE, _race.GetSectionData(section));
						else if (_oldPosition == 2)
							PrintToConsole(_cornerSE, _race.GetSectionData(section));
						else if (_oldPosition == 3)
							PrintToConsole(_cornerSW, _race.GetSectionData(section));
						else if (_oldPosition == 4)
							PrintToConsole(_cornerNW, _race.GetSectionData(section));
						break;

					case SectionType.LeftCorner:
						DetermineDirection(SectionType.LeftCorner, track);
						if (_oldPosition == 3)
							PrintToConsole(_cornerNW, _race.GetSectionData(section));
						else if (_oldPosition == 4)
							PrintToConsole(_cornerNE, _race.GetSectionData(section));
						else if (_oldPosition == 1)
							PrintToConsole(_cornerSE, _race.GetSectionData(section));
						else if (_oldPosition == 2)
							PrintToConsole(_cornerSW, _race.GetSectionData(section));
						break;
				}
			}
		}

		private static void DetermineDirection(SectionType sectionType, Track track)
		{
			switch (sectionType)
			{
				case SectionType.Finish:
					if (_position == 1)
					{
						SetPositie(4, -4);
					}
					else if (_position == 3)
					{
						SetPositie(0, -8);
					}
					else if (_position == 2)
					{
						SetPositie(0, 0);
					}
					else if (_position == 4)
					{
						SetPositie(0, -8);
					}
					break;

				case SectionType.StartGrid:
					if (_position == 1)
					{
						SetPositie(4, -4);
					}
					else if (_position == 3)
					{
						SetPositie(0, -8);
					}
					else if (_position == 2)
					{
						SetPositie(0, 0);
					}
					else if (_position == 4)
					{
						SetPositie(0, -8);
					}
					break;

				case SectionType.Straight:
					if (_position == 1)
					{
						SetPositie(4, -4);
					}
					else if (_position == 3)
					{
						SetPositie(-4, -4);
					}
					else if (_position == 2)
					{
						SetPositie(0, 0);
					}
					else if (_position == 4)
					{
						SetPositie(0, -8);
					}
					break;

				case SectionType.RightCorner:
					if (_position == 1)
					{
						SetPositie(4, -4);
						_position = 2;
						_oldPosition = 1;
					}
					else if (_position == 2)
					{
						SetPositie(0, 0);
						_position = 3;
						_oldPosition = 2;
					}
					else if (_position == 3)
					{
						SetPositie(-4, -4);
						_position = 4;
						_oldPosition = 3;
					}
					else if (_position == 4)
					{
						SetPositie(0, -8);
						_position = 1;
						_oldPosition = 4;
					}
					break;

				case SectionType.LeftCorner:
					if (_position == 3)
					{
						SetPositie(-4, -4);
						_position = 2;
						_oldPosition = 3;
					}
					else if (_position == 4)
					{
						SetPositie(0, -8);
						_position = 3;
						_oldPosition = 4;
					}
					else if (_position == 1)
					{
						SetPositie(4, -4);
						_position = 4;
						_oldPosition = 1;
					}
					else if (_position == 2)
					{
						SetPositie(0, 0);
						_position = 1;
						_oldPosition = 2;
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