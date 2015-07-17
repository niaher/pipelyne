namespace Pipelyne.Web.Logic.View
{
	public abstract class WebViewPage<TModel, TViewBag> : System.Web.Mvc.WebViewPage<TModel>
		where TViewBag : class
	{
		protected readonly TViewBag StrongViewBag;

		protected WebViewPage(IViewBagFactory<TViewBag> viewBagFactory)
		{
			this.StrongViewBag = viewBagFactory.Create(this.ViewBag);
		}
	}
}