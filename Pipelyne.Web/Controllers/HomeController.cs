namespace Pipelyne.Web.Controllers
{
	using System.Web.Mvc;
	using Pipelyne.Core;

	public partial class HomeController : Controller
	{
		private readonly Pipelyne pipelyne;

		public HomeController(Pipelyne pipelyne)
		{
			this.pipelyne = pipelyne;
		}

		public virtual ActionResult Index()
		{
			return this.View(this.pipelyne);
		}
	}
}