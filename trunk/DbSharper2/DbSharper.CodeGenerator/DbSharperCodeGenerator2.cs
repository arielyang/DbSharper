using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using EnvDTE;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextTemplating.VSHost;

using VSLangProj;

namespace DbSharper2.CodeGenerator
{
	[Guid("F6035BCF-3416-4461-9ACC-2ECD505A368E")]
	public sealed class DbSharperCodeGenerator2 : BaseCodeGeneratorWithSite
	{
		#region Fields

		private Project project;
		private ProjectItem projectItem;

		#endregion Fields

		#region Constructors

		public DbSharperCodeGenerator2()
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
			return ".Mapping.xml";
		}

		protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
		{
			this.project = GetProject();

			this.projectItem = GetService(typeof(ProjectItem)) as ProjectItem;

			if (projectItem == null)
			{
				throw new ApplicationException("Unable to retrieve Visual Studio ProjectItem");
			}

			List<string> references = GetReferences();
			List<string> fileNames = GetFileNames();

			GeneratorEngine engine = new GeneratorEngine(inputFileName, inputFileContent, GetDefaultExtension(), this.FileNamespace, references, fileNames);

			engine.BeforeFileItemAdded = CheckOutFile;
			engine.AfterFileItemAdded = AddFile;
			engine.BeforeFileItemDeleted = fileName => projectItem.ProjectItems.Item(fileName).Delete();

			return engine.Generate();
		}

		private void AddFile(string fileName)
		{
			var item = projectItem.ProjectItems.AddFromFile(fileName);

			// Only build C# files.
			if (string.Compare(Path.GetExtension(fileName), ".cs", true) != 0)
			{
				item.Properties.Item("BuildAction").Value = 0;
			}
		}

		private void CheckOutFile(string fileName)
		{
			if (project.DTE.SourceControl.IsItemUnderSCC(fileName) &&
				!project.DTE.SourceControl.IsItemCheckedOut(fileName))
			{
				project.DTE.SourceControl.CheckOutItem(fileName);
			}
		}

		private List<string> GetFileNames()
		{
			List<string> fileNames = new List<string>();

			foreach (ProjectItem childItem in projectItem.ProjectItems)
			{
				fileNames.Add(childItem.Name);
			}

			return fileNames;
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

		private List<string> GetReferences()
		{
			List<string> referenceList = new List<string>();

			var fullPath = project.Properties.Item("FullPath").Value.ToString();
			var outputPath = project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();
			var outputFileName = project.Properties.Item("OutputFileName").Value.ToString();

			var path = Path.Combine(Path.Combine(fullPath, outputPath), outputFileName);

			referenceList.Add(path);

			var vsProject = this.project.Object as VSProject;

			var references = vsProject.References;

			foreach (Reference reference in references)
			{
				if (reference.Type == prjReferenceType.prjReferenceTypeActiveX)
				{
					continue;
				}

				if (reference.Identity == "mscorlib" ||
					reference.Identity == "System" ||
					reference.Identity.StartsWith("System.")
					)
				{
					continue;
				}

				referenceList.Add(reference.Path);
			}

			return referenceList;
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