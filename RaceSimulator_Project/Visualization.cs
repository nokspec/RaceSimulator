using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace RaceSimulator_Project
{
	public static class Visualization
	{
		public static void Initialize()
		{

		}

		#region graphics

		private static string[] _finishHorizontal = { "----", "  # ", "  # ", "----" };
		private static string[] _finishVertical = { "|  |", "|  |", "|  |", "|  |" };
		private static string[] _startHorizontal = { "----", "  # ", "  # ", "----" };
		private static string[] _startVertical = { "|  |", "|  |", "|  |", "|  |" };
		private static string[] _straightHorizontal = { "----", "    ", "    ", "----" };
		private static string[] _straightVertical = { "|  |", "|  |", "|  |", "|  |" };
		private static string[] _leftCornerHorizontal = { "----", "    ", "    ", "----" };
		private static string[] _leftCornerVertical = { "|  |", "|  |", "|  |", "|  |" };


		#endregion

		public static void startHorizontal()
		{
			for (int i = 0; i < _startHorizontal.Length; i++)
			{
				Console.WriteLine(_startHorizontal[i]);
			}
		}

		public static void DrawTrack(Track track)
		{
			startHorizontal();
			
		}

	}
}

