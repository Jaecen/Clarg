using System.Diagnostics;

namespace Clarg
{
	[DebuggerDisplay("arg: {Argument?.Name} -> param: {Parameter?.Name}")]
	class ArgumentParameterMapping
	{
		public readonly ArgumentDescriptor Argument;
		public readonly ParameterDescriptor Parameter;

		public ArgumentParameterMapping(ArgumentDescriptor argument, ParameterDescriptor parameter)
		{
			Argument = argument;
			Parameter = parameter;
		}

		public override bool Equals(object obj)
		{
			if(ReferenceEquals(obj, null))
				return false;

			if(ReferenceEquals(obj, this))
				return true;

			if(obj is ArgumentParameterMapping)
				return Equals((ArgumentParameterMapping)obj);

			return base.Equals(obj);
		}

		public bool Equals(ArgumentParameterMapping obj)
		{
			if(ReferenceEquals(obj, null))
				return false;

			if(ReferenceEquals(obj, this))
				return true;

			return Argument == obj.Argument && Parameter == obj.Parameter;
		}

		public override int GetHashCode()
			=> (Argument == null ? int.MinValue : Argument.GetHashCode())
				^ (Parameter == null ? int.MinValue : Parameter.GetHashCode());

		public static bool operator ==(ArgumentParameterMapping x, ArgumentParameterMapping y)
		{
			if(ReferenceEquals(x, null) || ReferenceEquals(y, null))
				return ReferenceEquals(x, null) && ReferenceEquals(y, null);

			if(ReferenceEquals(x, y))
				return true;

			return x.Equals(y);
		}

		public static bool operator !=(ArgumentParameterMapping x, ArgumentParameterMapping y)
			=> !(x == y);
	}
}
