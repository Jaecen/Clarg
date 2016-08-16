namespace Clarg
{
	class FormattedArgument
	{
		public readonly ParserSuggestionArgument Argument;
		public readonly string DisplayName;
		public readonly string DisplayType;

		public FormattedArgument(ParserSuggestionArgument argument, string displayName, string displayType)
		{
			Argument = argument;
			DisplayName = displayName;
			DisplayType = displayType;
		}
	}
}
