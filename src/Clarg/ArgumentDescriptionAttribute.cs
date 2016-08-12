using System;

namespace Clarg
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Constructor, Inherited = false, AllowMultiple = false)]
	public sealed class ArgumentDescriptionAttribute : Attribute
	{
		public readonly string Description;

		public ArgumentDescriptionAttribute(string description)
		{
			Description = description;
		}
	}
}
