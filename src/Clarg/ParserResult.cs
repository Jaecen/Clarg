using System;
using System.Collections.Generic;
using System.Linq;

namespace Clarg
{
	public class ParserResult<T>
		where T : class
	{
		public readonly T Value;
		public readonly IEnumerable<ParserSuggestion> Suggestions;
		public readonly Exception Error;

		protected ParserResult(T value, IEnumerable<ParserSuggestion> suggestions, Exception error)
		{
			Value = value;
			Suggestions = suggestions;
			Error = error;
		}
	}

	public class ParserSuccess<T> : ParserResult<T>
		where T : class
	{
		public ParserSuccess(T value)
			: base(value, null, null)
		{ }
	}

	public class ParserSuggestions<T> : ParserResult<T>
		where T : class
	{
		public ParserSuggestions(IEnumerable<ParserSuggestion> suggestions)
			: base(null, suggestions ?? Enumerable.Empty<ParserSuggestion>(), null)
		{ }
	}

	public class ParserError<T> : ParserResult<T>
		where T : class
	{
		public ParserError(Exception error)
			: base(null, null, error)
		{ }
	}
}
