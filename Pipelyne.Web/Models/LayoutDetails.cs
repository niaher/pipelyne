namespace Pipelyne.Web.Models
{
	using System.Collections.Generic;
	using Coderful.Web;

	/// <summary>
	/// Encapsulates details for layout.
	/// </summary>
	public class LayoutDetails
	{
		public LayoutDetails()
		{
			this.Title = string.Empty;
			this.Breadcrumbs = new List<Link>();
		}

		public enum TopMenu
		{
			Home = 1,
			Admin
		}

		public string Title { get; set; }
		public List<Link> Breadcrumbs { get; set; }
		public TopMenu CurrentTopMenu { get; set; }
	}
}