using System.Configuration.Provider;
using System.Text;
using System.Web.Configuration;

using DbSharper.Library.Configuration;
using DbSharper.Library.Providers;

namespace DbSharper.Library.Caching
{
	/// <summary>
	/// Caching service.
	/// </summary>
	public sealed class CachingService
	{
		#region Fields

		private const string internalKeyPrefix = "_DbSharper_DAL_";

		private static CacheSettingsSection cacheSettingsSection;
		private static CacheProvider defaultProvider;
		private static object lockObject = new object();
		private static CacheProviderCollection providers;

		private int defaultDuration;
		private bool enableCache;
		private string internalKey;
		private CacheProvider provider = null;
		private CacheSetting setting;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Constructor, initialize a caching service object.
		/// </summary>
		/// <param name="key">Cache key.</param>
		/// <param name="parms">Parameters.</param>
		public CachingService(string key, params string[] parms)
		{
			LoadProviders();

			setting = cacheSettingsSection.Caches[key];

			if (setting == null) // No setting, use default setting.
			{
				enableCache = cacheSettingsSection.Enabled;

				if (!enableCache)
				{
					return;
				}

				defaultDuration = cacheSettingsSection.DefaultDuration;
				provider = defaultProvider;
			}
			else
			{
				enableCache = cacheSettingsSection.Enabled ? setting.Enabled : false;

				if (!enableCache)
				{
					return;
				}

				defaultDuration = setting.Duration;

				if (!string.IsNullOrEmpty(setting.Provider))
				{
					provider = providers[setting.Provider];
				}

				if (provider == null)
				{
					provider = defaultProvider;
				}
			}

			internalKey = GetInternalKey(key, parms);
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		/// Get value from cache.
		/// </summary>
		/// <returns>Cache value.</returns>
		public object Get()
		{
			if (enableCache)
			{
				return provider.Get(internalKey);
			}

			return null;
		}

		/// <summary>
		/// Insert a value to cache.
		/// </summary>
		/// <param name="value">Cache value.</param>
		public void Insert(object value)
		{
			Insert(value, defaultDuration);
		}

		/// <summary>
		/// Insert a value to cache with duration.
		/// </summary>
		/// <param name="value">Cache value.</param>
		/// <param name="duration">Cache duration, by second.</param>
		public void Insert(object value, int duration)
		{
			if (value == null)
			{
				return;
			}

			if (enableCache)
			{
				provider.Insert(internalKey, value, duration);
			}
		}

		/// <summary>
		/// Remove cache by key.
		/// </summary>
		public void Remove()
		{
			if (enableCache)
			{
				provider.Remove(internalKey);
			}
		}

		/// <summary>
		/// Remove cache batch by key pattern.
		/// </summary>
		public void RemoveBatch()
		{
			if (enableCache)
			{
				provider.RemoveBatch(internalKey);
			}
		}

		/// <summary>
		/// Build a internal key according to the key and parameters.
		/// </summary>
		/// <param name="key">Key of method.</param>
		/// <param name="parms">Method paramters</param>
		/// <returns>Internal key.</returns>
		private static string GetInternalKey(string key, string[] parms)
		{
			StringBuilder sb = new StringBuilder(64);

			sb.Append(internalKeyPrefix);
			sb.Append(key);

			foreach (string parm in parms)
			{
				sb.Append('_');
				sb.Append(parm);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Load all cache providers.
		/// </summary>
		private static void LoadProviders()
		{
			// Avoid claiming lock if providers are already loaded
			if (defaultProvider == null)
			{
				lock (lockObject)
				{
					// Do this again to make sure defaultProvider is still null
					if (defaultProvider == null)
					{
						// Get a reference to the <cachingService> section
						cacheSettingsSection = (CacheSettingsSection)WebConfigurationManager.GetSection("dbSharper/cachingService/cacheSettings");

						// Get a reference to the <cachingService> section
						CacheProvidersSection cacheProvidersSection = (CacheProvidersSection)WebConfigurationManager.GetSection("dbSharper/cachingService/providers");

						// Load registered providers and point provider
						// to the default provider
						providers = new CacheProviderCollection();

						ProvidersHelper.InstantiateProviders(cacheProvidersSection.Providers, providers, typeof(CacheProvider));

						defaultProvider = providers[cacheProvidersSection.DefaultProvider];

						if (defaultProvider == null)
						{
							throw new ProviderException("Unable to load default CacheProvider.");
						}
					}
				}
			}
		}

		#endregion Methods
	}
}