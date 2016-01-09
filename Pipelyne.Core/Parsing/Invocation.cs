namespace Pipelyne.Core.Parsing
{
	using System.Collections.Generic;
	using System.Linq;

	public class Invocation
	{
		internal Invocation(IEnumerable<Argument> arguments)
		{
			this.Arguments = arguments.ToDictionary(t => t.Parameter.Name, t => t);
		}

		public IReadOnlyDictionary<string, Argument> Arguments { get; }
	}
}