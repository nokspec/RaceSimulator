using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SectionData
    {
        public IParticipant Left { get; set; }

		public IParticipant Right { get; set; }

		//TODO kan weg?
		public Section CurrentSection;
	}
}
