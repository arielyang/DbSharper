using System;
using System.Text;

namespace DbSharper.Library.Providers
{
	/// <summary>
	/// Cache provider using Memcached.
	/// </summary>
	public class MemcachedCacheProvider : CacheProvider
	{
		#region Fields

		private const string providerDescription = "Memcached cache provider.";
		private const string providerName = "MemcachedCacheProvider";

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
			key = GetValidKey(key);

			return MemcachedClient.MemcachedClient.GetInstance().Get(key);
		}

		public override void Insert(string key, object value, int duration)
		{
			key = GetValidKey(key);

			MemcachedClient.MemcachedClient.GetInstance().Insert(key, value, duration);
		}

		public override void Remove(string key)
		{
			key = GetValidKey(key);

			MemcachedClient.MemcachedClient.GetInstance().Remove(key);
		}

		public override void RemoveBatch(string tag)
		{
			throw new NotImplementedException();
		}

		private static string GetValidKey(string key)
		{
			StringBuilder sb = new StringBuilder(key.Length);

			foreach (char c in key)
			{
				if (c == ' ' || c == '\r' || c == '\n' || c == '\t' || c == '\f' || c == '\v')
				{
					sb.Append('_');
				}

				sb.Append(c);
			}

			return sb.ToString();
		}

		#endregion Methods
	}
}