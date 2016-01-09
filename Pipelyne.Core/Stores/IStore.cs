namespace Pipelyne.Core
{
	using global::Pipelyne.Core.Parsing;

	public interface IStore
	{
		string Name { get; }
		Signature Signature { get; }
		ContentItem GetContent(string id, bool throwExceptionIfNotFound);
	}
}