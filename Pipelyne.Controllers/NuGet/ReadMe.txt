## Configure ASP.NET MVC

Call the following at `Application_Start` (in *Global.asax*).

```
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
```