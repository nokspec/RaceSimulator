using Controller;
using Model;
using Section = Model.Section;

namespace RaceSimulator_Project
{
	public static class Visualization
	{
		public static int X;
		public static int Y;
		public static int Position;
		public static int OldPosition; //Used for corners
		public static Section CurrentSection;
		private static Race? race;

		public static void Initialize(Race? race)
		{
			Visualization.race = race;
			Data.CurrentRace.DriversChanged += OnDriversChanged;
			ConsolePreparation();
		}
		
		/// <summary>
		/// Prepares the Console for the next race/track.
		/// </summary>
		private static void ConsolePreparation()
		{
			Console.Clear(); //Remove old track
			X = 0;
			Y = 0;
			Position = 1;
			SetPosition(35, 30);
		}

		private static void OnDriversChanged(object source, DriversChangedEventArgs e)
		{
			DrawTrack(e.Track);
		}

		public static void OnNextRaceEvent(object sender, NextRaceEventArgs e)
		{
			Initialize(e.Race);
			Data.CurrentRace.DriversChanged += OnDriversChanged; 
			DrawTrack(Data.CurrentRace.Track);
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

		private static string[] _straightStartHorizontal = 
		{
			"----",
			"  2|",
			" 1| ",
			"----"
		};

		private static string[] _straightStartVertical = 
		{
			"|  |",
			"| 2|",
			"|1 |",
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


		#endregion

		/// <summary>
		/// Prints the track to the console.
		/// Also puts the drivers on their respective positions.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="sectionData"></param>
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
				SetPosition(0, 1); //Crashes without this line
				Console.WriteLine(newS);
			}
		}

		/// <summary>
		/// Replaces the placeholder (1 and 2) with the first letter of a participants name, an empty space (" ") or with an "X".
		/// If a participant is null, it replaces the 1 or 2 with a space.
		/// Otherwise if the Equipment is broken it replaces 1 or 2 with an "X".
		/// Finally, if a participant isn't null AND their Equipment isn't broken;
		/// it'll replace the 1 or 2 with the first letter of their name.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="leftParticipant"></param>
		/// <param name="rightParticipant"></param>
		/// <returns></returns>
		private static string ReplaceString(string input, IParticipant leftParticipant, IParticipant rightParticipant)
		{ 
			string leftP = leftParticipant == null ? " " : leftParticipant.Equipment.IsBroken ? "X" : leftParticipant.Name.Substring(0, 1);
			string rightP = rightParticipant == null ? " " : rightParticipant.Equipment.IsBroken ? "X" : rightParticipant.Name.Substring(0, 1);
			// : is null-coalescing operator.
			return input.Replace("1", leftP).Replace("2", rightP);
		}

		private static string ReplaceString(string input, IParticipant participant)
		{
			participant.CurrentSection = CurrentSection;

			if (race.GetSectionData(participant.CurrentSection).Left == participant)
			{
				return input.Replace("1", participant.Name[0].ToString());
			}
			else if (race.GetSectionData(participant.CurrentSection).Right == participant)
			{
				return input.Replace("2", participant.Name[0].ToString());
			}
			return null;
		}

		/// <summary>
		/// Sets the position for the next section.
		/// </summary>
		/// <param name="xChange"></param>
		/// <param name="yChange"></param>
		private static void SetPosition(int xChange, int yChange)
		{
			X += xChange;
			Y += yChange;
			Console.SetCursorPosition(X, Y);
		}

