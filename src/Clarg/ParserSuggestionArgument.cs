using System;
using System.Linq;
using System.Reflection;

namespace Clarg
{
	public class ParserSuggestionArgument
	{
		const int HashCodeSeed = 0x0229175F;
		const int HashCodeFactor = 0x213C0725;

		public readonly string Name;
		public readonly string Description;
		public readonly Type Type;
		public readonly bool IsEnumerable;
		public readonly bool IsOptional;
		public readonly bool IsParams;
		public readonly Type InnerType;

		public ParserSuggestionArgument(string name, string description, Type type, bool isEnumerable, bool isOptional, bool isParams)
		{
			Name = name;
			Description = description;
			Type = type;
			IsEnumerable = isEnumerable;
			IsOptional = isOptional;
			IsParams = isParams;

			if(isParams)
				InnerType = type.GetTypeInfo().GetElementType().GenericTypeArguments[1];
			else if(isEnumerable)
				InnerType = type.GetTypeInfo().GetGenericArguments().First();
			else
				InnerType = null;
		}

		// Compare everything except the description

		public override bool Equals(object obj)
			=> obj is ParserSuggestionArgument
				? Equals(this, (ParserSuggestionArgument)obj)
				: false;

		bool Equals(ParserSuggestionArgument x, ParserSuggestionArgument y)
		{
			if(ReferenceEquals(x, null) || ReferenceEquals(y, null))
				return ReferenceEquals(x, null) && ReferenceEquals(y, null);

			if(!StringComparer.OrdinalIgnoreCase.Equals(x.Name, y.Name))
				return false;

			if(x.Type != y.Type)
				return false;

			if(x.IsEnumerable != y.IsEnumerable)
				return false;

			if(x.IsOptional != y.IsOptional)
				return false;

			if(x.IsParams != y.IsParams)
				return false;

			return true;
		}

		public override int GetHashCode()
		{
			var hashCode = HashCodeSeed;

			unchecked // Allow arithmetic overflows
			{
				hashCode = hashCode * HashCodeFactor + Name?.GetHashCode() ?? 0;
				hashCode = hashCode * HashCodeFactor + Type?.GetHashCode() ?? 0;
				hashCode = hashCode * HashCodeFactor + IsEnumerable.GetHashCode();
				hashCode = hashCode * HashCodeFactor + IsOptional.GetHashCode();
				hashCode = hashCode * HashCodeFactor + IsParams.GetHashCode();
			}

			return hashCode;
		}
	}
}
