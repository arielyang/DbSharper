using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using EnvDTE;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextTemplating.VSHost;

namespace DbSharper.CodeGenerator
{
	[Guid("7EF57AF1-ED7A-48F6-B4A2-8BF0C4521375")]
	public sealed class DbSharperCodeGenerator : BaseCodeGeneratorWithSite
	{
		#region Constructors

		public DbSharperCodeGenerator()
		{
			try
			{
				UpdateService service = new UpdateService();

				VersionInfo latestVersionInfo = service.GetLatestVersionInfo();

				//if (latestVersionInfo != VersionInfo.Null &&
				//    !UpdateService.ExecutingVersionInfo.Equals(latestVersionInfo))
				//{
				//    cancelGenerating = true;

				//    string message = UpdateService.GetNewVersionInformation(latestVersionInfo);

				//    MessageBox.Show(message, "DbSharper CodeGenerator", MessageBoxButtons.OK, MessageBoxIcon.Information);

				//    LaunchUpdater();
				//}
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		#endregion Constructors

		#region Methods

		public override string GetDefaultExtension()
		{
			return ".Schema.xml";
		}

		protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
		{
			List<string> fileNames = GetFileNames();

			GeneratorEngine engine = new GeneratorEngine(inputFileName, inputFileContent, GetDefaultExtension(), this.FileNamespace, fileNames);

			return engine.Generate();
		}

		private List<string> GetFileNames()
		{
			List<string> fileItems = new List<string>();

			ProjectItem projectItem;

			projectItem = GetService(typeof(ProjectItem)) as ProjectItem;

			if (projectItem == null)
			{
				throw new ApplicationException("Unable to retrieve Visual Studio ProjectItem");
			}

			foreach (ProjectItem childItem in projectItem.ProjectItems)
			{
				fileItems.Add(childItem.Name);
			}

			return fileItems;
		}

		private Project GetProject()
		{
			Project project;

			DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));

			Array ary = (Array)dte.ActiveSolutionProjects;

			if (ary.Length > 0)
			{
				project = (Project)ary.GetValue(0);
			}
			else
			{
				throw new ApplicationException("Unable to retrieve Visual Studio ProjectItem");
			}

			return project;
		}

		private void LaunchUpdater()
		{
			string executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			string updaterPath = Path.Combine(executingDirectory, "DbSharper.Updater.exe");

			System.Diagnostics.Process.Start(updaterPath);
		}

		private void LogException(Exception ex)
		{
			string path = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "log");

			using (StreamWriter sw = File.AppendText(path))
			{
				sw.WriteLine(DateTime.Now);
				sw.WriteLine(new string('-', 50));
				sw.WriteLine(ex.ToString());
				sw.WriteLine();
				sw.WriteLine(ex.Message);
				sw.WriteLine();
				sw.WriteLine(ex.StackTrace);
				sw.WriteLine();
			}
		}

		#endregion Methods

		#region Other

		//private FormProcessing formProcessing;

		#endregion Other
	}
}