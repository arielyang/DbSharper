using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace DbSharper.Schema.Provider
{
	public static class SchemaProviderFactory
	{
		#region Fields

		private static string executingDirectory;
		private static FileSystemWatcher fileSystemWatcher;
		private static Dictionary<string, Type> typeCaches;

		#endregion Fields

		#region Constructors

		static SchemaProviderFactory()
		{
			executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			fileSystemWatcher = new FileSystemWatcher(executingDirectory);
			fileSystemWatcher.Changed += new FileSystemEventHandler(fileSystemWatcher_Changed);
		}

		#endregion Constructors

		#region Methods

		public static SchemaProviderBase Create(string providerName)
		{
			if (typeCaches == null)
			{
				typeCaches = new Dictionary<string, Type>();
			}

			Type providerType;

			if (typeCaches.ContainsKey(providerName))
			{
				providerType = typeCaches[providerName];
			}
			else
			{
				providerType = GetProviderType(providerName);

				typeCaches.Add(providerName, providerType);
			}

			SchemaProviderBase provider = (SchemaProviderBase)Activator.CreateInstance(providerType);

			return provider;
		}

		private static string[] GetProviderAssemblies()
		{
			string[] assemblies = Directory.GetFiles(executingDirectory, "*.dll");

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
				try
				{
					assembly = Assembly.LoadFile(assemblyFile);

					types = assembly.GetTypes();
				}
				catch
				{
					continue;
				}

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

		private static void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			typeCaches = null;
		}

		#endregion Methods
	}
}