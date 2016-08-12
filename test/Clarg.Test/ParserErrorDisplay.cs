using System.Collections.Generic;
using Clarg;
using Xunit;

namespace Tests
{
	public class ParserErrorDisplay
    {
		const string Command = "test.exe";
		const string StringConstructor = nameof(StringConstructor);
		const string OptionalConstructor = nameof(OptionalConstructor);
		const string ParamsConstructor = nameof(ParamsConstructor);

		const string StringParameter = nameof(StringParameter);
		const string EnumerableIntParameter = nameof(EnumerableIntParameter);
		const string IntParameter = nameof(IntParameter);
		const string BoolParameter = nameof(BoolParameter);
		const string ParamsParameter = nameof(ParamsParameter);

		class DecoratedArguments
		{
			[ArgumentDescription(StringConstructor)]
			public DecoratedArguments(
				[ArgumentDescription(StringParameter)] string one)
			{ }

			[ArgumentDescription(OptionalConstructor)]
			public DecoratedArguments(
				[ArgumentDescription(EnumerableIntParameter)] IEnumerable<int> two,
				[ArgumentDescription(BoolParameter)] bool three = true)
			{ }

			[ArgumentDescription(ParamsConstructor)]
			public DecoratedArguments(
				[ArgumentDescription(StringParameter)] string one,
				[ArgumentDescription(IntParameter)] int two,
				[ArgumentDescription(ParamsParameter)] params KeyValuePair<string, string>[] remainder)
			{ }
		}

		[Fact]
		void Should_Format_Error_Message()
		{
			var parser = new Parser();
			var parserResult = parser.Create<DecoratedArguments>(new string[0]);

			Assert.IsType<ParserError<DecoratedArguments>>(parserResult);

			var parserSuggestionFormatter = new ParserSuggestionFormatter();
			var result = parserSuggestionFormatter.CreateErrorMessage(Command, parserResult.Suggestions);

			Assert.NotNull(result);
			Assert.Contains(Command, result);
			Assert.Contains(StringConstructor, result);
			Assert.Contains(OptionalConstructor, result);
			Assert.Contains(ParamsConstructor, result);
			Assert.Contains(StringParameter, result);
			Assert.Contains(EnumerableIntParameter, result);
			Assert.Contains(IntParameter, result);
			Assert.Contains(BoolParameter, result);
			Assert.Contains(ParamsParameter, result);
		}
	}
}
