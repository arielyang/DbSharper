using System;
using System.Configuration;

namespace DbSharper.Library.Configuration
{
	public sealed class CacheSettingCollection : ConfigurationElementCollection
	{
		#region Constructors

		public CacheSettingCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		#endregion Constructors

		#region Indexers

		public new CacheSetting this[string name]
		{
			get
			{
				return (CacheSetting)BaseGet(name);
			}
		}

		#endregion Indexers

		#region Methods

		protected override ConfigurationElement CreateNewElement()
		{
			return new CacheSetting();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((CacheSetting)element).Name;
		}

		#endregion Methods
	}
}