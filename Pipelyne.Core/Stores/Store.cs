namespace Pipelyne.Core
{
	using System.Collections.Generic;
	using System.IO.IsolatedStorage;
	using global::Pipelyne.Core.Parsing;

	public abstract class Store
	{
		public abstract string Name { get; }

		/// <summary>
		/// Gets list of parameters accepted by <see cref="Store"/>.
		/// </summary>
		public abstract IReadOnlyList<Parameter> Parameters { get; }

		/// <summary>
		/// Creates <see cref="Invocation"/> instance which represents invocation of
		/// this signature.
		/// </summary>
		/// <param name="arguments">Comma-separated list of arguments.</param>
		/// <returns><see cref="Invocation"/> instance.</returns>
		private Invocation CreateInvocation(string arguments)
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

		internal ContentItem GetContent(string id, bool throwExceptionIfNotFound)
		{
			var invocation = this.CreateInvocation(id);
			return this.GetContent(invocation, throwExceptionIfNotFound);
		}

		public abstract ContentItem GetContent(Invocation id, bool throwExceptionIfNotFound);
	}
}