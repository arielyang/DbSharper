using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TextTemplating.VSHost;

namespace DbSharper.CodeGenerator
{
	[Guid("7EF57AF1-ED7A-48F6-B4A2-8BF0C4521375")]
	public sealed class DbSharperCodeGenerator : BaseCodeGeneratorWithSite
	{
		#region Methods

		public override string GetDefaultExtension()
		{
			return ".Schema.xml";
		}

		protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
		{
			throw new System.NotImplementedException();
		}

		#endregion Methods
	}
}