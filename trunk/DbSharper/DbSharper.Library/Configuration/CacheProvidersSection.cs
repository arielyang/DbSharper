namespace DbSharper.Library.Configuration
{
    using System.Configuration;
    using System.Configuration.Provider;

    public class CacheProvidersSection : ConfigurationSection
    {
        #region Fields

        private static readonly ConfigurationProperty propertyDefaultProvider = new ConfigurationProperty("defaultProvider", typeof(string), null, ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty propertyProviders = new ConfigurationProperty(null, typeof(ProviderSettingsCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

        private static ConfigurationPropertyCollection properties = 
            new ConfigurationPropertyCollection()
                {
                    propertyDefaultProvider,
                    propertyProviders
                };

        #endregion Fields

        #region Properties

        public string DefaultProvider
        {
            get { return (string)base[propertyDefaultProvider]; }
        }

        public ProviderSettingsCollection Providers
        {
            get { return (ProviderSettingsCollection)base[propertyProviders]; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return properties; }
        }

        #endregion Properties
    }
}