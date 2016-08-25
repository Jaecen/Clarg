namespace Clarg
{
	class FormattedArgument
	{
		public readonly ParserSuggestionArgument Argument;
		public readonly ConsoleString DisplayName;
		public readonly ConsoleString DisplayType;

		public FormattedArgument(ParserSuggestionArgument argument, ConsoleString displayName, ConsoleString displayType)
		{
			Argument = argument;
			DisplayName = displayName;
			DisplayType = displayType;
		}
	}
}
