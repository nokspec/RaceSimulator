using Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class ParticipantSectionTime : RWRaceData<IParticipant>
	{
		public IParticipant Name { get; set; }
		public TimeSpan Time { get; set; }
		public Section Section { get; set; }

		private List<TimeSpan> _list = new();


		public ParticipantSectionTime(IParticipant name, TimeSpan time, Section section)
		{
			Name = name;
			Time = time;
			Section = section;
		}

		public void Add<T>(List<T> list) where T : class, IDataContext
		{
			list.Add(this as T);
		}

		public void CalcSectionTime()
		{
			ParticipantSectionTime pst = new ParticipantSectionTime(Name, Time, Section);
			if (pst.Name == Name && pst.Section == Section)
			{
				pst.Time = Time;
			}
		}
	}
}
