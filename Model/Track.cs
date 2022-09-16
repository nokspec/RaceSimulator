using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Track
	{
		public string Name { get; set; }
		LinkedList<Section> Sections { get; set; }

		public Track(string name, LinkedList<Section> sectionTypes)
		{
			Name = name;
			Sections = sectionTypes;
		}
	}
}
