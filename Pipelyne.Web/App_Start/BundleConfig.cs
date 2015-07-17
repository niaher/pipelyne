namespace Pipelyne.Web
{
	using System.Web.Optimization;

	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/js/libs/angular").Include(
				"~/Scripts/Libs/Angular/angular.js",
				"~/Scripts/Libs/Angular/angular-route.js",
				"~/Scripts/Libs/Angular/angular-animate.js",
				"~/Scripts/Libs/ui-bootstrap/ui-bootstrap-0.11.0.js",
				"~/Scripts/Libs/ngProgress/ngProgress.js",
				"~/Scripts/Libs/script.js"));

			bundles.Add(new ScriptBundle("~/js/app").Include(
				"~/Scripts/App/Coderful.Persona.js",
				"~/Scripts/App/App.js",
				"~/Scripts/App/Coderful.Progress.js"));

			bundles.Add(new StyleBundle("~/css/main")
				.Include("~/Styles/Bootstrap/less/bootstrap.css", new CssRewriteUrlTransform())
				.Include("~/Styles/Bootstrap/less/theme.css", new CssRewriteUrlTransform())
				.Include("~/Styles/FontAwesome/css/font-awesome.css", new CssRewriteUrlTransform())
				.Include("~/Scripts/Libs/ngProgress/ngProgress.css", new CssRewriteUrlTransform())
				.Include("~/Styles/Main.css", new CssRewriteUrlTransform()));
		}
	}
}
