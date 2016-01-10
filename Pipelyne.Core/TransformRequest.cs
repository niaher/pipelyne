namespace Pipelyne.Core
{
	public class TransformRequest
	{
		public TransformRequest(string input, string[] arguments, PipelyneRequest pipelyneRequest)
		{
			this.Input = input;
			this.Arguments = arguments;
			this.PipelyneRequest = pipelyneRequest;
		}

		public string Input { get; }
		public string[] Arguments { get; }
		public PipelyneRequest PipelyneRequest { get; }
	}
}