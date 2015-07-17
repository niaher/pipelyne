namespace Pipelyne.Web.Logic
{
	using System;
	using System.Net;
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;
	using Pipelyne.Web.Controllers;

	public class MyControllerFactory : DefaultControllerFactory
	{
		protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
		{
			try
			{
				if (controllerType != null)
				{
					return DependencyResolver.Current.GetService(controllerType) as IController;
				}

				return base.GetControllerInstance(requestContext, null);
			}
			catch (HttpException ex)
			{
				if (ex.GetHttpCode() == (int)HttpStatusCode.NotFound)
				{
					requestContext.RouteData.Values["controller"] = "Error";
					requestContext.RouteData.Values["action"] = "NotFound";

					return new ErrorController();
				}

				throw;
			}
		}
	}
}