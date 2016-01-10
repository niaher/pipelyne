namespace Pipelyne.Core
{
	using Newtonsoft.Json.Linq;

	public class JsonTransformer : ITransformer
	{
		public string Name => "json";

		public ContentItem Transform(TransformRequest request)
		{
			var path = request.Arguments[0].Split('.');

			var result = JObject.Parse(request.Input).Root;

			foreach (var p in path)
			{
				result = result[p];
			}

			return new ContentItem(result.ToString(), "application/json");
		}
	}
}