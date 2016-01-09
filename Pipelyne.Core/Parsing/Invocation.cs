namespace Pipelyne.Core.Parsing
{
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Represents invocation of a <see cref="Store"/>.
	/// </summary>
	public class Invocation
	{
		internal Invocation(IEnumerable<Argument> arguments)
		{
			this.Arguments = arguments.ToDictionary(t => t.Parameter.Name, t => t);
		}

		/// <summary>
		/// List of supplied arguments.
		/// </summary>
		public IReadOnlyDictionary<string, Argument> Arguments { get; }
	}
}