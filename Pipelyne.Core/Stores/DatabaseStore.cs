namespace Pipelyne.Core
{
	using System;
	using System.Configuration;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;
	using global::Pipelyne.Core.Parsing;
	using Newtonsoft.Json;

	public class DatabaseStore : IStore
	{
		private enum RequestType
		{
			Metadata,
			Data
		}

		public string Name => "db";
		public Signature Signature => MySignature.Instance;

		public ContentItem GetContent(string id, bool throwExceptionIfNotFound)
		{
			return new DatabaseQuery(id).Execute();
		}

		private class DatabaseQuery
		{
			public DatabaseQuery(string query)
			{
				var invocation = MySignature.Instance.CreateInvocation(query);
				// Sample queries:
				//   mydb/user:data
				//   mydb/table/user:data

				this.ConnectionStringName = invocation.Arguments[MySignature.ConnectionStringName.Name].Value;
				this.Collection = ItemCollection.TryParse(invocation.Arguments[MySignature.Collection.Name].Value);
				this.Item = invocation.Arguments[MySignature.Item.Name].Value;
				this.RequestType = invocation.Arguments[MySignature.Request.Name].AsEnum<RequestType>();
			}

			private ItemCollection Collection { get; }
			private string ConnectionStringName { get; }

			private string Item { get; }
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

			private static ContentItem ToContentItem(DataTable data)
			{
				var json = JsonConvert.SerializeObject(data);
				return new ContentItem(json, "application/json");
			}

			private SqlConnection GetConnection()
			{
				var connectionString = ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString;
				return new SqlConnection(connectionString);
			}

			private ContentItem GetData(SqlConnection connection)
			{
				var adapter = new SqlDataAdapter("select * from [" + this.Item + "]", connection);
				var data = new DataTable();
				adapter.Fill(data);

				return ToContentItem(data);
			}

			private ContentItem GetMetadata(DbConnection connection)
			{
				var schema = this.Collection == null ? connection.GetSchema() : connection.GetSchema(this.Collection.Name);
				return ToContentItem(schema);
			}
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
		}

		private class MySignature : Signature
		{
			public static readonly Parameter Collection = new Parameter("collection");
			public static readonly Parameter ConnectionStringName = new Parameter("connectionStringName");
			public static readonly Parameter Item = new Parameter("item");
			public static readonly Parameter Request = new Parameter("requestType");

			public static readonly MySignature Instance;

			static MySignature()
			{
				Instance = new MySignature();
			}

			private MySignature()
				: base(ConnectionStringName, Collection, Item, Request)
			{
			}
		}
	}
}