namespace Pipelyne.Core.Parsing
{
	using System.Collections.Generic;

	public class Signature
	{
		public Signature(params Parameter[] parameters)
		{
			this.Parameters = parameters;
		}

		public IReadOnlyList<Parameter> Parameters { get; }

		public Invocation CreateInvocation(string invocation)
		{
			var arguments = invocation.Split(',');

			var list = new List<Argument>();

			for (int i = 0; i < this.Parameters.Count; i++)
			{
				var parameter = this.Parameters[i];
				var argument = arguments[i];

				var a = parameter.CreateArgument(argument);
				list.Add(a);
			}

			return new Invocation(list);
		}
	}
}