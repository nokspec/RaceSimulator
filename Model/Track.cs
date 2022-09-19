using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Track
	{
		public string Name { get; set; }
		public LinkedList<Section> Sections { get; set; }

		public Track(string name, LinkedList<Section> sectionTypes)
		{
			Name = name;
			Sections = SectionTypeToLinkedList(sectionTypes);
		}

		public LinkedList<Section> SectionTypeToLinkedList(LinkedList<Section> sectionTypes)
		{
			LinkedList<Section> sectionList = new LinkedList<Section>();

			foreach (Section sectionType in sectionTypes)
			{
				sectionList.AddLast(sectionType);
			}
			return sectionList;
		}
	}
}
