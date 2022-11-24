using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public class InvalidDirectionException :Exception
	{
		public InvalidDirectionException()
		{
		}

		public InvalidDirectionException(string message)
			: base(message)
		{
		}

		public InvalidDirectionException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
