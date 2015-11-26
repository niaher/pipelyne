namespace Pipelyne.Core
{
	using System.Text;

	public class ConversationTransformer : ITransformer
	{
		public string Name => "conversation";

		public ContentItem Transform(string input, TransformationRequest request)
		{
			var converationKey = GetConverationKey(request);

			string html = input + @"<hr />
<div class='panel panel-default'>
<div class='panel-body' data-conversations-key='" + converationKey + @"' data-conversations-url='https://intra.unops.org/apps/conversations/' data-conversations-app-name='pipelyne-conversation'></div>
</div>
<link href='https://intra.unops.org/Apps/Conversations/css/main' rel='stylesheet' />
<script src='//code.jquery.com/jquery-2.1.4.min.js'></script>
<script src='https://intra.unops.org/apps/conversations/scripts/app/conversations-plugin.min.js'></script><script>
	$('[data-conversations-key]').each(function (i, element) {
        var $el = $(element);
		window.conversation($el, {
			key: $el.attr('data-conversations-key'),
			appName: $el.attr('data-conversations-app-name'),
			url: $el.attr('data-conversations-url'),
			nostyle: $el.attr('data-conversations-nostyle')
	    });
	});
</script>";
            return new ContentItem(html, "text/html");
		}

		private static string GetConverationKey(TransformationRequest request)
		{
			const int MaxKeySize = 20;
            var fullkey = new StringBuilder(request.Source + request.Id).Replace("%", "p").Replace(".", "d").Replace("&", "a");

			int toRemove = 0;

			while (fullkey.Length > MaxKeySize)
			{
				fullkey = fullkey.Remove(toRemove, 1);
				toRemove += 1;

				if (toRemove >= fullkey.Length)
				{
					toRemove = 0;
				}
			}

			return fullkey.ToString();
		}
	}
}