using Controller;
using Model;
using RaceSimulator_Project;

/*
Console.WriteLine($"Naam track: {Data.CurrentRace.Track.Name}");
*/



Data.Initialize();
Data.NextRace();

Visualization.DrawTrack(Data.CurrentRace.Track);


for (; ; )
{
	Thread.Sleep(100);
}