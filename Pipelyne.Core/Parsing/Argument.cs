namespace Pipelyne.Core.Parsing
{
	using System;

	/// <summary>
	/// Represents a value supplied to a <see cref="Parameter"/>.
	/// </summary>
	public class Argument
	{
		/// <summary>
		/// Initializes a new instance of <see cref="Argument"/> class.
		/// </summary>
		/// <param name="value">String representation of the argument value.</param>
		/// <param name="parameter">Corresponding <see cref="Parameter"/> object.</param>
		internal Argument(string value, Parameter parameter)
		{
			this.Value = value;
			this.Parameter = parameter;
		}

		/// <summary>
		/// Gets corresponding <see cref="Parameter"/> object.
		/// </summary>
		public Parameter Parameter { get; }

		/// <summary>
		/// String representation of argument value.
		/// </summary>
		public string Value { get; }

		/// <summary>
		/// Parses and returns argument value as <see cref="int"/>.
		/// </summary>
		/// <returns>Argument value as <see cref="int"/>.</returns>
		public int AsInt32()
		{
			return int.Parse(this.Value);
		}

		/// <summary>
		/// Parses and returns argument value as enum of type <see cref="T"/>.
		/// </summary>
		/// <returns>Argument value as <see cref="T"/>.</returns>
		public T AsEnum<T>()
		{
			return (T)Enum.Parse(typeof(T), this.Value, true);
		}

		public Uri AsUri()
		{
			return new Uri(this.Value);
		}
	}
}