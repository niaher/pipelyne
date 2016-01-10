namespace Pipelyne.Core
{
	using System.Globalization;

	internal class TransformQuery
	{
		public TransformQuery(string query)
		{
            this.TransformerName = query.SubstringUpTo("(").ToLower(CultureInfo.InvariantCulture);
			this.Arguments = query.SubstringFrom("(", inclusive: false).SubstringUpTo(")").Split(',');
		}

		public string[] Arguments { get; set; }

		public string TransformerName { get; }
	}
}