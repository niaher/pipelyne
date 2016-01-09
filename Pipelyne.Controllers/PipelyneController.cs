namespace Pipelyne.Controllers
{
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Text;
	using System.Web.Http;
	using System.Web.Http.Results;
	using Pipelyne.Core;

	public class PipelyneController : ApiController
	{
		private readonly Pipelyne pipelyne;

		public PipelyneController(Pipelyne pipelyne)
		{
			this.pipelyne = pipelyne;
		}

		[HttpGet]
		public ResponseMessageResult Get([FromUri]TransformationRequest request)
		{
			ContentItem content = this.pipelyne.ProcessRequest(request);

			return this.ResponseMessage(new HttpResponseMessage
			{
				Content = new StringContent(content.Content, Encoding.UTF8, content.ContentType)
			});
		}

		[HttpGet]
		public PipelyneMetadata Metadata()
		{
			return new PipelyneMetadata
			{
				Stores = this.pipelyne.Stores,
				Transformers = this.pipelyne.Transformers
			};
		}

		public class PipelyneMetadata
		{
			public IEnumerable<Store> Stores { get; set; }
			public IEnumerable<ITransformer> Transformers { get; set; }
		}
	}
}