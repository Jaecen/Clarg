using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clarg
{
	public class ParserSuggestionFormatter
	{
		const string ArgumentPrefix = "--";

		public string CreateErrorMessage(string command, IEnumerable<ParserSuggestion> suggestions)
		{
			var stringBuilder = new StringBuilder();

			foreach(var suggestion in suggestions)
			{
				stringBuilder.AppendLine(BuildSyntax(command, suggestion));
				if(!string.IsNullOrWhiteSpace(suggestion.Description))
					stringBuilder.AppendLine(suggestion.Description);

				stringBuilder.AppendLine();
			}

			stringBuilder.AppendLine(BuildDetail(suggestions));

			return stringBuilder.ToString();
		}

		FormattedArgument FormatArgument(ParserSuggestionArgument argument)
			=> new FormattedArgument(
				argument: argument,
				displayName: argument.IsParams
					? "[...]"
					: $"{ArgumentPrefix}{argument.Name}",
				displayType: argument.IsParams || argument.IsEnumerable
					? $"<{argument.InnerType.Name}>..."
					: $"<{argument.Type.Name}>");

		string BuildSyntax(string command, ParserSuggestion suggestion)
		{
			var argumentSyntaxes = suggestion
				.Arguments
				.Select(FormatArgument)
				.Select(BuildArgumentSyntax);

			var argumentSyntaxList = string.Join(" ", argumentSyntaxes);

			return $"Usage: {command} {argumentSyntaxList}";
		}

		string BuildArgumentSyntax(FormattedArgument formattedArgument)
			=> formattedArgument.Argument.IsParams
				? formattedArgument.DisplayName
				: formattedArgument.Argument.IsOptional
					? $"[{formattedArgument.DisplayName} {formattedArgument.DisplayType}]"
					: $"{formattedArgument.DisplayName} {formattedArgument.DisplayType}";

		string BuildDetail(IEnumerable<ParserSuggestion> suggestions)
		{
			var formattedArguments = suggestions
				.SelectMany(suggestion => suggestion.Arguments)
				.Distinct()
				.OrderBy(argument => argument.IsParams)
				.ThenBy(argument => argument.Name)
				.Select(FormatArgument);

			var maxArgumentNameLength = formattedArguments.Max(argument => argument.DisplayName.Length);
			var maxArgumentTypeLength = formattedArguments.Max(argument => argument.DisplayType.Length);

			var stringBuilder = new StringBuilder();

			stringBuilder.AppendLine("Options:");
			foreach(var argument in formattedArguments)
				stringBuilder.AppendLine(BuildArgumentDetail(argument, maxArgumentNameLength, maxArgumentTypeLength));

			return stringBuilder.ToString();
		}

		string BuildArgumentDetail(FormattedArgument formattedArgument, int maxArgumentNameLength, int maxArgumentTypeLength)
			=> $"  {formattedArgument.DisplayName.PadRight(maxArgumentNameLength)} {formattedArgument.DisplayType.PadRight(maxArgumentTypeLength)} {formattedArgument.Argument.Description}";
	}
}