		#region Draw track
		/// <summary>
		/// Draws the track.
		/// Uses DetermineDirection to determine where to draw the next section.
		/// 1, 3 = horizontal
		/// 2, 4 = vertical
		/// </summary>
		/// <param name="track"></param>
		public static void DrawTrack(Track track)
		{
			foreach (Section section in track.Sections)
			{
				CurrentSection = section;
				switch (section.SectionTypes)
				{
					case SectionType.StartGrid:
						DetermineDirection(SectionType.StartGrid, track);
						PrintToConsole(Position is 1 or 3 ? _straightStartHorizontal : _straightStartVertical,
							race.GetSectionData(section));
						break;

					case SectionType.Finish:
						DetermineDirection(SectionType.Finish, track);
						PrintToConsole(Position is 1 or 3 ? _finishHorizontal : _finishVertical,
							race.GetSectionData(section));
						break;

					case SectionType.Straight:
						DetermineDirection(SectionType.Straight, track);
						PrintToConsole(Position is 1 or 3 ? _straightHorizontal : _straightVertical,
							race.GetSectionData(section));
						break;

					case SectionType.RightCorner:
						DetermineDirection(SectionType.RightCorner, track);
						switch (OldPosition)
						{
							case 1:
								PrintToConsole(_cornerNE, race.GetSectionData(section));
								break;
							case 2:
								PrintToConsole(_cornerSE, race.GetSectionData(section));
								break;
							case 3:
								PrintToConsole(_cornerSW, race.GetSectionData(section));
								break;
							case 4:
								PrintToConsole(_cornerNW, race.GetSectionData(section));
								break;
						}
						break;

					case SectionType.LeftCorner:
						DetermineDirection(SectionType.LeftCorner, track);
						switch (OldPosition)
						{
							case 3:
								PrintToConsole(_cornerNW, race.GetSectionData(section));
								break;
							case 4:
								PrintToConsole(_cornerNE, race.GetSectionData(section));
								break;
							case 1:
								PrintToConsole(_cornerSE, race.GetSectionData(section));
								break;
							case 2:
								PrintToConsole(_cornerSW, race.GetSectionData(section));
								break;
						}
						break;
				}
			}
		}

		/// <summary>
		/// Determines the direction of the next section that has to be drawn.
		/// </summary>
		/// <param name="sectionType"></param>
		/// <param name="track"></param>
		private static void DetermineDirection(SectionType sectionType, Track track)
		{
			switch (sectionType)
			{
				case SectionType.Finish:
					switch (Position)
					{
						case 1:
							SetPosition(4, -4);
							break;
						case 3:
						case 4:
							SetPosition(0, -8);
							break;
						case 2:
							SetPosition(0, 0);
							break;
					}
					break;

				case SectionType.StartGrid:
					switch (Position)
					{
						case 1:
							SetPosition(4, -4);
							break;
						case 3:
						case 4:
							SetPosition(0, -8);
							break;
						case 2:
							SetPosition(0, 0);
							break;
					}
					break;

				case SectionType.Straight:
					switch (Position)
					{
						case 1:
							SetPosition(4, -4);
							break;
						case 3:
							SetPosition(-4, -4);
							break;
						case 2:
							SetPosition(0, 0);
							break;
						case 4:
							SetPosition(0, -8);
							break;
					}
					break;

				case SectionType.RightCorner:
					switch (Position)
					{
						case 1:
							SetPosition(4, -4);
							Position = 2;
							OldPosition = 1;
							break;
						case 2:
							SetPosition(0, 0);
							Position = 3;
							OldPosition = 2;
							break;
						case 3:
							SetPosition(-4, -4);
							Position = 4;
							OldPosition = 3;
							break;
						case 4:
							SetPosition(0, -8);
							Position = 1;
							OldPosition = 4;
							break;
					}
					break;

				case SectionType.LeftCorner:
					switch (Position)
					{
						case 3:
							SetPosition(-4, -4);
							Position = 2;
							OldPosition = 3;
							break;
						case 4:
							SetPosition(0, -8);
							Position = 3;
							OldPosition = 4;
							break;
						case 1:
							SetPosition(4, -4);
							Position = 4;
							OldPosition = 1;
							break;
						case 2:
							SetPosition(0, 0);
							Position = 1;
							OldPosition = 2;
							break;
					}
					break;
			}
		}
	}
	#endregion
}
