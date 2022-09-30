using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public class Race
	{
		public Track Track { get; set; }
		public List<IParticipant> Participants { get; set; }
		public DateTime StartTime { get; set; }

		private Random _random;

		private Dictionary<Section, SectionData> _positions { get; set; }

		public Race(Track track, List<IParticipant> participants)
		{
			Track = track;
			Participants = participants;
			_random = new Random(DateTime.Now.Millisecond);
			StartPositions(Track, Participants);
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

		/*
		 * 2 deelnemers op 1 sectie, dus if-statements om te checken.
		 * beginnen met index op de startgrid
		 * als er geen sectie maar beschikbaar is voor een deelnemer dat ook fixen
		 * 
		*/
		public void StartPositions(Track track, List<IParticipant> participants)
		{
			_positions = new Dictionary<Section, SectionData>(); //Without this here it crashes.

			int index = 0;
			int sectionsLength = Track.Sections.Count;
			int participantsRemaining = Participants.Count; //Keep track of amount of participants remaining.


			foreach (Section section in Track.Sections)
			{
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
					if (participantsRemaining != 0)
					{
						sectionData.Right = participants[index];
						index++; //Increase index.
						participantsRemaining--;
					}
					if (participantsRemaining != 0)
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


/*
 * Fix index out of range 
 * Nu gaat ie naar de volgende section maar hij moet naar de voorgaande section.
 */