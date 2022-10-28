using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public class RWRaceData<T>
	{
		private List<T> _list;

		public RWRaceData()
		{
			_list = new();
		}

		public void Add(T item)
		{
			_list.Add(item);
		}
	}
}
