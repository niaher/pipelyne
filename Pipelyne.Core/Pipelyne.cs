namespace Pipelyne.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class Pipelyne
	{
		private readonly Dictionary<string, IStore> stores = new Dictionary<string, IStore>();
		private readonly Dictionary<string, ITransformer> transformers = new Dictionary<string, ITransformer>();

		public Pipelyne()
		{
			this.RegisterStore(new UrlStore());
			this.RegisterStore(new DatabaseStore());
			this.RegisterTransformer(new TextTransformer());
			this.RegisterTransformer(new MarkdownTransformer());
			this.RegisterTransformer(new WebpageTransformer());
			this.RegisterTransformer(new CodeTransformer());
		}

		public IEnumerable<ITransformer> Transformers
		{
			get
			{
				return this.transformers.Select(t => t.Value).ToList();
			}
		}

		public IEnumerable<IStore> Stores
		{
			get
			{
				return this.stores.Select(t => t.Value).ToList();
			}
		}

		private void RegisterTransformer(ITransformer transformer)
		{
			this.transformers.Add(transformer.Name, transformer);
		}

		private void RegisterStore(IStore store)
		{
			this.stores.Add(store.Name, store);
		}

		public IStore GetStore(string name, bool throwExceptionIfNotFound)
		{
			string normalizedName = name.ToLower();

			var store = this.stores[normalizedName];

			if (store == null && throwExceptionIfNotFound)
			{
				string message = string.Format("Store '{0}' was not found.", name);
				throw new ArgumentException(message);
			}

			return store;
		}

		public ITransformer GetTransform(string name, bool throwExceptionIfNotFound)
		{
			string normalizedName = name.ToLower();

			var store = this.transformers[normalizedName];

			if (store == null && throwExceptionIfNotFound)
			{
				var message = string.Format("Transform '{0}' was not found.", name);
				throw new ArgumentException(message);
			}

			return store;
		}

		/// <summary>
		/// Gets list of <see cref="ITransformer"/> instances based on the comma-delimitted list of their names 
		/// (as specified in <see cref="ITransformer.Name"/>). If any of the transformers cannot be found,
		/// then an exception is thrown.
		/// </summary>
		/// <param name="to">Comma-delimitted list of transformer names (i.e. - <see cref="ITransformer.Name"/>).</param>
		/// <returns>List of <see cref="ITransformer"/> instances.</returns>
		/// <exception cref="ArgumentException">Thrown if any of the transformers specified in the parameter cannot be found.</exception>
		public IList<ITransformer> GetTransforms(string to)
		{
			if (string.IsNullOrWhiteSpace(to))
			{
				return new List<ITransformer>();
			}

			var names = to.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

			var result = new List<ITransformer>(names.Length);

			foreach (var name in names)
			{
				var transform = this.GetTransform(name, true);
				result.Add(transform);
			}

			return result;
		}
	}
}