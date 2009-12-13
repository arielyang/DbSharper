using System;
using System.Configuration;
using System.Globalization;
using System.IO;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema
{
	internal static class MappingHelper
	{
		#region Methods

		/// <summary>
		/// Get
		/// </summary>
		/// <param name="commonType"></param>
		/// <returns></returns>
		internal static string GetCommonTypeString(CommonType commonType)
		{
			switch (commonType)
			{
				case CommonType.ByteArray:
					return "Byte[]";
				case CommonType.CharArray:
					return "Char[]";
				default:
					return commonType.ToString();
			}
		}

		internal static string GetConnectionStringName(string mappingConfigFile)
		{
			return Path.GetFileNameWithoutExtension(mappingConfigFile);
		}

		internal static string GetConnectionStringProviderName(string configFile, string connectionStringName)
		{
			if (string.IsNullOrEmpty(configFile))
			{
				throw new ArgumentNullException("configFile");
			}

			if (string.IsNullOrEmpty(connectionStringName))
			{
				throw new ArgumentNullException("connectionStringName");
			}

			ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connectionStringName];

			if (settings == null)
			{
				// TODO: Embed string into resource file later.
				throw new DbSharperException(string.Format(CultureInfo.InvariantCulture, "Can not find connection string named \"{0}\".", connectionStringName));
			}

			string providerName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;

			if (string.IsNullOrEmpty(providerName))
			{
				// TODO: Embed string into resource file later.
				throw new DbSharperException(string.Format(CultureInfo.InvariantCulture, "ProviderName of connection string named \"{0}\" is null or empty.", connectionStringName));
			}

			return providerName;
		}

		internal static string GetConnectionStringValue(string configFile, string connectionStringName)
		{
			if (string.IsNullOrEmpty(configFile))
			{
				throw new ArgumentNullException("configFile");
			}

			if (string.IsNullOrEmpty(connectionStringName))
			{
				throw new ArgumentNullException("connectionStringName");
			}

			ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connectionStringName];

			if (settings == null)
			{
				// TODO: Embed string into resource file later.
				throw new DbSharperException(string.Format(CultureInfo.InvariantCulture, "Can not find connection string named \"{0}\".", connectionStringName));
			}

			string value = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

			if (string.IsNullOrEmpty(value))
			{
				// TODO: Embed string into resource file later.
				throw new DbSharperException(string.Format(CultureInfo.InvariantCulture, "Value of connection string named \"{0}\" is null or empty.", connectionStringName));
			}

			return value;
		}

		/// <summary>
		/// Get method type by method name.
		/// </summary>
		/// <param name="methodName">Method name.</param>
		/// <returns>Method type.</returns>
		internal static MethodType GetMethodType(string methodName)
		{
			return methodName.StartsWith("Get", StringComparison.OrdinalIgnoreCase) ? MethodType.ExecuteReader : MethodType.ExecuteNonQuery;
		}

		#endregion Methods
	}
}