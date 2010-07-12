using System.IO;
using System.Reflection;

namespace DbSharper.Schema
{
	internal sealed class ResourceFileManager
	{
		#region Methods

		public string ReadResourceString(string resourceFileName)
		{
			// Return value.
			string resourceValue = string.Empty;

			//string[] names = this.GetType().Assembly.GetManifestResourceNames();

			// Get current assembly.
			Assembly assembly = this.GetType().Assembly;

			// Get current assembly name.
			string assemblyName = this.GetType().Assembly.GetName().Name;

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