namespace Pipelyne.Core
{
	using System;
	using System.Collections.Generic;

	internal static class Extensions
	{
		public static string SubstringFrom(
			this string input,
			string value,
			StringComparison stringComparison = StringComparison.OrdinalIgnoreCase,
			bool inclusive = true)
		{
			int index = input.IndexOf(value, stringComparison);

			if (index > 0)
			{
				index = inclusive ? index : index + value.Length;

				return input.Substring(index);
			}

			return string.Empty;
		}

		public static string SubstringUpTo(
			this string input,
			string value,
			StringComparison stringComparison = StringComparison.OrdinalIgnoreCase,
			bool inclusive = false)
		{
			int index = input.IndexOf(value, stringComparison);

			if (index > 0)
			{
				index = inclusive ? index + value.Length : index;

				return input.Substring(0, index);
			}

			return input;
		}

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