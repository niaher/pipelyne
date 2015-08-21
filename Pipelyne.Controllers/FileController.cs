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
			ContentItem content = this.pipelyne.ProcessRequest(request);

			return this.ResponseMessage(new HttpResponseMessage
			{
				Content = new StringContent(content.Content, Encoding.UTF8, content.ContentType)
			});
		}
	}
}