using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public class InvalidTeamColorException : Exception
	{
		public InvalidTeamColorException()
		{
		}

		public InvalidTeamColorException(string message)
			: base(message)
		{
		}

		public InvalidTeamColorException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
