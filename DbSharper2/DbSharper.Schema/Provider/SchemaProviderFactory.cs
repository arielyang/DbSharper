using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace DbSharper.Schema.Provider
{
	public static class SchemaProviderFactory
	{
		#region Fields

		private static Type typeCache;

		#endregion Fields

		#region Methods

		public static SchemaProviderBase Create(string providerName)
		{
			if (typeCache == null)
			{
				typeCache = GetProviderType(providerName);
			}

			SchemaProviderBase provider = (SchemaProviderBase)Activator.CreateInstance(typeCache);

			return provider;
		}

		private static string[] GetProviderAssemblies()
		{
			string executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			string[] assemblies = Directory.GetFiles(executingDirectory, "*.Provider.*.dll");

			return assemblies;
		}

		private static Type GetProviderType(string providerName)
		{
			string[] assemblies = GetProviderAssemblies();

			Collection<Type> types = GetProviderTypes(assemblies);

			SchemaProviderAttribute attribute;

			foreach (var type in types)
			{
				attribute = (SchemaProviderAttribute)Attribute.GetCustomAttribute(type, typeof(SchemaProviderAttribute));

				if (attribute != null &&
					string.Compare(attribute.ProviderName, providerName, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return type;
				}
			}

			// TODO: Embed string into resource file later.
			throw new DbSharperException(string.Format(CultureInfo.InvariantCulture, "Can not find provider for {0}", providerName));
		}

		private static Collection<Type> GetProviderTypes(string[] assemblyFiles)
		{
			Collection<Type> typeCol = new Collection<Type>();
			Assembly assembly;
			Type[] types;

			foreach (var assemblyFile in assemblyFiles)
			{
				assembly = Assembly.LoadFile(assemblyFile);

				types = assembly.GetTypes();

				foreach (var type in types)
				{
					if (type.BaseType == typeof(SchemaProviderBase))
					{
						typeCol.Add(type);
					}
				}
			}

			return typeCol;
		}

		#endregion Methods
	}
}