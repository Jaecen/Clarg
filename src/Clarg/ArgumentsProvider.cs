using System;

namespace Clarg
{
	public class ArgumentsProvider
	{
		public static TArguments Parse<TArguments>(string[] args, string argumentPrefix = "-")
			where TArguments : class
		{
			var parserResult = new Parser().Create<TArguments>(argumentPrefix, args);
			return parserResult.Value;
		}

		public static void WriteErrorToConsole<TArguments>(string[] args, string argumentPrefix = "-")
			where TArguments : class
		{
			var parserResult = new Parser().Create<TArguments>(argumentPrefix, args);
			if(parserResult.Value != null)
				return;

			ConsoleString errorMessage;
			if(parserResult.Error != null)
			{
				errorMessage = new ConsoleString("There was an error parsing arguments:" + Environment.NewLine + parserResult.Error.Message);
			}
			else
			{
				errorMessage = new ParserSuggestionFormatter()
					.CreateErrorMessage(
						Environment.GetCommandLineArgs()[0],
						argumentPrefix,
						parserResult.Suggestions);
			}

			new ConsoleStringRenderer()
				.WriteLine(errorMessage);
		}
	}
}
