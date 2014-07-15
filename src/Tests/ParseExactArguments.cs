using Clarg;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class ParseExactArguments
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
	}
}
