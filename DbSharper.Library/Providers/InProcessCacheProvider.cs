using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Web;
using System.Web.Caching;

namespace DbSharper.Library.Providers
{
	/// <summary>
	/// Cache provider using System.Web.HttpRuntime.Cache.
	/// </summary>
	internal class InProcessCacheProvider : CacheProvider
	{
		#region Fields

		private const string providerDescription = "In-process cache provider.";
		private const string providerName = "InProcessCacheProvider";

		#endregion Fields

		#region Properties

		public override string Description
		{
			get { return providerDescription; }
		}

		public override string Name
		{
			get { return providerName; }
		}

		#endregion Properties

		#region Methods

		public override object Get(string key)
		{
			return HttpRuntime.Cache.Get(key);
		}

		public override void Initialize(string name, NameValueCollection config)
		{
			// Verify that config isn't null
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}

			// Assign the provider a default name if it doesn't have one
			if (String.IsNullOrEmpty(name))
			{
				name = providerName;
			}

			// Add a default "description" attribute to config if the
			// attribute doesn't exist or is empty
			if (string.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", providerDescription);
			}

			// Call the base class's Initialize method
			base.Initialize(name, config);

			// Throw an exception if unrecognized attributes remain
			if (config.Count > 0)
			{
				string attr = config.GetKey(0);

				if (!String.IsNullOrEmpty(attr))
				{
					throw new ProviderException("Unrecognized attribute: " + attr);
				}
			}
		}

		public override void Insert(string key, object value, int duration)
		{
			HttpRuntime.Cache.Insert(key, value, null, DateTime.Now.AddSeconds(duration), Cache.NoSlidingExpiration);
		}

		public override void Remove(string key)
		{
			HttpRuntime.Cache.Remove(key);
		}

		public override void RemoveBatch(string tag)
		{
			string key;

			foreach (DictionaryEntry entry in HttpRuntime.Cache)
			{
				key = entry.Key.ToString();

				if (key.StartsWith(tag, StringComparison.Ordinal))
				{
					HttpRuntime.Cache.Remove(key);
				}
			}
		}

		#endregion Methods
	}
}