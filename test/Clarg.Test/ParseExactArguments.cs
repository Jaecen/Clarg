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

			var arguments = parser.Create<ExactArguments>(new[] { "--one", "first", "/two", "2" });

			Assert.Equal("first", arguments.One);
			Assert.Equal(2, arguments.Two);
		}
	}
}
