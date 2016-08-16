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

			var result = parser.Create<OptionalArguments>(new[] { "--one", "first", "/two", "2", });

			Assert.IsType<ParserSuccess<OptionalArguments>>(result);
			Assert.Equal("first", result.Value.One);
			Assert.Equal(2, result.Value.Two);
			Assert.Equal(null, result.Value.Three);
		}

		[Fact]
		public void HandleOptionalArgumentsWithValue()
		{
			var parser = new Parser();

			var result = parser.Create<OptionalArguments>(new[] { "--one", "first", "/two", "2", "--three", "false" });

			Assert.IsType<ParserSuccess<OptionalArguments>>(result);
			Assert.Equal("first", result.Value.One);
			Assert.Equal(2, result.Value.Two);
			Assert.Equal(false, result.Value.Three);
		}

		[Fact]
		public void HandleOptionalArgumentsWithPresentAsBool()
		{
			var parser = new Parser();

			var result = parser.Create<OptionalArguments>(new[] { "--one", "first", "/two", "2", "--three" });

			Assert.IsType<ParserSuccess<OptionalArguments>>(result);
			Assert.Equal("first", result.Value.One);
			Assert.Equal(2, result.Value.Two);
			Assert.Equal(true, result.Value.Three);
		}
	}
}
