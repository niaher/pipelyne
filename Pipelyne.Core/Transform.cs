namespace Pipelyne.Core
{
	public class Transform
	{
		public Transform(ITransformer transformer, string[] arguments)
		{
			this.Transformer = transformer;
			this.Arguments = arguments;
		}

		public string[] Arguments { get; }

		private ITransformer Transformer { get; }

		public ContentItem Invoke(string content, PipelyneRequest request)
		{
			var transformRequest = new TransformRequest(content, this.Arguments, request);
			return this.Transformer.Transform(transformRequest);
		}
	}
}