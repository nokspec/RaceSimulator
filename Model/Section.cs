using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Section
    {
        public SectionType SectionTypes { get; set; }

        public Section(SectionType sectionTypes)
        {
	        SectionTypes = sectionTypes;
        }

    }

    public enum SectionType
    {
	    Straight,
	    LeftCorner,
	    RightCorner,
	    StartGrid,
	    Finish
    }
}
