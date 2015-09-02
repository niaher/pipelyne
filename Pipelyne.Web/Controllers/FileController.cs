namespace Pipelyne.Web.Controllers
{
	using System.Text;
	using System.Web.Mvc;
	using Pipelyne.Core;

	public partial class FileController : Controller
	{
		private readonly Pipelyne pipelyne;

		public FileController(Pipelyne pipelyne)
		{
			this.pipelyne = pipelyne;
		}

		[HttpGet]
		public virtual ActionResult Get(string source, string id, string to)
		{
			var request = new TransformationRequest
			{
				Id = id,
				Source = source,
				To = to
			};

			var content = this.pipelyne.ProcessRequest(request);

			return new ContentResult
			{
				Content = content.Content,
				ContentType = content.ContentType,
				ContentEncoding = Encoding.UTF8
			};
		}
	}
}