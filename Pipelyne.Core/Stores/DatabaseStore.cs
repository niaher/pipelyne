namespace Pipelyne.Core
{
	using System;
	using System.Configuration;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;
	using Newtonsoft.Json;

	public class DatabaseStore : IStore
	{
		private enum RequestType
		{
			Metadata,
			Data
		}

		public string Name => "db";

		public ContentItem GetContent(string id, bool throwExceptionIfNotFound)
		{
			return new DatabaseQuery(id).Execute();
		}

		private abstract class ItemCollection
		{
			private static readonly ItemCollection StoredProcedure = new StoredProcedureCollection();
			private static readonly ItemCollection Table = new TableCollection();
			private static readonly ItemCollection View = new ViewCollection();

			private ItemCollection(string name)
			{
				this.Name = name;
			}

			public string Name { get; }

			public static ItemCollection TryParse(string collectionName)
			{
				string normalizedCollectionName = collectionName.ToLower();
				switch (normalizedCollectionName)
				{
					case "tables":
					case "table":
						return Table;
					case "views":
					case "view":
						return View;
					case "storedprocedures":
					case "storedprocedure":
						return StoredProcedure;
					default:
						throw new ArgumentOutOfRangeException(nameof(collectionName));
				}
			}

			public abstract DataTable GetData(SqlConnection connection, string item);

			private class TableCollection : ItemCollection
			{
				public TableCollection() : base("Tables")
				{
				}

				public override DataTable GetData(SqlConnection connection, string item)
				{
					var adapter = new SqlDataAdapter("select * from [" + item + "]", connection);
					var data = new DataTable();
					adapter.Fill(data);

					return data;
				}
			}

			private class ViewCollection : ItemCollection
			{
				public ViewCollection() : base("Views")
				{
				}

				public override DataTable GetData(SqlConnection connection, string item)
				{
					var adapter = new SqlDataAdapter("select * from [" + item + "]", connection);
					var data = new DataTable();
					adapter.Fill(data);

					return data;
				}
			}

			private class StoredProcedureCollection : ItemCollection
			{
				public StoredProcedureCollection() : base("StoredProcedures")
				{
				}

				public override DataTable GetData(SqlConnection connection, string item)
				{
					throw new NotImplementedException();

				}
			}
		}

		private class DatabaseQuery
		{
			public DatabaseQuery(string query)
			{
				var parts = query.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

				this.ConnectionStringName = parts[0];
				this.Collection = ItemCollection.TryParse(parts.TryGet(1));
				this.Item = parts.TryGet(2)?.Split(':')[0];
				this.RequestType = query.Contains(":data") ? RequestType.Data : RequestType.Metadata;
			}

			private string Item { get; }
			private ItemCollection Collection { get; }
			private string ConnectionStringName { get; }
			private RequestType RequestType { get; }

			public ContentItem Execute()
			{
				using (var connection = this.GetConnection())
				{
					connection.Open();

					return this.RequestType == RequestType.Metadata
						? this.GetMetadata(connection)
						: this.GetData(connection);
				}
			}

			private ContentItem GetData(SqlConnection connection)
			{
				var adapter = new SqlDataAdapter("select * from [" + this.Item + "]", connection);
				var data = new DataTable();
				adapter.Fill(data);

				return ToContentItem(data);
			}

			private static ContentItem ToContentItem(DataTable data)
			{
				var json = JsonConvert.SerializeObject(data);
				return new ContentItem(json, "application/json");
			}

			private ContentItem GetMetadata(DbConnection connection)
			{
				var schema = this.Collection == null ? connection.GetSchema() : connection.GetSchema(this.Collection.Name);
				return ToContentItem(schema);
			}

			private SqlConnection GetConnection()
			{
				var connectionString = ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString;
				return new SqlConnection(connectionString);
			}
		}
	}
}