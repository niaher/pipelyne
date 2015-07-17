namespace Pipelyne.Core
{
	public class ContentItem
	{
		public ContentItem()
		{
		}

		public ContentItem(string content, string contentType)
		{
			this.Content = content;
			this.ContentType = contentType;
		}

		public string Content { get; set; }
		public string ContentType { get; set; }
	}
}