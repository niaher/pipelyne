namespace Pipelyne.Core
{
	using System;
	using System.IO;
	using System.Net;

	public class UrlStore : IStore
	{
		public ContentItem GetContent(string id, bool throwExceptionIfNotFound)
		{
			var uri = new Uri(id);
			var request = WebRequest.Create(uri);
			request.UseDefaultCredentials = true;
			
			var response = request.GetResponse();
			
			var result = new ContentItem { ContentType = response.ContentType };

			using (var stream = response.GetResponseStream())
			{
				if (stream == Stream.Null || stream == null)
				{
					if (throwExceptionIfNotFound)
					{
						throw new StoreItemNotFoundException(this.Name, id);
					}

					return null;
				}

				using (var reader = new StreamReader(stream))
				{
					result.Content = reader.ReadToEnd();
				}
			}

			return result;
		}

		public string Name
		{
			get
			{
				return "url";
			}
		}
	}
}