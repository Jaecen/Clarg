using System.Collections.Generic;
using Clarg;
using Xunit;

namespace Tests
{
	public class ParseParamsArguments
	{
		class ParamsArguments
		{
			public readonly string One;
			public readonly int Two;
			public readonly KeyValuePair<string, string>[] Others;

			public ParamsArguments(string one, int two, params KeyValuePair<string, string>[] others)
			{
				One = one;
				Two = two;
				Others = others;
			}
		}

		[Fact]
		public void HandleParamsArgumentsWithNoValue()
		{
			var parser = new Parser();

			var arguments = parser.Create<ParamsArguments>(new[] {
				"--one", "first",
				"/two", "2",
			});

			Assert.Equal("first", arguments.One);
			Assert.Equal(2, arguments.Two);
			Assert.Equal(0, arguments.Others.Length);
		}

		[Fact]
		public void HandleParamsArgumentsWithMultipleValues()
		{
			var parser = new Parser();

			var arguments = parser.Create<ParamsArguments>(new[] {
				"--one", "first",
				"/two", "2",
				"--three", "working",
				"/four", "hopefully",
			});

			Assert.Equal("first", arguments.One);
			Assert.Equal(2, arguments.Two);
			Assert.Equal(2, arguments.Others.Length);
			Assert.Equal("three", arguments.Others[0].Key);
			Assert.Equal("working", arguments.Others[0].Value);
			Assert.Equal("four", arguments.Others[1].Key);
			Assert.Equal("hopefully", arguments.Others[1].Value);
		}
	}
}
