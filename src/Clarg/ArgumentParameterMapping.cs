﻿using System;
using System.Diagnostics;

namespace Clarg
{
	[DebuggerDisplay("{Argument} -> {Parameter}")]
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
			if(Object.ReferenceEquals(obj, null))
				return false;

			if(Object.ReferenceEquals(obj, this))
				return true;

			if(obj is ArgumentParameterMapping)
				return Equals((ArgumentParameterMapping)obj);

			return base.Equals(obj);
		}

		public bool Equals(ArgumentParameterMapping obj)
		{
			if(Object.ReferenceEquals(obj, null))
				return false;

			if(Object.ReferenceEquals(obj, this))
				return true;

			return Argument == obj.Argument && Parameter == obj.Parameter;
		}

		public override int GetHashCode()
		{
			return
				(Argument == null ? Int32.MinValue : Argument.GetHashCode())
				^ (Parameter == null ? Int32.MinValue : Parameter.GetHashCode());
		}

		public static bool operator ==(ArgumentParameterMapping x, ArgumentParameterMapping y)
		{
			if(Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
				return Object.ReferenceEquals(x, null) && Object.ReferenceEquals(y, null);

			if(Object.ReferenceEquals(x, y))
				return true;

			return x.Equals(y);
		}

		public static bool operator !=(ArgumentParameterMapping x, ArgumentParameterMapping y)
		{
			return !(x == y);
		}
	}
}