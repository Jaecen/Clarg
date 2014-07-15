using Clarg;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class ParseOptionalArguments
	{
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
	}
}
