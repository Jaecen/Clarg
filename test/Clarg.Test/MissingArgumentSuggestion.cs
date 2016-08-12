using System;
using System.Collections.Generic;
using System.Linq;
using Clarg;
using Xunit;

namespace Tests
{
	public class MissingArgumentSuggestion
	{
		class ArgumentsDifferentiatedByType
		{
			public ArgumentsDifferentiatedByType(string one, bool two)
			{ }

			public ArgumentsDifferentiatedByType(string one, int two)
			{ }

			public ArgumentsDifferentiatedByType(string one, bool two, decimal three, float four)
			{ }

			public ArgumentsDifferentiatedByType(string one, int two, decimal three, float four)
			{ }
		}

		[Theory]
		[InlineData(new object[] { "/one", "one" }, 2)]
		[InlineData(new object[] { "/one", "one", "-two", "false", "/three", "3.0" }, 4)]
		public void Should_Select_All_Constructors_With_Lowest_Missing_Parameter_Count(string[] args, int expectedSuggestionCount)
		{
			var parser = new Parser();

			var result = parser.Create<ArgumentsDifferentiatedByType>(args);

			Assert.IsType<ParserError<ArgumentsDifferentiatedByType>>(result);

			Assert.Equal(expectedSuggestionCount, result.Suggestions.Count());
		}
	}
}
