namespace Pipelyne.Core
{
	public class TextTransformer : ITransformer
	{
		public string Name => "text";

		public ContentItem Transform(TransformRequest request)
		{
			return new ContentItem
			{
				Content = request.Input,
				ContentType = "text/plain"
			};
		}
	}
}