using System.Collections.Generic;
using System.Linq;
using Clarg;
using Xunit;

namespace Tests
{
	public class DescriptionDecoration
	{
		const string StringConstructor = nameof(StringConstructor);
		const string OptionalConstructor = nameof(OptionalConstructor);
		const string ParamsConstructor = nameof(ParamsConstructor);

		const string StringParameter = nameof(StringParameter);
		const string EnumerableIntParameter = nameof(EnumerableIntParameter);
		const string IntParameter = nameof(IntParameter);
		const string BoolParameter = nameof(BoolParameter);
		const string ParamsParameter = nameof(ParamsParameter);

		class DecoratedArguments
		{
			[ArgumentDescription(StringConstructor)]
			public DecoratedArguments(
				[ArgumentDescription(StringParameter)] string one)
			{ }

			[ArgumentDescription(OptionalConstructor)]
			public DecoratedArguments(
				[ArgumentDescription(EnumerableIntParameter)] IEnumerable<int> two,
				[ArgumentDescription(BoolParameter)] bool three = true)
			{ }

			[ArgumentDescription(ParamsConstructor)]
			public DecoratedArguments(
				[ArgumentDescription(StringParameter)] string one,
				[ArgumentDescription(IntParameter)] int two,
				[ArgumentDescription(ParamsParameter)] params KeyValuePair<string, string>[] remainder)
			{ }
		}

		[Fact]
		public void Should_Return_Constructor_And_Argument_Description()
		{
			var parser = new Parser();

			var result = parser.Create<DecoratedArguments>("-", new string[0]);

			Assert.IsType<ParserSuggestions<DecoratedArguments>>(result);

			var stringSuggestion = result
				.Suggestions
				.Where(suggestion => suggestion.Arguments.Count() == 1)
				.Single();

			Assert.Equal(StringConstructor, stringSuggestion.Description);

			var stringArgument = stringSuggestion
				.Arguments
				.ElementAt(0);

			Assert.Equal("one", stringArgument.Name);
			Assert.Equal(StringParameter, stringArgument.Description);
			Assert.Equal(false, stringArgument.IsEnumerable);
			Assert.Equal(false, stringArgument.IsOptional);
			Assert.Equal(false, stringArgument.IsParams);
		}

		[Fact]
		public void Should_Indicate_Optional_Arguments()
		{
			var parser = new Parser();

			var result = parser.Create<DecoratedArguments>("-", new string[0]);

			Assert.IsType<ParserSuggestions<DecoratedArguments>>(result);

			var optionalSuggestion = result
				.Suggestions
				.Where(suggestion => suggestion.Arguments.Count() == 2)
				.Single();

			Assert.Equal(OptionalConstructor, optionalSuggestion.Description);

			var optionalArgument = optionalSuggestion
				.Arguments
				.ElementAt(1);

			Assert.Equal("three", optionalArgument.Name);
			Assert.Equal(BoolParameter, optionalArgument.Description);
			Assert.Equal(false, optionalArgument.IsEnumerable);
			Assert.Equal(true, optionalArgument.IsOptional);
			Assert.Equal(false, optionalArgument.IsParams);
		}

		[Fact]
		public void Should_Indicate_Enumerable_Arguments()
		{
			var parser = new Parser();

			var result = parser.Create<DecoratedArguments>("-", new string[0]);

			Assert.IsType<ParserSuggestions<DecoratedArguments>>(result);

			var optionalSuggestion = result
				.Suggestions
				.Where(suggestion => suggestion.Arguments.Count() == 2)
				.Single();

			Assert.Equal(OptionalConstructor, optionalSuggestion.Description);

			var enumerableArgument = optionalSuggestion
				.Arguments
				.ElementAt(0);

			Assert.Equal("two", enumerableArgument.Name);
			Assert.Equal(EnumerableIntParameter, enumerableArgument.Description);
			Assert.Equal(true, enumerableArgument.IsEnumerable);
			Assert.Equal(false, enumerableArgument.IsOptional);
			Assert.Equal(false, enumerableArgument.IsParams);
		}

		[Fact]
		public void Should_Indicate_Params_Arguments()
		{
			var parser = new Parser();

			var result = parser.Create<DecoratedArguments>("-", new string[0]);

			Assert.IsType<ParserSuggestions<DecoratedArguments>>(result);

			var paramsSuggestion = result
				.Suggestions
				.Where(suggestion => suggestion.Arguments.Count() == 3)
				.Single();

			Assert.Equal(ParamsConstructor, paramsSuggestion.Description);

			var paramsArgument = paramsSuggestion
				.Arguments
				.ElementAt(2);

			Assert.Equal("remainder", paramsArgument.Name);
			Assert.Equal(ParamsParameter, paramsArgument.Description);
			Assert.Equal(false, paramsArgument.IsEnumerable);
			Assert.Equal(false, paramsArgument.IsOptional);
			Assert.Equal(true, paramsArgument.IsParams);
		}
	}
}
