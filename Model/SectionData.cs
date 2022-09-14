using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SectionData
    {
        IParticipant Left { get; set; }
        int DistanceLeft { get; set; }

        IParticipant Right { get; set; }
        int DistanceRight { get; set; }
    }
}
