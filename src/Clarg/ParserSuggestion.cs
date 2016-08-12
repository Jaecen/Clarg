using System.Collections.Generic;

namespace Clarg
{
	public class ParserSuggestion
	{
		public readonly string Description;
		public readonly IEnumerable<ParserSuggestionArgument> Arguments;

		public ParserSuggestion(string description, IEnumerable<ParserSuggestionArgument> arguments)
		{
			Description = description;
			Arguments = arguments;
		}
	}
}
