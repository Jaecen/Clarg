using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Clarg
{
	// A _C_ommand _L_ine _ARG_uments parser
	//	- Doesn't require a decorated type or mutable properties to specify arguments. 
	//	- Arguments are determined from the constructor of the target type.
	//	- Accepts -- or / for param names.
	//	- Assumes boolean if just a name is given.
	//	- Supports optional arguments via, well, optional arguments.
	//	- Supports extra arguments via params arg.
	//	- T is any undecorated type.
	//	- T can be immutable.

	// Eventially will support
	//	- Enumerable args
	//	- Generating help text

	public class Parser
	{
		public Parser()
		{ }

		public T Create<T>(string[] args)
		{
			// Recursively parse the arguments into KVP's
			var descriptors = ParseArguments(
					args.Select(s => (s ?? String.Empty).Trim())				// Help ensure good data going in
				)
				.ToDictionary(													// This will eventually need to change to a GroupBy to support enumerable values
					ad => ad.Key, 
					ad => ad.Value,
					StringComparer.OrdinalIgnoreCase							// Ignore case
				);

			// Find a constructor to invoke by matching descriptors to constructor parameters in the following order of preference:
			//	1. Every parameter and descriptor is matched 1:1
			//	2. Every descriptor is matched to a parameter, and the parameters without matches have default values
			//	3. Every non-optional parameter is matched to a descriptor, every unmatched parameter has a default value, and there is a params array for the unmatched descriptors
			var targetConstructor = typeof(T)
				.GetConstructors()
				.Select(constructor => new										// Evaluate each constructor
				{
					constructor,
					arguments = constructor
						.GetParameters()
						.Select(parameter => new								// For each constructor parameter, see if we have a matching descriptor, if it's optional, and if it's a params array
						{
							hasMatch = descriptors
								.Where(descriptor => parameter.Name.Equals(descriptor.Key, StringComparison.OrdinalIgnoreCase))	// Find the parameter name regardless of case
								.Select(descriptor => new KeyValuePair<string, string>(parameter.Name, descriptor.Value))		// Use the correctly-cased parameter name
								.Any(),
							isOptional = parameter.IsOptional,
							isParamArray = parameter.ParameterType == typeof(KeyValuePair<string, string>[]) && parameter.GetCustomAttributes<ParamArrayAttribute>().Any(),
						}),
					descriptors = descriptors									// For each descriptor, see if it has a matching constructor parameter
						.Select(descriptor => new
						{
							hasMatch = constructor
								.GetParameters()
								.Where(parameter => parameter.Name.Equals(descriptor.Key, StringComparison.OrdinalIgnoreCase))	// Find the parameter name regardless of case
								.Select(parameter => new KeyValuePair<string, string>(parameter.Name, descriptor.Value))		// Use the correctly-cased parameter name
								.Any(),
						}),
				})
				.Select(o => new
				{
					o.constructor,
					o.arguments,
					o.descriptors,
					isMatch = o													// A constructor that has a descriptor for every parameter and an parameter for every descriptor
						.arguments
						.All(argument => argument.hasMatch && !argument.isParamArray)
						&& o
						.descriptors
						.All(descriptor => descriptor.hasMatch),
					isMatchWithOptionals = o									// A constructor that has an parameter for every descriptor and the rest are optional
						.descriptors
						.All(descriptor => descriptor.hasMatch)
						&& o
						.arguments
						.All(argument => (argument.hasMatch || argument.isOptional) && !argument.isParamArray),
					isMatchWithParams = o										// A constructor that has a descriptor for every required parameter and a params array for the rest of the descriptors
						.arguments
						.Where(argument => !argument.isParamArray)
						.All(argument => argument.hasMatch || argument.isOptional)
						&& o
						.arguments
						.Any(argument => argument.isParamArray)
				})
				.Where(o => o.isMatch || o.isMatchWithOptionals || o.isMatchWithParams)
				.OrderBy(o => o.isMatch ? 0 : o.isMatchWithOptionals ? 1 : 2)	// Order by preference (lower is more preferred)
				.ThenByDescending(o => o										// Then order by highest number of matched args
					.arguments
					.Where(argument => argument.hasMatch)
					.Count()
				)
				.Select(o => o.constructor)
				.FirstOrDefault();

			if(targetConstructor == null)
				throw new Exception("No constructor found that matches provided arguments");

			// Build the constructor parameter list
			var parameters = targetConstructor
				.GetParameters()
				.Where(parameter => parameter.GetCustomAttribute<ParamArrayAttribute>() == null)
				.Select(pi => GetArgumentForParameter(descriptors, pi));

			// Add a params arg if needed
			var hasParamsArg = targetConstructor
				.GetParameters()
				.Where(parameter => parameter.GetCustomAttribute<ParamArrayAttribute>() != null)
				.Any();

			if(hasParamsArg)
				parameters = parameters.Concat(new object[] { GetArgumentsForParamsArray(descriptors, targetConstructor.GetParameters()) });

			// Pull the trigger and hope for the best
			return (T)targetConstructor.Invoke(parameters.ToArray());
		}

		object GetArgumentForParameter(Dictionary<string, string> descriptors, ParameterInfo parameter)
		{
			// Unspecified optional params use their default value
			if(parameter.IsOptional && !descriptors.ContainsKey(parameter.Name))
				return parameter.DefaultValue;

			// Convert all other args from string to the desired type
			return TypeDescriptor
				.GetConverter(parameter.ParameterType)
				.ConvertFromString(descriptors[parameter.Name]);
		}

		KeyValuePair<string, string>[] GetArgumentsForParamsArray(Dictionary<string, string> descriptors, ParameterInfo[] constructorParameters)
		{
			// Get all descriptors that don't have a corresponding argument
			return descriptors
				.Where(descriptor => !constructorParameters
					.Select(parameter => parameter.Name)
					.Contains(descriptor.Key, StringComparer.OrdinalIgnoreCase)		// Ignore case
				)
				.ToArray();
		}

		IEnumerable<KeyValuePair<string, string>> ParseArguments(IEnumerable<string> args)
		{
			if(!args.Any())
				return Enumerable.Empty<KeyValuePair<string, string>>();

			var arg = args.First();

			if(arg.StartsWith("--"))
				return ParseArguments(args.Skip(1), arg.Substring(2));
			else if(arg.StartsWith("/"))
				return ParseArguments(args.Skip(1), arg.Substring(1));
			else
				throw new Exception("Argument provided without a name.");
		}

		IEnumerable<KeyValuePair<string, string>> ParseArguments(IEnumerable<string> args, string argumentName)
		{
			var arg = args.FirstOrDefault();

			if(!args.Any() || (arg.StartsWith("--") || arg.StartsWith("/")))
				return new[] { new KeyValuePair<string, string>(argumentName, Boolean.TrueString) }
					.Concat(ParseArguments(args));									// Don't advance args, as there was no value
			else
				return new[] { new KeyValuePair<string, string>(argumentName, arg) }
					.Concat(ParseArguments(args.Skip(1)));
		}
	}
}
