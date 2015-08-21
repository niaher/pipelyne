namespace Pipelyne.Core
{
	public interface ITransformer
	{
		string Name { get; }
		ContentItem Transform(string input, TransformationRequest request);
	}
}