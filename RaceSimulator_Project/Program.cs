using Controller;
using Model;
using RaceSimulator_Project;





Data.Initialize();
Data.NextRace();


Console.WriteLine($"Naam track: {Data.CurrentRace.Track.Name}");



for (; ; )
{
	Thread.Sleep(100);
}

//Visualization.DrawTrack(Data.CurrentRace.Track);