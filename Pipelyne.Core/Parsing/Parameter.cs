namespace Pipelyne.Core.Parsing
{
	public class Parameter
	{
		public Parameter(string name)
		{
			this.Name = name;
		}

		public string Name { get; }

		internal Argument CreateArgument(string argument)
		{
			return new Argument(argument, this);
		}
	}
}