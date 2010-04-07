using System.IO;
using System.Reflection;

namespace DbSharper2.Schema.Utility
{
	public sealed class ResourceFileManager
	{
		#region Fields

		private Assembly assembly;

		#endregion Fields

		#region Constructors

		public ResourceFileManager(Assembly assembly)
		{
			this.assembly = assembly;
		}

		#endregion Constructors

		#region Methods

		public string ReadResourceAsString(string resourceFileName)
		{
			// Return value.
			string resourceValue;

			//string[] names = assembly.GetManifestResourceNames();

			// Get current assembly name.
			string assemblyName = assembly.GetName().Name;

			// Get resource value (text, string).
			using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(assemblyName + "." + resourceFileName)))
			{
				resourceValue = sr.ReadToEnd();
			}

			return resourceValue;
		}

		#endregion Methods
	}
}