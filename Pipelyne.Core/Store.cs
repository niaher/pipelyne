namespace Pipelyne.Core
{
	using System.Collections.Generic;
	using System.Linq;
	using global::Pipelyne.Core.Parsing;

	public abstract class Store
	{
		public abstract string Name { get; }

		/// <summary>
		/// Gets list of parameters accepted by <see cref="Store"/>.
		/// </summary>
		public abstract IReadOnlyList<Parameter> Parameters { get; }

		public abstract ContentItem GetContent(IReadOnlyDictionary<string, Argument> id, bool throwExceptionIfNotFound);

		internal ContentItem GetContent(string id, bool throwExceptionIfNotFound)
		{
			var argumentDictionary = this.CreateInvocation(id).ToDictionary(t => t.Parameter.Name, t => t);
			return this.GetContent(argumentDictionary, throwExceptionIfNotFound);
		}

		/// <summary>
		/// Parses argument string into list of <see cref="Argument"/> objects, which
		/// correspond to <see cref="Parameters"/> collection.
		/// </summary>
		/// <param name="arguments">Comma-separated list of arguments.</param>
		/// <returns><see cref="IReadOnlyDictionary{TKey,TValue}"/> instance.</returns>
		private IList<Argument> CreateInvocation(string arguments)
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

			return list;
		}
	}
}