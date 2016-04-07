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

			var arguments = parser.Create<BooleanArguments>(new[] { "--one", "/two", "--three" });

			Assert.True(arguments.One);
			Assert.True(arguments.Two);
			Assert.True(arguments.Three);
		}
	}
}
