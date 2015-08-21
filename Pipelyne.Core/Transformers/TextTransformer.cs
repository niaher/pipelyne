namespace Pipelyne.Core
{
	public class TextTransformer : ITransformer
	{
		public string Name => "text";

		public ContentItem Transform(string input, TransformationRequest request)
		{
			return new ContentItem
			{
				Content = input,
				ContentType = "text/plain"
			};
		}
	}
}