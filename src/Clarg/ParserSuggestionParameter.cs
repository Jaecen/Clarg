namespace Clarg
{
	public class ParserSuggestionParameter
	{
		public readonly string Name;
		public readonly string Description;
		public readonly bool IsOptional;

		public ParserSuggestionParameter(string name, string description, bool isOptional)
		{
			Name = name;
			Description = description;
			IsOptional = isOptional;
		}
	}
}
