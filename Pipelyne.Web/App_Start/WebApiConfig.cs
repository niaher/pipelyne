namespace Pipelyne.Web
{
	using System.Web.Http;
	using System.Web.Http.OData.Extensions;

	public class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.AddODataQueryFilter();
			ConfigurePipelyneApi(config);

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional });

			config.Routes.MapHttpRoute(
				name: "ApiByName",
				routeTemplate: "api/{controller}/{action}/{id}",
				defaults: new { id = RouteParameter.Optional, action = "Get" });

			// Route "pipelyne-metadata" can't be serialized to XML, so disable XML serialization until
			// a fix is found.
			config.Formatters.Remove(config.Formatters.XmlFormatter);
		}

		private static void ConfigurePipelyneApi(HttpConfiguration config, string route = "pipe")
		{
			config.Routes.MapHttpRoute(
				name: "pipelyne",
				routeTemplate: route,
				defaults: new { controller = "Pipelyne", action = "Get" });

			config.Routes.MapHttpRoute(
				name: "pipelyne-metadata",
				routeTemplate: route + "-metadata",
				defaults: new { controller = "Pipelyne", action = "Metadata" });
		}
	}
}