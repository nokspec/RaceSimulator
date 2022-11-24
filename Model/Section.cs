namespace Model
{
	public class Section
	{
		public SectionType SectionTypes { get; set; }
		public Section(SectionType sectionTypes)
		{
			SectionTypes = sectionTypes;
		}
	}

	public enum SectionType
	{
		Straight,
		LeftCorner,
		RightCorner,
		StartGrid,
		Finish
	}
}
