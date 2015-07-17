namespace Pipelyne.Core
{
	using System;
	using System.Configuration;
	using System.Data;
	using System.Data.SqlClient;
	using Newtonsoft.Json;

	public class DatabaseStore : IStore
	{
		public ContentItem GetContent(string id, bool throwExceptionIfNotFound)
		{
			var source = GetItemDetails(id);

			var connectionString = ConfigurationManager.ConnectionStrings[source.ConnectionStringName].ConnectionString;
			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();

				if (source.RequestType == RequestType.Metadata)
				{
					return GetMetadata(source, connection);
				}

				return GetData(connection, source.Item);
			}
		}

		public string Name
		{
			get
			{
				return "db";
			}
		}

		private static ContentItem GetData(SqlConnection connection, string itemName)
		{
			var adapter = new SqlDataAdapter("select * from [" + itemName + "]", connection);
			var data = new DataTable();
			adapter.Fill(data);
			var json = JsonConvert.SerializeObject(data);

			return new ContentItem(json, "application/json");
		}

		private static ContentItem GetMetadata(DatabaseItemDetails source, SqlConnection connection)
		{
			var schema = string.IsNullOrWhiteSpace(source.Item) ? connection.GetSchema() : connection.GetSchema(source.Item);
			var json = JsonConvert.SerializeObject(schema);

			return new ContentItem(json, "application/json");
		}

		private static DatabaseItemDetails GetItemDetails(string id)
		{
			var parts = id.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

			return new DatabaseItemDetails
			{
				ConnectionStringName = parts[0],
				Item = parts.TryGet(1).Split(':')[0],
				RequestType = id.Contains(":data") ? RequestType.Data : RequestType.Metadata
			};
		}

		private class DatabaseItemDetails
		{
			public string ConnectionStringName { get; set; }
			public string Item { get; set; }
			public RequestType RequestType { get; set; }
		}

		private enum RequestType
		{
			Metadata,
			Data
		}
	}
}