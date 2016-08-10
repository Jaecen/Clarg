using Clarg;
using Xunit;

namespace Tests
{
	public class ParseBooleanArguments
	{
		class BooleanArguments
		{
			public readonly bool One;
			public readonly bool Two;
			public readonly bool Three;

			public BooleanArguments(bool one, bool two, bool three)
			{
				One = one;
				Two = two;
				Three = three;
			}
		}

		[Fact]
		public void HandleMultipleBooleanArgumentsWithoutValues()
		{
			var parser = new Parser();

			var result = parser.Create<BooleanArguments>(new[] { "--one", "/two", "--three" });

			Assert.IsType<ParserSuccess<BooleanArguments>>(result);
			Assert.True(result.Value.One);
			Assert.True(result.Value.Two);
			Assert.True(result.Value.Three);
		}
	}
}
