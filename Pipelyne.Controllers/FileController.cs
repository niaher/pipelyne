namespace Pipelyne.Controllers
{
	using System.Net.Http;
	using System.Text;
	using System.Web.Http;
	using System.Web.Http.Results;
	using Pipelyne.Core;

	public class FileController : ApiController
	{
		private readonly Pipelyne pipelyne;

		public FileController(Pipelyne pipelyne)
		{
			this.pipelyne = pipelyne;
		}

		[HttpGet]
		public ResponseMessageResult Get([FromUri]TransformationRequest request)
		{
			var store = this.pipelyne.GetStore(request.Source, true);
			var content = store.GetContent(request.Id, true);

			var transforms = this.pipelyne.GetTransforms(request.To);
			
			foreach (var transform in transforms)
			{
				content = transform.Transform(content.Content);
			}

			return this.ResponseMessage(new HttpResponseMessage
			{
				Content = new StringContent(content.Content, Encoding.UTF8, content.ContentType)
			});
		}

		public class TransformationRequest
		{
			public string Source { get; set; }
			public string Id { get; set; }
			public string To { get; set; }
		}
	}
}