using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Competition
    {
        List<IParticipant> Participants { get; set; }
        Queue<Track> Tracks { get; set; }


        public static Track NextTrack()
        {
           
        }

    
}
