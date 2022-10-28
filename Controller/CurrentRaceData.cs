using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
	public class CurrentRaceData<T>
	{
		private List<T> _list = new List<T>();

		public void Add(T item)
		{
			_list.Add(item);
		}


	}
}
