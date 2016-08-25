using System;

namespace Clarg
{
	public class ArgumentsProvider
	{
		public static TArguments Parse<TArguments>(string[] args)
			where TArguments : class
		{
			var parserResult = new Parser().Create<TArguments>(args);
			return parserResult.Value;
		}

		public static void WriteErrorToConsole<TArguments>(string[] args)
			where TArguments : class
		{
			var parserResult = new Parser().Create<TArguments>(args);

			if(parserResult.Value != null)
				return;

			var errorMessage = new ParserSuggestionFormatter()
				.CreateErrorMessage(
					Environment.GetCommandLineArgs()[0],
					parserResult.Suggestions);

			new ConsoleStringRenderer()
				.WriteLine(errorMessage);
		}
	}
}
