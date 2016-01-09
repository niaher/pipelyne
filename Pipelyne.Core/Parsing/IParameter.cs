namespace Pipelyne.Core.Parsing
{
	using System;

	public interface IParameter
	{
		string Name { get; }
		Type Type { get; }
		IArgument ParseArgument(string argument);
	}
}