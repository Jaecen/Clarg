using Clarg;
using Xunit;

namespace Tests
{
	public class ParseExactArguments
	{
		class ExactArguments
		{
			public readonly string One;
			public readonly int Two;

			public ExactArguments(string one, int two)
			{
				One = one;
				Two = two;
			}
		}

		[Fact]
		public void HandleExactArguments()
		{
			var parser = new Parser();

			var result = parser.Create<ExactArguments>(
				"-",
				new[]
				{
					"-one", "first",
					"-two", "2"
				});

			Assert.IsType<ParserSuccess<ExactArguments>>(result);
			Assert.Equal("first", result.Value.One);
			Assert.Equal(2, result.Value.Two);
		}
	}
}
