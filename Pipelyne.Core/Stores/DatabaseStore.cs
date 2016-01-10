namespace Pipelyne.Core
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;
	using global::Pipelyne.Core.Parsing;
	using Newtonsoft.Json;

	// Sample queries:
	//   mydb/user:data
	//   mydb/table/user:data

	public class DatabaseStore : Store
	{
		private enum RequestType
		{
			Metadata,
			// ReSharper disable once UnusedMember.Local
			Data
		}

		public override string Name => "db";

		public override IReadOnlyList<Parameter> Parameters => new List<Parameter>
		{
			ParameterList.ConnectionStringName,
			ParameterList.Collection,
			ParameterList.Item,
			ParameterList.Request
		};

		public override ContentItem GetContent(IReadOnlyDictionary<string, Argument> invocation, bool throwExceptionIfNotFound)
		{
			return new DatabaseQuery(invocation).Execute();
		}

		private static class ParameterList
		{
			public static readonly Parameter Collection = new Parameter("collection");
			public static readonly Parameter ConnectionStringName = new Parameter("connectionStringName");
			public static readonly Parameter Item = new Parameter("item");
			public static readonly Parameter Request = new Parameter("requestType");
		}

		private class DatabaseQuery
		{
			public DatabaseQuery(IReadOnlyDictionary<string, Argument> invocation)
			{
				this.ConnectionStringName = invocation[ParameterList.ConnectionStringName.Name].Value;
				this.Collection = ItemCollection.TryParse(invocation[ParameterList.Collection.Name].Value);
				this.Item = invocation[ParameterList.Item.Name].Value;
				this.RequestType = invocation[ParameterList.Request.Name].AsEnum<RequestType>();
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
						return Table;
					case "views":
						return View;
					case "sprocs":
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
	}
}