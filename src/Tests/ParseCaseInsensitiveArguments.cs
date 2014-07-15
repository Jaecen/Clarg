using System.Collections.Generic;
using Clarg;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
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

		[TestMethod]
		public void HandlesCaseDifferences()
		{
			var parser = new Parser();

			var arguments = parser.Create<CaseInsensitiveArguments>(new[] { 
					"--FIRSTargument", "first", 
					"/secondargument", "2", 
				});

			Assert.AreEqual("first", arguments.FirstArgument);
			Assert.AreEqual(2, arguments.SecondArgument);
			Assert.IsNull(arguments.FurtherArguments);
		}

		[TestMethod]
		public void HandlesCaseDifferencesWithOptionalArgs()
		{
			var parser = new Parser();

			var arguments = parser.Create<CaseInsensitiveArguments>(new[] { 
					"/secondargument", "2", 
				});

			Assert.AreEqual(null, arguments.FirstArgument);
			Assert.AreEqual(2, arguments.SecondArgument);
			Assert.IsNull(arguments.FurtherArguments);
		}

		[TestMethod]
		public void HandlesCaseDifferencesWithParamsArg()
		{
			var parser = new Parser();

			var arguments = parser.Create<CaseInsensitiveArguments>(new[] { 
					"--FIRSTargument", "first", 
					"/secondargument", "2", 
					"/some", "other",
					"--argu", "ments",
				});

			Assert.AreEqual("first", arguments.FirstArgument);
			Assert.AreEqual(2, arguments.SecondArgument);
			Assert.AreEqual(2, arguments.FurtherArguments.Length);
		}
	}
}
