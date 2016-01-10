namespace Pipelyne.Core
{
	public class MarkdownTransformer : ITransformer
	{
		public string Name => "md-html";

		public ContentItem Transform(TransformRequest request)
		{
			var html = CommonMark.CommonMarkConverter.Convert(request.Input);

			return new ContentItem(html, "text/html");
		}
	}
}