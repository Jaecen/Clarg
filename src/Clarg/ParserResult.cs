using System.Collections.Generic;
using System.Linq;

namespace Clarg
{
	public class ParserResult<T>
		where T : class
	{
		public readonly T Value;
		public readonly IEnumerable<ParserSuggestion> Suggestions;

		protected ParserResult(T value)
		{
			Value = value;
			Suggestions = null;
		}

		protected ParserResult(IEnumerable<ParserSuggestion> suggestions)
		{
			Value = null;
			Suggestions = suggestions;
		}
	}

	public class ParserSuccess<T> : ParserResult<T>
		where T : class
	{
		public ParserSuccess(T value)
			: base(value: value)
		{ }
	}

	public class ParserError<T> : ParserResult<T>
		where T : class
	{
		public ParserError(IEnumerable<ParserSuggestion> suggestions)
			: base(suggestions: suggestions ?? Enumerable.Empty<ParserSuggestion>())
		{ }
	}
}
