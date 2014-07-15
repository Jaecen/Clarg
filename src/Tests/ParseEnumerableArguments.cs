using System.Collections.Generic;
using System.Linq;
using Clarg;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
	[TestClass]
	public class ParseEnumerableArguments
	{
		class EnumerableArguments
		{
			public IEnumerable<int> Numbers;

			public EnumerableArguments(IEnumerable<int> numbers)
			{
				Numbers = numbers;
			}
		}

		[TestMethod]
		public void HandlesEnumerableParametersWithASingleValue()
		{
			var parser = new Parser();

			var arguments = parser.Create<EnumerableArguments>(new[] { 
					"/numbers", "1",
				});

			Assert.IsNotNull(arguments.Numbers);
			Assert.AreEqual(1, arguments.Numbers.Count());
			Assert.AreEqual(1, arguments.Numbers.First());
		}

		[TestMethod]
		public void HandlesEnumerableParametersWithMultipleValues()
		{
			var parser = new Parser();

			var arguments = parser.Create<EnumerableArguments>(new[] { 
					"/numbers", "1",
					"--numbers", "2",
					"/Numbers", "3",
				});

			Assert.IsNotNull(arguments.Numbers);
			Assert.AreEqual(3, arguments.Numbers.Count());
			Assert.AreEqual(1, arguments.Numbers.First());
			Assert.AreEqual(2, arguments.Numbers.Skip(1).First());
			Assert.AreEqual(3, arguments.Numbers.Skip(2).First());
		}

		class OptionalEnumerableArguments
		{
			public double First;
			public IEnumerable<int> Numbers;

			public OptionalEnumerableArguments(double first, IEnumerable<int> numbers = null)
			{
				First = first;
				Numbers = numbers;
			}
		}

		[TestMethod]
		public void HandlesOptionalEnumerableParametersWithNoValues()
		{
			var parser = new Parser();

			var arguments = parser.Create<OptionalEnumerableArguments>(new[] { 
					"/first", "3.14",
				});

			Assert.AreEqual(3.14, arguments.First);
			Assert.IsNull(arguments.Numbers);
		}

		[TestMethod]
		public void HandlesOptionalEnumerableParametersWithASingleValue()
		{
			var parser = new Parser();

			var arguments = parser.Create<OptionalEnumerableArguments>(new[] { 
					"/first", "3.14",
					"/numbers", "1",
				});

			Assert.AreEqual(3.14, arguments.First);
			Assert.IsNotNull(arguments.Numbers);
			Assert.AreEqual(1, arguments.Numbers.Count());
			Assert.AreEqual(1, arguments.Numbers.First());
		}

		[TestMethod]
		public void HandlesOptionalEnumerableParametersWithMultipleValues()
		{
			var parser = new Parser();

			var arguments = parser.Create<OptionalEnumerableArguments>(new[] { 
					"/first", "3.14",
					"/numbers", "1",
					"--numbers", "2",
					"/Numbers", "3",
				});

			Assert.AreEqual(3.14, arguments.First);
			Assert.IsNotNull(arguments.Numbers);
			Assert.AreEqual(3, arguments.Numbers.Count());
			Assert.AreEqual(1, arguments.Numbers.First());
			Assert.AreEqual(2, arguments.Numbers.Skip(1).First());
			Assert.AreEqual(3, arguments.Numbers.Skip(2).First());
		}
	}
}
