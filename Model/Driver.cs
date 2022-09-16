using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Model.IParticipant;

namespace Model
{
    public class Driver : IParticipant
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColors { get; set; }

        public Driver(string name, int points, IEquipment equipment, TeamColors teamColors)
        {

            Name = name;
            Points = points;
            Equipment = equipment;
            TeamColors = teamColors;

        }
    }
}
