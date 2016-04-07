using System.Collections.Generic;
using Clarg;
using Xunit;

namespace Tests
{
	public class ParseCaseInsensitiveArguments
	{
		class CaseInsensitiveArguments
		{
			public readonly string FirstArgument;
			public readonly int? SecondArgument;
			public readonly KeyValuePair<string, string>[] FurtherArguments;

			public CaseInsensitiveArguments(string firstArgument = null, int? secondArgument = null)
			{
				FirstArgument = firstArgument;
				SecondArgument = secondArgument;
			}

			public CaseInsensitiveArguments(string firstArgument, int secondArgument, params KeyValuePair<string, string>[] furtherArguments)
				: this(firstArgument, (int?)secondArgument)
			{
				FurtherArguments = furtherArguments;
			}
		}

		[Fact]
		public void HandlesCaseDifferences()
		{
			var parser = new Parser();

			var arguments = parser.Create<CaseInsensitiveArguments>(new[] {
					"--FIRSTargument", "first",
					"/secondargument", "2",
				});

			Assert.Equal("first", arguments.FirstArgument);
			Assert.Equal(2, arguments.SecondArgument);
			Assert.Null(arguments.FurtherArguments);
		}

		[Fact]
		public void HandlesCaseDifferencesWithOptionalArgs()
		{
			var parser = new Parser();

			var arguments = parser.Create<CaseInsensitiveArguments>(new[] {
					"/secondargument", "2",
				});

			Assert.Equal(null, arguments.FirstArgument);
			Assert.Equal(2, arguments.SecondArgument);
			Assert.Null(arguments.FurtherArguments);
		}

		[Fact]
		public void HandlesCaseDifferencesWithParamsArg()
		{
			var parser = new Parser();

			var arguments = parser.Create<CaseInsensitiveArguments>(new[] {
					"--FIRSTargument", "first",
					"/secondargument", "2",
					"/some", "other",
					"--argu", "ments",
				});

			Assert.Equal("first", arguments.FirstArgument);
			Assert.Equal(2, arguments.SecondArgument);
			Assert.Equal(2, arguments.FurtherArguments.Length);
		}
	}
}
