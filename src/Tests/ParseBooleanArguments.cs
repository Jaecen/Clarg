using Clarg;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class ParseBooleanArguments
	{
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
	}
}
