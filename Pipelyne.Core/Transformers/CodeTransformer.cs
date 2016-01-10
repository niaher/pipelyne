namespace Pipelyne.Core
{
	public class CodeTransformer : ITransformer
	{
		public string Name => "code";

		public ContentItem Transform(TransformRequest request)
		{
			string html = "<pre>" + request.Input + "</pre>";
			return new ContentItem(html, "text/html");
		}
	}
}