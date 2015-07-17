namespace Pipelyne.Web.Logic.View
{
	using Pipelyne.Web.Models;

	public abstract class MyWebViewPage<TModel> : WebViewPage<TModel, MyWebViewPage<TModel>.MyViewBag>
	{
		protected MyWebViewPage()
			: base(new ViewBagFactory())
		{
		}

		private class ViewBagFactory : IViewBagFactory<MyViewBag>
		{
			public MyViewBag Create(dynamic viewBag)
			{
				return new MyViewBag(viewBag);
			}
		}

		public class MyViewBag
		{
			private readonly dynamic viewBag;

			public MyViewBag(dynamic viewBag)
			{
				this.viewBag = viewBag;
			}

			public LayoutDetails LayoutDetails
			{
				get
				{
					return this.viewBag.LayoutDetails;
				}

				set
				{
					this.viewBag.LayoutDetails = value;
				}
			}
		}
	}
}