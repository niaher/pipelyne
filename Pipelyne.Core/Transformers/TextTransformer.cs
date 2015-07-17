namespace Pipelyne.Core
{
	public class TextTransformer : ITransformer
	{
		public string Name
		{
			get
			{
				return "text";
			}
		}

		public ContentItem Transform(string input)
		{
			return new ContentItem
			{
				Content = input,
				ContentType = "text/plain"
			};
		}
	}
}