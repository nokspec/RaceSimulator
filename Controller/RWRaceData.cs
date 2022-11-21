using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public class RWRaceData<T>
	{
		private List<T> _list = new();

		public void Add(T value)
		{
			_list.Add(value);
		}
	}
}
