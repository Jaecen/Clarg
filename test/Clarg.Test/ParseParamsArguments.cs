using System.Collections.Generic;
using Clarg;
using Xunit;

namespace Tests
{
	public class ParseParamsArguments
	{
		class StringParamsArguments
		{
			public readonly string One;
			public readonly int Two;
			public readonly string[] Others;

			public StringParamsArguments(string one, int two, params string[] others)
			{
				One = one;
				Two = two;
				Others = others;
			}
		}

		class DictionaryParamsArguments
		{
			public readonly string One;
			public readonly int Two;
			public readonly KeyValuePair<string, string>[] Others;

			public DictionaryParamsArguments(string one, int two, params KeyValuePair<string, string>[] others)
			{
				One = one;
				Two = two;
				Others = others;
			}
		}

		[Fact]
		public void HandleStringParamsArgumentsWithNoValue()
		{
			var parser = new Parser();

			var result = parser.Create<StringParamsArguments>(
				"-",
				new[]
				{
					"-one", "first",
					"-two", "2",
				});

			Assert.IsType<ParserSuccess<StringParamsArguments>>(result);
			Assert.Equal("first", result.Value.One);
			Assert.Equal(2, result.Value.Two);
			Assert.Equal(0, result.Value.Others.Length);
		}

		[Fact]
		public void HandleStringParamsArgumentsWithMultipleValues()
		{
			var parser = new Parser();

			var result = parser.Create<StringParamsArguments>(
				"-",
				new[]
				{
					"-one", "first",
					"-two", "2",
					"-three", "working",
					"-four", "hopefully",
				});

			Assert.IsType<ParserSuccess<StringParamsArguments>>(result);
			Assert.Equal("first", result.Value.One);
			Assert.Equal(2, result.Value.Two);
			Assert.Equal(4, result.Value.Others.Length);
			Assert.Equal("three", result.Value.Others[0]);
			Assert.Equal("working", result.Value.Others[1]);
			Assert.Equal("four", result.Value.Others[2]);
			Assert.Equal("hopefully", result.Value.Others[3]);
		}

		[Fact]
		public void HandleDictionaryParamsArgumentsWithNoValue()
		{
			var parser = new Parser();

			var result = parser.Create<DictionaryParamsArguments>(
				"-",
				new[]
				{
					"-one", "first",
					"-two", "2",
				});

			Assert.IsType<ParserSuccess<DictionaryParamsArguments>>(result);
			Assert.Equal("first", result.Value.One);
			Assert.Equal(2, result.Value.Two);
			Assert.Equal(0, result.Value.Others.Length);
		}

		[Fact]
		public void HandleDictionaryParamsArgumentsWithMultipleValues()
		{
			var parser = new Parser();

			var result = parser.Create<DictionaryParamsArguments>(
				"-",
				new[]
				{
					"-one", "first",
					"-two", "2",
					"-three", "working",
					"-four", "hopefully",
				});

			Assert.IsType<ParserSuccess<DictionaryParamsArguments>>(result);
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
