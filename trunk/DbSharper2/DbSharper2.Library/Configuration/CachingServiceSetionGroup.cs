using System.Configuration;

namespace DbSharper2.Library.Configuration
{
	public class CachingServiceSectionGroup : ConfigurationSectionGroup
	{
		#region Fields

		private const string cacheSettings = "cacheSettings";
		private const string providers = "providers";

		#endregion Fields

		#region Properties

		[ConfigurationProperty(cacheSettings)]
		public CacheSettingsSection CacheSettings
		{
			get { return (CacheSettingsSection)base.Sections[cacheSettings]; }
		}

		[ConfigurationProperty(providers)]
		public CacheProvidersSection Providers
		{
			get { return (CacheProvidersSection)base.Sections[providers]; }
		}

		#endregion Properties
	}
}