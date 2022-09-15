using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Controller
{
	public static class Data
	{
        static Competition Competition { get; set; }

        public static void Initialize()
        {
            Competition = new Competition();
        }
    }
}
