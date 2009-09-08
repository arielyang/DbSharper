namespace DbSharper.Library.Configuration
{
    using System.Configuration;

    public sealed class CacheSetting : ConfigurationElement
    {
        #region Fields

        private static readonly ConfigurationProperty propertyDuration = new ConfigurationProperty("duration", typeof(int), 60, ConfigurationPropertyOptions.None);
        private static readonly ConfigurationProperty propertyEnabled = new ConfigurationProperty("enabled", typeof(bool), true, ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty propertyName = new ConfigurationProperty("name", typeof(string), null, ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty propertyProvider = new ConfigurationProperty("provider", typeof(string), null, ConfigurationPropertyOptions.None);

        private static ConfigurationPropertyCollection properties = 
            new ConfigurationPropertyCollection()
            {
                propertyName,
                propertyEnabled,
                propertyDuration,
                propertyProvider
            };

        #endregion Fields

        #region Constructors

        public CacheSetting(string name, bool enabled)
        {
            base[propertyName] = name;
            base[propertyEnabled] = enabled;
        }

        internal CacheSetting()
        {
        }

        #endregion Constructors

        #region Properties

        public int Duration
        {
            get { return (int)base[propertyDuration]; }
        }

        public bool Enabled
        {
            get { return (bool)base[propertyEnabled]; }
        }

        public string Name
        {
            get { return (string)base[propertyName]; }
        }

        public string Provider
        {
            get { return (string)base[propertyProvider]; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return properties;
            }
        }

        #endregion Properties
    }
}