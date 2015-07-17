namespace Pipelyne.Core
{
	using System.Collections.Generic;

	internal static class Extensions
	{
		public static T TryGet<T>(this IList<T> list, int index)
		{
			return index < list.Count ? list[index] : default(T);
		}

		public static T TryGet<T>(this IList<T> list, int index, T defaultValue)
		{
			return index < list.Count ? list[index] : defaultValue;
		}
	}
}