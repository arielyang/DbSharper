using System.Configuration;

namespace DbSharper2.Library.Configuration
{
	public class CacheSettingsSection : ConfigurationSection
	{
		#region Fields

		private static readonly ConfigurationProperty propertyCaches = new ConfigurationProperty(null, typeof(CacheSettingCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
		private static readonly ConfigurationProperty propertyDefaultDuration = new ConfigurationProperty("defaultDuration", typeof(int), 60, ConfigurationPropertyOptions.None);
		private static readonly ConfigurationProperty propertyEnabled = new ConfigurationProperty("enabled", typeof(bool), false, ConfigurationPropertyOptions.None);

		private static ConfigurationPropertyCollection properties = 
			new ConfigurationPropertyCollection()
				{
					propertyCaches,
					propertyDefaultDuration,
					propertyEnabled
				};

		#endregion Fields

		#region Properties

		public CacheSettingCollection Caches
		{
			get { return (CacheSettingCollection)base[propertyCaches]; }
		}

		public int DefaultDuration
		{
			get { return (int)base[propertyDefaultDuration]; }

			set { base[propertyDefaultDuration] = value; }
		}

		public bool Enabled
		{
			get { return (bool)base[propertyEnabled]; }

			set { base[propertyEnabled] = value; }
		}

		protected override ConfigurationPropertyCollection Properties
		{
			get { return properties; }
		}

		#endregion Properties
	}
}