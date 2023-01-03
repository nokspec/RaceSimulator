using Controller;
using Model;
using RaceSimulator_Project;

//alleen nodig voor windows11.
//Console.SetWindowSize(100, 100);

Data.Initialize();
Data.NextRaceEvent += Visualization.OnNextRaceEvent;
Data.NextRace();

Visualization.Initialize(Data.CurrentRace);
if (Data.CurrentRace != null) Visualization.DrawTrack(Data.CurrentRace.Track);

for (; ; ) //to keep the window opened
{
	Thread.Sleep(100);
}

