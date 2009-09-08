namespace DbSharper.Schema
{
    using System;

    public class ProviderNameAttribute : Attribute
    {
        #region Fields

        private string providerName;

        #endregion Fields

        #region Constructors

        public ProviderNameAttribute(string providerName)
        {
            this.providerName = providerName;
        }

        #endregion Constructors

        #region Properties

        public string ProviderName
        {
            get { return providerName; }
        }

        #endregion Properties
    }
}