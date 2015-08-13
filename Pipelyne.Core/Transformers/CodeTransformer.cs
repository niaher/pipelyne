namespace Pipelyne.Core
{
	public class CodeTransformer : ITransformer
	{
		public string Name => "code";

		public ContentItem Transform(string input)
		{
			string html = "<pre>" + input + "</pre>";
			return new ContentItem(html, "text/html");
		}
	}
}