using System;
using System.Diagnostics;

namespace Clarg
{
	[DebuggerDisplay("{Name}: {Value}")]
	class ArgumentDescriptor
	{
		public readonly string Name;
		public readonly string Value;

		public ArgumentDescriptor(string name, string value)
		{
			Name = name;
			Value = value;
		}

		public override bool Equals(object obj)
		{
			if(Object.ReferenceEquals(obj, null))
				return false;

			if(Object.ReferenceEquals(obj, this))
				return true;

			if(obj is ArgumentDescriptor)
				return Equals((ArgumentDescriptor)obj);

			return base.Equals(obj);
		}

		public bool Equals(ArgumentDescriptor obj)
		{
			if(Object.ReferenceEquals(obj, null))
				return false;

			if(Object.ReferenceEquals(obj, this))
				return true;

			return StringComparer.OrdinalIgnoreCase.Equals(obj.Name, Name) && StringComparer.OrdinalIgnoreCase.Equals(obj.Value, Value);
		}

		public override int GetHashCode()
		{
			return StringComparer.OrdinalIgnoreCase.GetHashCode(Name) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
		}

		public static bool operator ==(ArgumentDescriptor x, ArgumentDescriptor y)
		{
			if(Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
				return Object.ReferenceEquals(x, null) && Object.ReferenceEquals(y, null);

			if(Object.ReferenceEquals(x, y))
				return true;

			return x.Equals(y);
		}

		public static bool operator !=(ArgumentDescriptor x, ArgumentDescriptor y)
		{
			return !(x == y);
		}
	}
}
