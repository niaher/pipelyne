namespace Pipelyne.Core
{
	public class MarkdownTransformer : ITransformer
	{
		public string Name => "md-html";

		public ContentItem Transform(string input)
		{
			var html = CommonMark.CommonMarkConverter.Convert(input);

			return new ContentItem(html, "text/html");
		}
	}
}