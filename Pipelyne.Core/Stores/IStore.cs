namespace Pipelyne.Core
{
	public interface IStore
	{
		ContentItem GetContent(string id, bool throwExceptionIfNotFound);
		string Name { get; }
	}
}