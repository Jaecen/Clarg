using System.Collections.Generic;
using Clarg;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class ParserTests
	{
		class ExactArguments
		{
			public readonly string One;
			public readonly int Two;

			public ExactArguments(string one, int two)
			{
				One = one;
				Two = two;
			}
		}

		[TestMethod]
		public void HandleExactArguments()
		{
			var parser = new Parser();

			var arguments = parser.Create<ExactArguments>(new[] { "--one", "first", "/two", "2" });

			Assert.AreEqual("first", arguments.One);
			Assert.AreEqual(2, arguments.Two);
		}

		class BooleanArguments
		{
			public readonly bool One;
			public readonly bool Two;
			public readonly bool Three;

			public BooleanArguments(bool one, bool two, bool three)
			{
				One = one;
				Two = two;
				Three = three;
			}
		}

		[TestMethod]
		public void HandleMultipleBooleanArgumentsWithoutValues()
		{
			var parser = new Parser();

			var arguments = parser.Create<BooleanArguments>(new[] { "--one", "/two", "--three" });

			Assert.IsTrue(arguments.One);
			Assert.IsTrue(arguments.Two);
			Assert.IsTrue(arguments.Three);
		}

		class OptionalArguments
		{
			public readonly string One;
			public readonly int Two;
			public readonly bool? Three;

			public OptionalArguments(string one, int two, bool? three = null)
			{
				One = one;
				Two = two;
				Three = three;
			}
		}
		
		[TestMethod]
		public void HandleOptionalArgumentsWithNoValue()
		{
			var parser = new Parser();

			var arguments = parser.Create<OptionalArguments>(new[] { "--one", "first", "/two", "2", });

			Assert.AreEqual("first", arguments.One);
			Assert.AreEqual(2, arguments.Two);
			Assert.AreEqual(null, arguments.Three);
		}

		[TestMethod]
		public void HandleOptionalArgumentsWithValue()
		{
			var parser = new Parser();

			var arguments = parser.Create<OptionalArguments>(new[] { "--one", "first", "/two", "2", "--three", "false" });

			Assert.AreEqual("first", arguments.One);
			Assert.AreEqual(2, arguments.Two);
			Assert.AreEqual(false, arguments.Three);
		}

		[TestMethod]
		public void HandleOptionalArgumentsWithPresentAsBool()
		{
			var parser = new Parser();

			var arguments = parser.Create<OptionalArguments>(new[] { "--one", "first", "/two", "2", "--three" });

			Assert.AreEqual("first", arguments.One);
			Assert.AreEqual(2, arguments.Two);
			Assert.AreEqual(true, arguments.Three);
		}

		class ParamsArguments
		{
			public readonly string One;
			public readonly int Two;
			public readonly KeyValuePair<string, string>[] Others;

			public ParamsArguments(string one, int two, params KeyValuePair<string, string>[] others)
			{
				One = one;
				Two = two;
				Others = others;
			}
		}
		
		[TestMethod]
		public void HandleParamsArgumentsWithNoValue()
		{
			var parser = new Parser();

			var arguments = parser.Create<ParamsArguments>(new[] { "--one", "first", "/two", "2", });

			Assert.AreEqual("first", arguments.One);
			Assert.AreEqual(2, arguments.Two);
			Assert.AreEqual(0, arguments.Others.Length);
		}

		[TestMethod]
		public void HandleParamsArgumentsWithMultipleValues()
		{
			var parser = new Parser();

			var arguments = parser.Create<ParamsArguments>(new[] { "--one", "first", "/two", "2", "--three", "working", "/four", "hopefully" });

			Assert.AreEqual("first", arguments.One);
			Assert.AreEqual(2, arguments.Two);
			Assert.AreEqual(2, arguments.Others.Length);
			Assert.AreEqual("three", arguments.Others[0].Key);
			Assert.AreEqual("working", arguments.Others[0].Value);
			Assert.AreEqual("four", arguments.Others[1].Key);
			Assert.AreEqual("hopefully", arguments.Others[1].Value);
		}

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
