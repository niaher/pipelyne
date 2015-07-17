namespace Pipelyne.Web.Logic.View
{
	public interface IViewBagFactory<out TViewBag>
		where TViewBag : class
	{
		TViewBag Create(dynamic viewBag);
	}
}