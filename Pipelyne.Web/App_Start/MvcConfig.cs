namespace Pipelyne.Web
{
	using System.Web.Http;
	using System.Web.Mvc;
	using System.Web.Routing;

	public class MvcConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.LowercaseUrls = true;

			routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
				new { controller = MVC.Home.Name, action = MVC.Home.ActionNames.Index, id = RouteParameter.Optional });

			routes.MapRoute(
				name: "NotFound",
				url: "{*url}",
				defaults: new { controller = MVC.Error.Name, action = MVC.Error.ActionNames.NotFound });
		}

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}