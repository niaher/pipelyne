namespace Pipelyne.Core.Parsing
{
	using System;

	public class Argument
	{
		internal Argument(string value, Parameter parameter)
		{
			this.Value = value;
			this.Parameter = parameter;
		}

		public Parameter Parameter { get; }

		public string Value { get; }

		public int AsInt32()
		{
			return int.Parse(this.Value);
		}

		public T AsEnum<T>()
		{
			return (T)Enum.Parse(typeof(T), this.Value, true);
		}
	}
}