namespace Pipelyne.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class Pipelyne
	{
		private readonly Dictionary<string, Store> stores = new Dictionary<string, Store>();
		private readonly Dictionary<string, ITransformer> transformers = new Dictionary<string, ITransformer>();

		public Pipelyne()
		{
			this.RegisterStore(new UrlStore());
			this.RegisterStore(new DatabaseStore());
			this.RegisterTransformer(new TextTransformer());
			this.RegisterTransformer(new MarkdownTransformer());
			this.RegisterTransformer(new WebpageTransformer());
			this.RegisterTransformer(new CodeTransformer());
			this.RegisterTransformer(new TableTransformer());
		}

		public IEnumerable<Store> Stores
		{
			get
			{
				return this.stores.Select(t => t.Value).ToList();
			}
		}

		public IEnumerable<ITransformer> Transformers
		{
			get
			{
				return this.transformers.Select(t => t.Value).ToList();
			}
		}

		private Store GetStore(string name, bool throwExceptionIfNotFound)
		{
			if (name == null)
			{
				throw new ArgumentNullException(nameof(name), "Name of store hasn't been supplied.");
			}

			string normalizedName = name.ToLower();

			var store = this.stores[normalizedName];

			if (store == null && throwExceptionIfNotFound)
			{
				string message = string.Format("Store '{0}' was not found.", name);
				throw new ArgumentException(message);
			}

			return store;
		}

		/// <summary>
		/// Gets list of <see cref="ITransformer"/> instances based on the comma-delimited list of their names 
		/// (as specified in <see cref="ITransformer.Name"/>). If any of the transformers cannot be found,
		/// then an exception is thrown.
		/// </summary>
		/// <param name="to">Comma-delimited list of transformer names (i.e. - <see cref="ITransformer.Name"/>).</param>
		/// <returns>List of <see cref="ITransformer"/> instances.</returns>
		/// <exception cref="ArgumentException">Thrown if any of the transformers specified in the parameter cannot be found.</exception>
		private List<Transform> GetTransforms(string to)
		{
			if (string.IsNullOrWhiteSpace(to))
			{
				return new List<Transform>();
			}

			var names = to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

			var result = new List<Transform>(names.Length);

			foreach (var name in names)
			{
				var query = new TransformQuery(name);

				var transform = this.GetTransform(query.TransformerName, true);

				var invocation = new Transform(transform, query.Arguments);

				result.Add(invocation);
			}

			return result;
		}

		/// <summary>
		/// Processes request and returns final result.
		/// </summary>
		/// <param name="request"><see cref="PipelyneRequest"/> instance.</param>
		/// <returns><see cref="ContentItem"/> instance.</returns>
		public ContentItem ProcessRequest(PipelyneRequest request)
		{
			var store = this.GetStore(request.Source, true);
			var content = store.GetContent(request.Id, true);

			var transforms = this.GetTransforms(request.To);

			foreach (var transform in transforms)
			{
				content = transform.Invoke(content.Content, request);
			}

			return content;
		}

		/// <summary>
		/// Register new <see cref="Store"/>.
		/// </summary>
		/// <param name="store"><see cref="Store"/> instance.</param>
		public void RegisterStore(Store store)
		{
			this.stores.Add(store.Name, store);
		}

		/// <summary>
		/// Register new <see cref="ITransformer"/>.
		/// </summary>
		/// <param name="transformer"><see cref="ITransformer"/> instance.</param>
		public void RegisterTransformer(ITransformer transformer)
		{
			this.transformers.Add(transformer.Name, transformer);
		}

		private ITransformer GetTransform(string transformerName, bool throwExceptionIfNotFound)
		{
			string normalizedName = transformerName.ToLower();
			
			var transformer = this.transformers[normalizedName];

			if (transformer == null && throwExceptionIfNotFound)
			{
				var message = $"Transformer '{transformerName}' was not found.";
				throw new ArgumentException(message);
			}

			return transformer;
		}
	}
}