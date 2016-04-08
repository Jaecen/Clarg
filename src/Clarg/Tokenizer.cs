using System;
using System.Collections.Generic;
using System.Linq;

namespace Clarg
{
	// Assembles a stream of command-line arguments into key/value pairs
	class Tokenizer
	{
		public IEnumerable<ArgumentDescriptor> Tokenize(string[] args)
		{
			// Recursively parse the arguments into KVP's
			return ParseArguments(
					args.Select(s => (s ?? String.Empty).Trim())                // Help ensure good data going in
				);
		}

		IEnumerable<ArgumentDescriptor> ParseArguments(IEnumerable<string> args)
		{
			if(!args.Any())
				return Enumerable.Empty<ArgumentDescriptor>();

			var arg = args.First();

			if(arg.StartsWith("--"))
				return ParseArguments(args.Skip(1), arg.Substring(2));
			else if(arg.StartsWith("/"))
				return ParseArguments(args.Skip(1), arg.Substring(1));
			else
				throw new Exception("Argument provided without a name.");
		}

		IEnumerable<ArgumentDescriptor> ParseArguments(IEnumerable<string> args, string argumentName)
		{
			var arg = args.FirstOrDefault();

			if(!args.Any() || (arg.StartsWith("--") || arg.StartsWith("/")))
				return new[] { new ArgumentDescriptor(argumentName, Boolean.TrueString) }
					.Concat(ParseArguments(args));                                  // Don't advance args, as there was no value
			else
				return new[] { new ArgumentDescriptor(argumentName, arg) }
					.Concat(ParseArguments(args.Skip(1)));
		}
	}
}
