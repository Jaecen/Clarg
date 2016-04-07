using Clarg;
using Xunit;

namespace Tests
{
	public class ParseOptionalArguments
	{
		class OptionalArguments
		{
			public readonly string One;
			public readonly int Two;
			public readonly bool? Three;

			public OptionalArguments(string one, int two, bool? three = null)
			{
				One = one;
				Two = two;
				Three = three;
			}
		}

		[Fact]
		public void HandleOptionalArgumentsWithNoValue()
		{
			var parser = new Parser();

			var arguments = parser.Create<OptionalArguments>(new[] { "--one", "first", "/two", "2", });

			Assert.Equal("first", arguments.One);
			Assert.Equal(2, arguments.Two);
			Assert.Equal(null, arguments.Three);
		}

		[Fact]
		public void HandleOptionalArgumentsWithValue()
		{
			var parser = new Parser();

			var arguments = parser.Create<OptionalArguments>(new[] { "--one", "first", "/two", "2", "--three", "false" });

			Assert.Equal("first", arguments.One);
			Assert.Equal(2, arguments.Two);
			Assert.Equal(false, arguments.Three);
		}

		[Fact]
		public void HandleOptionalArgumentsWithPresentAsBool()
		{
			var parser = new Parser();

			var arguments = parser.Create<OptionalArguments>(new[] { "--one", "first", "/two", "2", "--three" });

			Assert.Equal("first", arguments.One);
			Assert.Equal(2, arguments.Two);
			Assert.Equal(true, arguments.Three);
		}
	}
}
