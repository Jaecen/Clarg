using System.Collections.Generic;
using Clarg;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class ParserTests
	{
		[TestMethod]
		public void HandleExactArguments()
		{
			var parser = new Parser();

			var arguments = parser.Create<ExactArguments>(new[] { "--one", "first", "/two", "2" });

			Assert.AreEqual("first", arguments.One);
			Assert.AreEqual(2, arguments.Two);
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
	}

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
}
