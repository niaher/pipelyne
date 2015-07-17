namespace Pipelyne.Core
{
	public class WebpageTransformer : ITransformer
	{
		public string Name
		{
			get
			{
				return "webpage";
			}
		}

		public ContentItem Transform(string input)
		{
			string html = @"
<!DOCTYPE html>
<html lang='en'>
  <head>
    <meta charset='utf-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1'>
    <title></title>
    <link href='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css' rel='stylesheet'>
  </head>
  <body>
    <div class='container' style='margin-top:25px; margin-bottom:25px'>" + input + @"</div>
  </body>
</html>";
			return new ContentItem(html, "text/html");
		}
	}
}