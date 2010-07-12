using System;
using System.Configuration.Provider;

namespace DbSharper.Library.Providers
{
	public class CacheProviderCollection : ProviderCollection
	{
		#region Indexers

		/// <summary>
		/// Gets the cache provider with the specified name.
		/// </summary>
		/// <param name="name">The key by which the cache provider is identified.</param>
		/// <returns>The cache provider with the specified name.</returns>
		public new CacheProvider this[string name]
		{
			get { return (CacheProvider)base[name]; }
		}

		#endregion Indexers

		#region Methods

		/// <summary>
		/// Adds a cache provider to the collection.
		/// </summary>
		/// <param name="provider">The cache provider to be added.</param>
		public override void Add(ProviderBase provider)
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}

			if (!(provider is CacheProvider))
			{
				throw new ArgumentException("Invalid provider type.", "provider");
			}

			base.Add(provider);
		}

		#endregion Methods
	}
}