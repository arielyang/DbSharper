using System.IO;
using System.Reflection;

namespace DbSharper.Schema.Provider
{
	public static class SchemaProviderFactory
	{
		#region Methods

		public static SchemaProviderBase Create(string providerName)
		{
			return null;
		}

		private static string[] GetAssemblies()
		{
			string executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			string[] assemblies = Directory.GetFiles(executingDirectory, "DbSharper.Data.*.dll");

			return assemblies;
		}

		#endregion Methods
	}
}