using System;
using System.Collections.Generic;

namespace Clarg
{
	class TokenizerResult
	{
		public readonly bool Ok;
		public readonly IEnumerable<ArgumentDescriptor> Value;
		public readonly Exception Error;

		public TokenizerResult(bool ok, IEnumerable<ArgumentDescriptor> value, Exception error)
		{
			Ok = ok;
			Value = value;
			Error = error;
		}
	}

	class TokenizerSuccess : TokenizerResult
	{
		public TokenizerSuccess(IEnumerable<ArgumentDescriptor> value)
			: base(true, value, null)
		{ }
	}

	class TokenizerError : TokenizerResult
	{
		public TokenizerError(Exception error)
			: base(false, null, error)
		{ }
	}
}
