namespace Pipelyne.Core.Parsing
{
	using System.Collections.Generic;

	/// <summary>
	/// Represents "method signature" of an <see cref="IStore"/>.
	/// </summary>
	public class Signature
	{
		/// <summary>
		/// Initializes a new instance of <see cref="Signature"/> class.
		/// </summary>
		/// <param name="parameters">List of parameters accepted by the <see cref="IStore"/>.</param>
		public Signature(params Parameter[] parameters)
		{
			this.Parameters = parameters;
		}

		/// <summary>
		/// Gets list of parameters accepted by <see cref="IStore"/>.
		/// </summary>
		public IReadOnlyList<Parameter> Parameters { get; }

		/// <summary>
		/// Creates <see cref="Invocation"/> instance which represents invocation of
		/// this signature.
		/// </summary>
		/// <param name="arguments">Comma-separated list of arguments.</param>
		/// <returns><see cref="Invocation"/> instance.</returns>
		public Invocation CreateInvocation(string arguments)
		{
			var args = arguments.Split(',');

			var list = new List<Argument>();

			for (int i = 0; i < this.Parameters.Count; i++)
			{
				var parameter = this.Parameters[i];
				var argument = args[i];

				var a = parameter.CreateArgument(argument);
				list.Add(a);
			}

			return new Invocation(list);
		}
	}
}