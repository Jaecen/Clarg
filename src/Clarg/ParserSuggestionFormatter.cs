using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Clarg
{
	public class ParserSuggestionFormatter
	{
		// This class outputs parser suggestion as a formatted string to be displayed to humans.
		// It has two sections: syntax and detail.

		// The syntax section shows all of the possible combinations of arguments with the names of each
		// parameter and a description of what that combination does.

		// The detail section shows a description of each parameter.

		public ConsoleString CreateErrorMessage(string command, string argumentPrefix, IEnumerable<ParserSuggestion> suggestions)
		{
			var invokedName = Path.GetFileName(command);

			var errorMessage = new ConsoleString();
			errorMessage += "Usage:";
			errorMessage += Environment.NewLine;

			foreach(var suggestion in suggestions)
			{
				errorMessage += BuildSyntax(invokedName, argumentPrefix, suggestion);
				errorMessage += Environment.NewLine;

				if(!string.IsNullOrWhiteSpace(suggestion.Description))
				{
					errorMessage += suggestion.Description;
					errorMessage += Environment.NewLine;
				}

				errorMessage += Environment.NewLine;
			}

			errorMessage += BuildDetail(argumentPrefix, suggestions);

			return errorMessage;
		}

		FormattedArgument FormatArgument(string argumentPrefix, ParserSuggestionArgument argument)
			=> new FormattedArgument(
				argument: argument,
				displayName:
					argument.IsParams
						? "[...]"
						: $"{argumentPrefix}{argument.Name}",
				displayType: argument.IsParams || argument.IsEnumerable
					? $"<{argument.InnerType.Name}>..."
					: $"<{argument.Type.Name}>");

		ConsoleString BuildSyntax(string invokedName, string argumentPrefix, ParserSuggestion suggestion)
		{
			var argumentSyntaxes = suggestion
				.Arguments
				.Select(argument => FormatArgument(argumentPrefix, argument))
				.Select(BuildArgumentSyntax);

			var argumentSyntaxList = ConsoleString.Join(" ", argumentSyntaxes);
			return invokedName + " " + argumentSyntaxList;
		}

		ConsoleString BuildArgumentSyntax(FormattedArgument formattedArgument)
		{
			var argumentSyntax = new ConsoleString();

			if(formattedArgument.Argument.IsParams)
				argumentSyntax += formattedArgument.DisplayName;
			else if(formattedArgument.Argument.IsOptional)
				argumentSyntax += "[" + formattedArgument.DisplayName + formattedArgument.DisplayType + "]";
			else
				argumentSyntax += formattedArgument.DisplayName + " " + formattedArgument.DisplayType;

			if(formattedArgument.Argument.IsFulfilled == false)
				argumentSyntax = ("!" + argumentSyntax + "!").Colored(foreground: ConsoleColor.Red);

			return argumentSyntax;
		}

		ConsoleString BuildDetail(string argumentPrefix, IEnumerable<ParserSuggestion> suggestions)
		{
			var formattedArguments = suggestions
				.SelectMany(suggestion => suggestion.Arguments)
				.Distinct()
				.OrderBy(argument => argument.IsParams)
				.ThenBy(argument => argument.Name)
				.Select(argument => FormatArgument(argumentPrefix, argument));

			var maxArgumentNameLength = formattedArguments.Max(argument => argument.DisplayName.GetLength());
			var maxArgumentTypeLength = formattedArguments.Max(argument => argument.DisplayType.GetLength());

			var detail = new ConsoleString();

			detail += "Options:";
			detail += Environment.NewLine;

			foreach(var argument in formattedArguments)
			{
				detail += BuildArgumentDetail(argument, maxArgumentNameLength, maxArgumentTypeLength);
				detail += Environment.NewLine;
			}

			return detail;
		}

		ConsoleString BuildArgumentDetail(FormattedArgument formattedArgument, int maxArgumentNameLength, int maxArgumentTypeLength)
		{
			var argumentDetail = new ConsoleString();

			argumentDetail += "  ";
			argumentDetail += formattedArgument
				.DisplayName
				.PadRight(maxArgumentNameLength);
			argumentDetail += " ";
			argumentDetail += formattedArgument
				.DisplayType
				.PadRight(maxArgumentTypeLength);
			argumentDetail += " ";
			argumentDetail += formattedArgument.Argument.Description;

			if(formattedArgument.Argument.IsFulfilled == false)
				argumentDetail = argumentDetail.Colored(foreground: ConsoleColor.Red);

			return argumentDetail;
		}
	}
}
