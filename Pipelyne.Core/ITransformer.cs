namespace Pipelyne.Core
{
	public interface ITransformer
	{
		string Name { get; }
		ContentItem Transform(TransformRequest request);
	}
}