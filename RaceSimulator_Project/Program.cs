using Controller;
using Model;
using RaceSimulator_Project;





Data.Initialize();
Data.NextRace();

Visualization.Initialize();
//Console.WriteLine($"Naam track: {Data.CurrentRace.Track.Name}");
Visualization.DrawTrack(Data.CurrentRace.Track);



for (; ; ) //to keep the window opened
{
	Thread.Sleep(100);
}

