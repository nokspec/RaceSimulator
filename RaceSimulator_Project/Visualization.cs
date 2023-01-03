using Controller;
using Model;
using Section = Model.Section;

namespace RaceSimulator_Project
{
	public static class Visualization
	{
		private static int _x;
		private static int _y;
		private static int _position;
		private static int _oldPosition; //Used for corners
		private static Section? _currentSection;
		private static Race? _race;

		public static void Initialize(Race? race)
		{
			_race = race;
			if (Data.CurrentRace != null) Data.CurrentRace.DriversChanged += OnDriversChanged;
			ConsolePreparation();
		}

		/// <summary>
		/// Prepares the Console for the next race/track.
		/// </summary>
		private static void ConsolePreparation()
		{
			Console.Clear(); //Remove old track
			_x = 0;
			_y = 0;
			_position = 1;
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

		private static readonly string[] _finishHorizontal =
		{
			"----",
			"  2#",
			" 1 #",
			"----"
		};

		private static readonly string[] _finishVertical =
		{
			"|##|",
			"| 2|",
			"|1 |",
			"|  |"
		};

		private static readonly string[] _straightStartHorizontal =
		{
			"----",
			"  2|",
			" 1| ",
			"----"
		};

		private static readonly string[] _straightStartVertical =
		{
			"|  |",
			"| 2|",
			"|1 |",
			"|  |"
		};

		private static readonly string[] _straightHorizontal =
		{
			"----",
			"  2 ",
			" 1  ",
			"----"
		};

		private static readonly string[] _straightVertical =
		{
			"|  |",
			"| 1|",
			"|2 |",
			"|  |"
		};

		private static readonly string[] _cornerNE =
		{
			"----",
			"2  |",
			" 1 |",
			"   |"
		};

		private static readonly string[] _cornerSE =
		{
			"   |",
			"  2|",
			" 1 |",
			"----"
		};

		private static readonly string[] _cornerSW =
		{
			"|   ",
			"|  2",
			"| 1 ",
			"----"
		};

		private static readonly string[] _cornerNW =
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
				string newS;
				if (sectionData.Left != null)
					ReplaceString(s, sectionData.Left);
				else if (sectionData.Right != null)
				{
					ReplaceString(s, sectionData.Right);
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
			string LeftParticipant = leftParticipant == null ? " " : leftParticipant.Equipment.IsBroken ? "X" : leftParticipant.Name[..1];
			string RightParticipant = rightParticipant == null ? " " : rightParticipant.Equipment.IsBroken ? "X" : rightParticipant.Name[..1];
			return input.Replace("1", LeftParticipant).Replace("2", RightParticipant);
		}

		private static string ReplaceString(string input, IParticipant participant)
		{
			if (_currentSection != null) participant.CurrentSection = _currentSection;

			if (_race != null && _race.GetSectionData(participant.CurrentSection).Left == participant)
			{
				return input.Replace("1", participant.Name[0].ToString());
			}
			if (_race != null && _race.GetSectionData(participant.CurrentSection).Right == participant)
			{
				return input.Replace("2", participant.Name[0].ToString());
			}
			return null!;
		}

		/// <summary>
		/// Sets the position for the next section.
		/// </summary>
		/// <param name="xChange"></param>
		/// <param name="yChange"></param>
		private static void SetPosition(int xChange, int yChange)
		{
			_x += xChange;
			_y += yChange;
			Console.SetCursorPosition(_x, _y);
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
				_currentSection = section;
				if (_race != null)
				{
					switch (section.SectionTypes)
					{
						case SectionType.StartGrid:
							DetermineDirection(SectionType.StartGrid);
							PrintToConsole(_position is 1 or 3 ? _straightStartHorizontal : _straightStartVertical,
								_race.GetSectionData(section));
							break;

						case SectionType.Finish:
							DetermineDirection(SectionType.Finish);
							PrintToConsole(_position is 1 or 3 ? _finishHorizontal : _finishVertical,
								_race.GetSectionData(section));
							break;

						case SectionType.Straight:
							DetermineDirection(SectionType.Straight);
							PrintToConsole(_position is 1 or 3 ? _straightHorizontal : _straightVertical,
								_race.GetSectionData(section));
							break;

						case SectionType.RightCorner:
							DetermineDirection(SectionType.RightCorner);
							switch (_oldPosition)
							{
								case 1:
									PrintToConsole(_cornerNE, _race.GetSectionData(section));
									break;
								case 2:
									PrintToConsole(_cornerSE, _race.GetSectionData(section));
									break;
								case 3:
									PrintToConsole(_cornerSW, _race.GetSectionData(section));
									break;
								case 4:
									PrintToConsole(_cornerNW, _race.GetSectionData(section));
									break;
							}

							break;

						case SectionType.LeftCorner:
							DetermineDirection(SectionType.LeftCorner);
							switch (_oldPosition)
							{
								case 3:
									PrintToConsole(_cornerNW, _race.GetSectionData(section));
									break;
								case 4:
									PrintToConsole(_cornerNE, _race.GetSectionData(section));
									break;
								case 1:
									PrintToConsole(_cornerSE, _race.GetSectionData(section));
									break;
								case 2:
									PrintToConsole(_cornerSW, _race.GetSectionData(section));
									break;
							}

							break;
					}
				}
			}
		}

		/// <summary>
		/// Determines the direction of the next section that has to be drawn.
		/// </summary>
		/// <param name="sectionType"></param>
		private static void DetermineDirection(SectionType sectionType)
		{
			switch (sectionType)
			{
				case SectionType.Finish:
					switch (_position)
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
					switch (_position)
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
					switch (_position)
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
					switch (_position)
					{
						case 1:
							SetPosition(4, -4);
							_position = 2;
							_oldPosition = 1;
							break;
						case 2:
							SetPosition(0, 0);
							_position = 3;
							_oldPosition = 2;
							break;
						case 3:
							SetPosition(-4, -4);
							_position = 4;
							_oldPosition = 3;
							break;
						case 4:
							SetPosition(0, -8);
							_position = 1;
							_oldPosition = 4;
							break;
					}
					break;

				case SectionType.LeftCorner:
					switch (_position)
					{
						case 3:
							SetPosition(-4, -4);
							_position = 2;
							_oldPosition = 3;
							break;
						case 4:
							SetPosition(0, -8);
							_position = 3;
							_oldPosition = 4;
							break;
						case 1:
							SetPosition(4, -4);
							_position = 4;
							_oldPosition = 1;
							break;
						case 2:
							SetPosition(0, 0);
							_position = 1;
							_oldPosition = 2;
							break;
					}
					break;
			}
		}
	}
	#endregion
}
