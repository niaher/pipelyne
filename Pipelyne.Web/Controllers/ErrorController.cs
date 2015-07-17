namespace Pipelyne.Web.Controllers
{
	using System.Web.Mvc;

	public partial class ErrorController : Controller
	{
		public virtual ActionResult NotFound()
		{
			this.Response.StatusCode = 404;
			return this.View(MVC.Error.Views.NotFound);
		}
	}
}