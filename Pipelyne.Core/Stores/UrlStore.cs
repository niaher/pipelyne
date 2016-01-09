namespace Pipelyne.Core
{
	using System;
	using System.IO;
	using System.Net;
	using global::Pipelyne.Core.Parsing;

	public class UrlStore : IStore
	{
		private static readonly Signature SignatureInstance = new Signature(new Parameter("url"));

		public string Name => "url";
		public Signature Signature => SignatureInstance;

		public ContentItem GetContent(string id, bool throwExceptionIfNotFound)
		{
			var uri = new Uri(id);
			var request = WebRequest.Create(uri);
			request.UseDefaultCredentials = true;

			var response = this.GetResponse(id, request);

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

		private WebResponse GetResponse(string id, WebRequest request)
		{
			try
			{
				return request.GetResponse();
			}
			catch (Exception)
			{
				throw new StoreItemNotFoundException(this.Name, id);
			}
		}
	}
}