namespace Model
{
	public class Track
	{
		public string Name { get; set; }
		public LinkedList<Section> Sections { get; set; }

		public Track(string name, SectionType[]? sections)
		{
			Name = name;
			Sections = SectionTypeToLinkedList(sections);
		}

		private static LinkedList<Section> SectionTypeToLinkedList(SectionType[]? sectionTypes)
		{
			LinkedList<Section> sectionList = new LinkedList<Section>();

			foreach (SectionType sectionType in sectionTypes)
			{
				sectionList.AddLast(new Section(sectionType));
			}
			return sectionList;
		}
	}
}
