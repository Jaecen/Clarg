using System;
using System.Collections.Generic;
using System.Linq;

namespace Clarg
{
	// Assembles a stream of command-line arguments into key/value pairs
	class Tokenizer
	{
		// Recursively parse the arguments into KVP's
		public TokenizerResult Tokenize(string argumentPrefix, string[] args)
		{
			try
			{
				var descriptors = ParseArguments(
					argumentPrefix,
					args.Select(s => s?.Trim() ?? string.Empty));

				return new TokenizerSuccess(descriptors);
			}
			catch(Exception exception)
			{
				return new TokenizerError(exception);
			}
		}

		IEnumerable<ArgumentDescriptor> ParseArguments(string argumentPrefix, IEnumerable<string> args)
		{
			if(!args.Any())
				return Enumerable.Empty<ArgumentDescriptor>();

			var arg = args.First();

			if(!arg.StartsWith(argumentPrefix))
				throw new Exception("Argument provided without a name");

			return ParseArguments(
				argumentPrefix,
				args.Skip(1),
				arg.Substring(argumentPrefix.Length));
		}

		IEnumerable<ArgumentDescriptor> ParseArguments(string argumentPrefix, IEnumerable<string> args, string argumentName)
		{
			var arg = args.FirstOrDefault();

			if(!args.Any())
				throw new Exception("Argument provided without a value");

			return new[] { new ArgumentDescriptor(argumentName, arg) }
				.Concat(ParseArguments(argumentPrefix, args.Skip(1)));
		}
	}
}
