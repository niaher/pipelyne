namespace Pipelyne.Web
{
	using System;
	using System.Globalization;
	using System.Threading;
	using System.Web;
	using System.Web.Http;
	using System.Web.Mvc;
	using System.Web.Optimization;
	using System.Web.Routing;
	using Pipelyne.Web.Controllers;
	using Pipelyne.Web.Logic;

	public class Global : HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			AreaRegistration.RegisterAllAreas();

			BundleConfig.RegisterBundles(BundleTable.Bundles);
			WebApiConfig.Register(GlobalConfiguration.Configuration);
			MvcConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			MvcConfig.RegisterRoutes(RouteTable.Routes);

			ControllerBuilder.Current.SetControllerFactory(new MyControllerFactory());

			SetupCulture();
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			this.HandleHttp404<ErrorController>(MVC.Error.Name, MVC.Error.ActionNames.NotFound);
		}

		private void HandleHttp404<TErrorController>(string controller, string action)
			where TErrorController : class, IController, new()
		{
			Exception exception = this.Server.GetLastError();
			HttpException httpException = exception as HttpException;
			if (httpException != null && httpException.GetHttpCode() == 404)
			{
				RouteData routeData = new RouteData();
				routeData.Values.Add("controller", controller);
				routeData.Values.Add("action", action);
				this.Server.ClearError();
				this.Response.Clear();
				IController errorController = new TErrorController();
				errorController.Execute(new RequestContext(new HttpContextWrapper(this.Context), routeData));
			}
		}

		private static void SetupCulture()
		{
			var newCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
			newCulture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
			newCulture.DateTimeFormat.ShortDatePattern = "dd MMM yyyy";
			newCulture.DateTimeFormat.DateSeparator = "-";
			newCulture.NumberFormat.NumberDecimalDigits = 2;
			newCulture.NumberFormat.CurrencyDecimalDigits = 2;
			newCulture.NumberFormat.CurrencyNegativePattern = 1;
			newCulture.NumberFormat.CurrencySymbol = string.Empty;
			Thread.CurrentThread.CurrentCulture = newCulture;
		}
	}
}