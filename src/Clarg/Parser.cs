using System;
using System.Collections;
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
	//	- Supports multiple argument values via IEnumerable
	//	- T is any undecorated type.
	//	- T can be immutable.

	// Eventially will support
	//	- Generating help text
	//	- Enums

	public class Parser
	{
		readonly Tokenizer Tokenizer;
		public Parser()
		{
			Tokenizer = new Tokenizer();
		}

		public T Create<T>(string[] args)
		{
			// Turn args into a set of kvp's
			var arguments = Tokenizer.Tokenize(args);

			// Extract some basic information about each constructor on the target type and its parameters
			var constructorCandidates = typeof(T)
				.GetConstructors()
				.Select(constructor => new
					{
						constructor,
						parameters = constructor
							.GetParameters()
							.Select(parameter => new ParameterDescriptor(
									name: parameter.Name,
									type: parameter.ParameterType,
									isEnumerable: parameter.ParameterType.IsAssignableFrom(typeof(IEnumerable<>)),
									isOptional: parameter.IsOptional,
									isParamsArray: parameter.ParameterType == typeof(KeyValuePair<string, string>[]) && parameter.GetCustomAttributes<ParamArrayAttribute>().Any(),
									parameterInfo: parameter
								))
							.ToArray(),
					})
				.Select(candidate => new
					{
						candidate.constructor,
						candidate.parameters,
						paramsArray = candidate
							.parameters
							.Where(parameter => parameter.IsParamsArray)
							.FirstOrDefault(),
					})
				.ToArray();

			// Map up the arugments and parameters by name
			var mappedCandidates = constructorCandidates
				.Select(candidate => new
					{
						candidate.constructor,
						candidate.paramsArray,
						mappings = candidate
							.parameters
							.Where(parameter => !parameter.IsParamsArray)					// Don't map params array on name
							.Select(parameter => new ArgumentParameterMapping(
								argument: arguments
									.Where(argument => parameter.Name.Equals(argument.Name, StringComparison.OrdinalIgnoreCase))
									.FirstOrDefault(),
								parameter: parameter))
							.Union(arguments
								.Select(argument => new ArgumentParameterMapping(
									argument: argument,
									parameter: candidate
										.parameters
										.Where(parameter => parameter.Name.Equals(argument.Name, StringComparison.OrdinalIgnoreCase))
										.DefaultIfEmpty(candidate.paramsArray)				// Default to params array, if it exists
										.FirstOrDefault())))
							.ToArray()
					})
				.ToArray();

			// Evaluate each parameter mapping and decide if and how the associated constructor matches the arguments
			var evaluatedCandidates = mappedCandidates
				.Select(candidate => new
					{
						candidate.constructor,
						candidate.mappings,
						candidate.paramsArray,

						isExactMatch = candidate									// A constructor that has an argument for every parameter and an parameter for every argument
							.mappings
							.All(mapping => 
								mapping.Parameter != null 
								&& mapping.Argument != null
								&& !mapping.Parameter.IsParamsArray),

						isMatchWithOptionals = candidate							// A constructor that has a parameter for every argument and the remaining parameters are optional
							.mappings
							.All(mapping => 
								mapping.Parameter != null
								&& (mapping.Argument != null || mapping.Parameter.IsOptional)),

						isMatchWithParams = candidate								// A constructor that has an argument for every required parameter and a params array for the rest of the arguments
							.mappings
							.All(mapping => 
								candidate.paramsArray != null
								&& (mapping.Parameter == null						// Ensure that all non-params-array parameters are mapped
									|| (!mapping.Parameter.IsParamsArray 
										&& (mapping.Argument != null || mapping.Parameter.IsOptional)
										)
									)
							)
					});
		
			// Rank the candidates in order of preference:
			//	1. Every parameter and argument is matched 1:1
			//	2. Every argument is matched to a parameter, and the parameters without matches have default values
			//	3. Every non-optional parameter is matched to a argument, every unmatched parameter has a default value, and there is a params array for the unmatched arguments
			//	null: Not all arguments could be matched to parameters
			var rankedCandidates = evaluatedCandidates
				.Select(candidate => new
				{
					candidate.constructor,
					candidate.mappings,
					candidate.paramsArray,
					candidate.isExactMatch,
					candidate.isMatchWithOptionals,
					candidate.isMatchWithParams,
					matchRanking = 
						candidate.isExactMatch ? 0 
						: candidate.isMatchWithOptionals ? 1 
						: candidate.isMatchWithParams ? 2 
						: (int?)null,
					parameterRanking = candidate
						.mappings
						.Where(mapping => mapping.Parameter != null)
						.Count()
				})
				.OrderBy(candidate => candidate.matchRanking)						// Order by preference (lower is more preferred)
				.ThenByDescending(candidate => candidate.parameterRanking);			// Then order by highest number of matched parameters

			// Filter out constructors that we don't have the arguments for
			var viableCandidates = rankedCandidates
				.Where(candidate => candidate.matchRanking != null);

			// Prepare the arguments by converting them to the corresponding parameter type
			var preparedCandidates = viableCandidates
				.Select(candidate => new
					{
						candidate.constructor,
						candidate.matchRanking,
						candidate.parameterRanking,
						typedParameters = candidate
							.mappings
							.Concat(												// Ensure the params array argument is included even if there are no additional arguments
								candidate.paramsArray != null && !candidate.mappings.Any(mapping => mapping.Parameter == candidate.paramsArray) 
									? new[] { new ArgumentParameterMapping(null, candidate.paramsArray) } 
									: new ArgumentParameterMapping[] { })
							.GroupBy(mapping => mapping.Parameter, mapping => mapping.Argument)
							.Select(groupedMapping => GetArgumentForParameter(groupedMapping))
					});

			if(!preparedCandidates.Any())
			{
				// Maybe do something better here like write out what args are missing from best possible candidate
				throw new Exception("No constructor found that matches provided arguments");
			}

			// Create an instance of the type
			var selectedCandidate = preparedCandidates.First();
			var instance = (T)selectedCandidate.constructor.Invoke(selectedCandidate.typedParameters.ToArray());

			return instance;
		}

		object GetArgumentForParameter(IGrouping<ParameterDescriptor, ArgumentDescriptor> parameterMapping)
		{
			// Unspecified optional params use their default value
			if(parameterMapping.Key.IsOptional && !parameterMapping.Where(argument => argument != null).Any())
				return parameterMapping.Key.ParameterInfo.DefaultValue;

			// Params array is always KVP<string, string>[]
			if(parameterMapping.Key.IsParamsArray)
				return parameterMapping
					.Where(argument => argument != null)
					.Select(argument => new KeyValuePair<string, string>(argument.Name, argument.Value))
					.ToArray();

			var parameterType = parameterMapping
				.Key
				.ParameterInfo
				.ParameterType;

			var parameterIsEnumerable = parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>);

			if(parameterIsEnumerable)
				parameterType = parameterType
					.GetGenericArguments()
					.First();

			var converter = TypeDescriptor
				.GetConverter(parameterType);

			// Convert all other args from string to the desired type
			var convertedArguments = parameterMapping
				.Select(argument => converter.ConvertFromString(argument.Value))
				.ToArray();

			if(parameterIsEnumerable)
			{
				// Cast the converted types to parameterType
				// Get the generic cast extension method
				var castExtensionMethod = typeof(System.Linq.Enumerable)
					.GetMethod("Cast", BindingFlags.Static | BindingFlags.Public)
					.MakeGenericMethod(parameterType);

				// Invoke the cast method
				var convertedEnumerable = (IEnumerable)castExtensionMethod.Invoke(convertedArguments, new[] { convertedArguments });

				return convertedEnumerable;
			}
			else
				return convertedArguments.Last();
		}
	}
}
