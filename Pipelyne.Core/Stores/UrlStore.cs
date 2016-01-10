namespace Pipelyne.Core
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Net;
	using global::Pipelyne.Core.Parsing;

	public class UrlStore : Store
	{
		private static readonly IReadOnlyList<Parameter> ParameterList = new List<Parameter> { new Parameter("url") };

		public override string Name => "url";

		public override IReadOnlyList<Parameter> Parameters => ParameterList;

		public override ContentItem GetContent(IReadOnlyDictionary<string, Argument> invocation, bool throwExceptionIfNotFound)
		{
			var uri = invocation["url"].AsUri();
			var request = WebRequest.Create(uri);
			request.UseDefaultCredentials = true;

			var response = this.GetResponse(uri.AbsoluteUri, request);

			var result = new ContentItem { ContentType = response.ContentType };

			using (var stream = response.GetResponseStream())
			{
				if (stream == Stream.Null || stream == null)
				{
					if (throwExceptionIfNotFound)
					{
						throw new StoreItemNotFoundException(this.Name, uri.AbsoluteUri);
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