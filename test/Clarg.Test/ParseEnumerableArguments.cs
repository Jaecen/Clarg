using System.Collections.Generic;
using System.Linq;
using Clarg;
using Xunit;

namespace Tests
{
	public class ParseEnumerableArguments
	{
		class EnumerableArguments
		{
			public IEnumerable<int> Numbers;

			public EnumerableArguments(IEnumerable<int> numbers)
			{
				Numbers = numbers;
			}
		}

		[Fact]
		public void HandlesEnumerableParametersWithASingleValue()
		{
			var parser = new Parser();

			var result = parser.Create<EnumerableArguments>(new[] {
					"/numbers", "1",
				});

			Assert.IsType<ParserSuccess<EnumerableArguments>>(result);
			Assert.NotNull(result.Value.Numbers);
			Assert.Equal(1, result.Value.Numbers.Count());
			Assert.Equal(1, result.Value.Numbers.First());
		}

		[Fact]
		public void HandlesEnumerableParametersWithMultipleValues()
		{
			var parser = new Parser();

			var result = parser.Create<EnumerableArguments>(new[] {
					"/numbers", "1",
					"--numbers", "2",
					"/Numbers", "3",
				});

			Assert.IsType<ParserSuccess<EnumerableArguments>>(result);
			Assert.NotNull(result.Value.Numbers);
			Assert.Equal(3, result.Value.Numbers.Count());
			Assert.Equal(1, result.Value.Numbers.First());
			Assert.Equal(2, result.Value.Numbers.Skip(1).First());
			Assert.Equal(3, result.Value.Numbers.Skip(2).First());
		}

		class OptionalEnumerableArguments
		{
			public double First;
			public IEnumerable<int> Numbers;

			public OptionalEnumerableArguments(double first, IEnumerable<int> numbers = null)
			{
				First = first;
				Numbers = numbers;
			}
		}

		[Fact]
		public void HandlesOptionalEnumerableParametersWithNoValues()
		{
			var parser = new Parser();

			var result = parser.Create<OptionalEnumerableArguments>(new[] {
					"/first", "3.14",
				});

			Assert.IsType<ParserSuccess<OptionalEnumerableArguments>>(result);
			Assert.Equal(3.14, result.Value.First);
			Assert.Null(result.Value.Numbers);
		}

		[Fact]
		public void HandlesOptionalEnumerableParametersWithASingleValue()
		{
			var parser = new Parser();

			var result = parser.Create<OptionalEnumerableArguments>(new[] {
					"/first", "3.14",
					"/numbers", "1",
				});

			Assert.IsType<ParserSuccess<OptionalEnumerableArguments>>(result);
			Assert.Equal(3.14, result.Value.First);
			Assert.NotNull(result.Value.Numbers);
			Assert.Equal(1, result.Value.Numbers.Count());
			Assert.Equal(1, result.Value.Numbers.First());
		}

		[Fact]
		public void HandlesOptionalEnumerableParametersWithMultipleValues()
		{
			var parser = new Parser();

			var result = parser.Create<OptionalEnumerableArguments>(new[] {
					"/first", "3.14",
					"/numbers", "1",
					"--numbers", "2",
					"/Numbers", "3",
				});

			Assert.IsType<ParserSuccess<OptionalEnumerableArguments>>(result);
			Assert.Equal(3.14, result.Value.First);
			Assert.NotNull(result.Value.Numbers);
			Assert.Equal(3, result.Value.Numbers.Count());
			Assert.Equal(1, result.Value.Numbers.First());
			Assert.Equal(2, result.Value.Numbers.Skip(1).First());
			Assert.Equal(3, result.Value.Numbers.Skip(2).First());
		}
	}
}
