namespace Pipelyne.Core
{
	using System;

	public class StoreItemNotFoundException : Exception
	{
		public StoreItemNotFoundException(string storeName, string itemId)
			: base(string.Format("Item '{0}' not found in store '{1}'.", itemId, storeName))
		{
		}
	}
}