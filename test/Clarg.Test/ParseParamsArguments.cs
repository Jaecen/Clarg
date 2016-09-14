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

			var result = parser.Create<ParamsArguments>(
				"-",
				new[]
				{
					"-one", "first",
					"-two", "2",
				});

			Assert.IsType<ParserSuccess<ParamsArguments>>(result);
			Assert.Equal("first", result.Value.One);
			Assert.Equal(2, result.Value.Two);
			Assert.Equal(0, result.Value.Others.Length);
		}

		[Fact]
		public void HandleParamsArgumentsWithMultipleValues()
		{
			var parser = new Parser();

			var result = parser.Create<ParamsArguments>(
				"-",
				new[]
				{
					"-one", "first",
					"-two", "2",
					"-three", "working",
					"-four", "hopefully",
				});

			Assert.IsType<ParserSuccess<ParamsArguments>>(result);
			Assert.Equal("first", result.Value.One);
			Assert.Equal(2, result.Value.Two);
			Assert.Equal(2, result.Value.Others.Length);
			Assert.Equal("three", result.Value.Others[0].Key);
			Assert.Equal("working", result.Value.Others[0].Value);
			Assert.Equal("four", result.Value.Others[1].Key);
			Assert.Equal("hopefully", result.Value.Others[1].Value);
		}
	}
}
