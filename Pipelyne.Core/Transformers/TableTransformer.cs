namespace Pipelyne.Core
{
	internal class TableTransformer : ITransformer
	{
		public string Name
		{
			get
			{
				return "table";
			}
		}

		public ContentItem Transform(string input)
		{
			string html = @"
<div id='mytable'></div>
<script>
function tableCreate(el, data)
{
    var tbl = document.createElement('table');
	tbl.className = 'table';

    for (var i = 0; i < data.length; ++i)
    {
        var tr = tbl.insertRow();

		for (var property in data[i]) {
			if (data[i].hasOwnProperty(property)) {
				var td = tr.insertCell();
				td.appendChild(document.createTextNode(data[i][property].toString()));
			}
		}
    }
    el.appendChild(tbl);
}
var data = " + input + @";
tableCreate(document.getElementById('mytable'), data);
</script>";

			return new ContentItem(html, "text/html");
		}
	}
}