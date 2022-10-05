using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using Section = Model.Section;

namespace Controller
{
	public class Race
	{
		public event EventHandler ThresholdReached; //idk
		public event EventHandler<DriversChangedEventArgs> DriversChanged; //L5-5

		public Track Track { get; set; }
		public List<IParticipant> Participants { get; set; }
		public DateTime StartTime { get; set; }
		private Random _random;
		private System.Timers.Timer timer; //Timer
		private Dictionary<Section, SectionData> _positions { get; set; } //Participants positions with left and right.

		public Race(Track track, List<IParticipant> participants)
		{
			Track = track;
			Participants = participants;
			_random = new Random(DateTime.Now.Millisecond);
			StartPositions(Track, Participants);

			//Timer
			timer = new System.Timers.Timer(500);
			timer.Elapsed += OnTimedEvent;
		}

		public void OnTimedEvent(object sender, EventArgs e)
		{
			EventHandler handler = ThresholdReached;
			handler?.Invoke(this, e);
		}

		public void Start()
		{
			StartTime = DateTime.Now;
			timer.Elapsed += OnTimedEvent;
			timer.AutoReset = true;
			timer.Enabled = true;
		}

		public SectionData GetSectionData(Section section)
		{
			if (!_positions.ContainsKey(section))
			{
				_positions.Add(section, new SectionData());
			}

			return _positions[section];
		}

		public void RandomizeEquipment()
		{
			foreach (IParticipant participant in Participants)
			{
				participant.Equipment.Quality = _random.Next(0, 10);
				participant.Equipment.Performance = _random.Next(0, 10);
			}
		}


		public void StartPositions(Track track, List<IParticipant> participants) //Place participants on their start position.
		{
			_positions = new Dictionary<Section, SectionData>(); //Without this here it crashes.

			int index = 0;
			int sectionsLength = Track.Sections.Count;
			int participantsRemaining = Participants.Count; //Keep track of amount of participants remaining.

			for (int i = Track.Sections.Count; i > 0; i--)
			{
				var section = Track.Sections.ElementAt(i - 1); //Get the section at the current index.
				var sectionData = GetSectionData(section);
				if (section.SectionTypes == SectionType.StartGrid)
				{
					sectionData.Right = participants[index];
					index++; //Increase index.
					sectionData.Left = participants[index];
					index++; //Increase index

					participantsRemaining = participantsRemaining - 2;

					GetSectionData(section); //Add participants to _positions
				}
				else if (section.SectionTypes == SectionType.Finish)
				{
					break; //Race ended.
				}
				else if (sectionsLength == 0) //If there's no more sections available.
				{
					break; //No more space left for participants.
				}
				else //Go back one section and add the participants there.
				{
					if (participantsRemaining != 0) //Avoid trying to add a non-existing participant to a section.
					{
						sectionData.Right = participants[index];
						index++; //Increase index.
						participantsRemaining--;
					}
					if (participantsRemaining != 0) //Avoid trying to add a non-existing participant to a section.
					{
						sectionData.Left = participants[index];
						index++; //Increase index.
						participantsRemaining--;
					}
					GetSectionData(section); //Add participant(s) to _positions
				}
				sectionsLength--;
			}
		}
	}
}


