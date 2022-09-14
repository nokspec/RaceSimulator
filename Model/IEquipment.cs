using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IEquipment
    {
        int Quality { get; set; }
        int Performance { get; set; }
        int Speed { get; set; }
        bool IsBroken { get; set; }
    }
}
