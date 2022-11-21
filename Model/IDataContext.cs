using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public interface IDataContext
	{
		public string Name { get; set; }

		public void Add<T>(List<T> list) where T : class, IDataContext;

		public string BestParticipant<T>(List<T> list) where T : class, IDataContext;
	}
}
