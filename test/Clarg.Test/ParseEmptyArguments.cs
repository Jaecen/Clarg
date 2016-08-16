using System.Collections.Generic;
using Clarg;
using Xunit;

namespace Tests
{
	public class ParseEmptyArguments
	{
		class NoArguments
		{ }

		[Fact]
		void Successfully_Parses_No_Parameters()
		{
			var parser = new Parser();

			var result = parser.Create<NoArguments>(new string[0]);

			Assert.IsType<ParserSuccess<NoArguments>>(result);
		}

		class RequiredArguments
		{
			public readonly string One;

			public RequiredArguments(string one)
			{
				One = one;
			}
		}

		[Fact]
		void Errors_When_Parsing_Required_Parameters()
		{
			var parser = new Parser();

			var result = parser.Create<RequiredArguments>(new string[0]);

			Assert.IsType<ParserError<RequiredArguments>>(result);
		}

		class OptionalArguments
		{
			public readonly string One;

			public OptionalArguments(string one = "default")
			{
				One = one;
			}
		}

		[Fact]
		void Successfully_Parses_All_Optional_Parameters()
		{
			var parser = new Parser();

			var result = parser.Create<OptionalArguments>(new string[0]);

			Assert.IsType<ParserSuccess<OptionalArguments>>(result);
		}

		class ParamsArguments
		{
			public readonly IEnumerable<KeyValuePair<string, string>> Everything;

			public ParamsArguments(params KeyValuePair<string, string>[] everything)
			{
				Everything = everything;
			}
		}

		[Fact]
		void Successfully_Parses_Only_Params()
		{
			var parser = new Parser();

			var result = parser.Create<ParamsArguments>(new string[0]);

			Assert.IsType<ParserSuccess<ParamsArguments>>(result);
		}
	}
}
