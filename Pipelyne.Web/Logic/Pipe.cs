namespace Pipelyne.Web.Logic
{
	using System;
	using Microsoft.Practices.Unity;
	using Pipelyne.Core;

	public class Pipe
	{
		public static Lazy<Pipelyne> Pipelyne = new Lazy<Pipelyne>(() => UnityConfig.GetContainer().Resolve<Pipelyne>());
	}
}