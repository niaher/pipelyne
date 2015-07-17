namespace Pipelyne.Web
{
	using System.Web.Http;
	using System.Web.Http.OData.Extensions;

	public class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.AddODataQueryFilter();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional });

			config.Routes.MapHttpRoute(
				name: "ApiByName",
				routeTemplate: "api/{controller}/{action}/{id}",
				defaults: new { id = RouteParameter.Optional, action = "Get" });
		}
	}
}