namespace Pipelyne.Core
{
	internal class TableTransformer : ITransformer
	{
		public string Name => "table";

		public ContentItem Transform(TransformRequest request)
		{
			string html = @"
<link href='//cdn.datatables.net/1.10.7/css/jquery.dataTables.min.css' rel='stylesheet' />
<table id='mytable' class='table'>
	<thead></thead>
	<tbody></tbody>
</table>
<script src='//code.jquery.com/jquery-2.1.4.min.js'></script>
<script src='//cdn.datatables.net/1.10.7/js/jquery.dataTables.min.js'></script>
<script>
$(document).ready(function(){
	function tableCreate(el, data)
	{
		var tbl = el;
		
		var thead = el.tHead;
		var tbody = el.getElementsByTagName('tbody')[0];

		var firstData = data[0];
		var tr = thead.insertRow();

		for (var property in firstData) {
			if (firstData.hasOwnProperty(property)) {
				var td = tr.insertCell();
				td.appendChild(document.createTextNode(property || ''));
			}
		}

		for (var i = 0; i < data.length; ++i)
		{
			var tr = tbody.insertRow();

			for (var property in data[i]) {
				if (data[i].hasOwnProperty(property)) {
					var td = tr.insertCell();
					td.appendChild(document.createTextNode((data[i][property] || '').toString()));
				}
			}
		}
	}

	var data = " + request.Input + @";
	tableCreate(document.getElementById('mytable'), data);

	$('#mytable').DataTable();
});

</script>
";

			return new ContentItem(html, "text/html");
		}
	}
}