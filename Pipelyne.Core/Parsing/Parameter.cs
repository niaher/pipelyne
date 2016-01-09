namespace Pipelyne.Core.Parsing
{
	using System;

	/// <summary>
	/// Represents input parameter.
	/// </summary>
	public class Parameter
	{
		/// <summary>
		/// Initializes a new instance of <see cref="Parameter"/> class with type <see cref="string"/>.
		/// </summary>
		/// <param name="name">Name of the parameter.</param>
		public Parameter(string name)
			: this (name, typeof(string))
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="Parameter"/> class.
		/// </summary>
		/// <param name="name">Name of the parameter.</param>
		/// <param name="type">Type of the parameter.</param>
        public Parameter(string name, Type type)
		{
			this.Name = name;
			this.Type = type;
		}

		/// <summary>
		/// Gets parameter name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Gets parameter type.
		/// </summary>
		public Type Type { get; }
		
		/// <summary>
		/// Creates argument for this parameter.
		/// </summary>
		/// <param name="argument">Argument value as string.</param>
		/// <returns><see cref="Argument"/> instance.</returns>
		internal Argument CreateArgument(string argument)
		{
			return new Argument(argument, this);
		}
	}
}