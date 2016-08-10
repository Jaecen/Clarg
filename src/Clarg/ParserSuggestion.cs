using System.Collections.Generic;

namespace Clarg
{
	public class ParserSuggestion
	{
		public readonly string Description;
		public readonly IEnumerable<ParserSuggestionParameter> Parameters;

		public ParserSuggestion(string description, IEnumerable<ParserSuggestionParameter> parameters)
		{
			Description = description;
			Parameters = parameters;
		}
	}
}
