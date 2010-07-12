using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;

using DbSharper.Schema;
using DbSharper.Schema.Code;

namespace DbSharper.CodeGenerator
{
	[Guid("7EF57AF1-ED7A-48F6-B4A2-8BF0C4521375")]
	public class DbSharperCodeGenerator : VsMultipleFileGenerator<FileItem>
	{
		#region Fields

		private FormProcessing formProcessing;
		private bool generatedSuccessfully;
		private string mappingContent;
		private int processBarStep;

		#endregion Fields

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

		#region Properties

		private string MappingName
		{
			get
			{
				return Path.GetFileNameWithoutExtension(this.InputFilePath);
			}
		}

		#endregion Properties

		#region Methods

		public override byte[] GenerateContent(FileItem item)
		{
			string codeContent;

			StringWriter outputWriter = null;

			try
			{
				outputWriter = new StringWriter();

				XmlDocument sourceDocument = new XmlDocument();
				sourceDocument.LoadXml(mappingContent);

				XsltArgumentList args = new XsltArgumentList();
				args.AddParam("defaultNamespace", string.Empty, base.DefaultNamespace + "." + this.MappingName);
				args.AddParam("namespace", string.Empty, item.Name);

				XslCompiledTransform transformer;

				transformer = new XslCompiledTransform();
				transformer.Load(item.TemplateFilePath, new XsltSettings(false, true), new XmlUrlResolver());
				transformer.Transform(sourceDocument, args, outputWriter);
			}
			catch (Exception ex)
			{
				outputWriter.WriteLine("/*");
				outputWriter.WriteLine("\tERROR: Unable to generate output for template:");
				outputWriter.WriteLine("\t'{0}'", this.InputFilePath);
				outputWriter.WriteLine("\tUsing template:");
				outputWriter.WriteLine("\t'{0}'", item.TemplateFilePath);
				outputWriter.WriteLine("");
				outputWriter.WriteLine(ex.ToString());
				outputWriter.WriteLine("*/");
			}
			finally
			{
				codeContent = outputWriter.ToString();

				outputWriter.Close();
			}

			return Encoding.UTF8.GetBytes(codeContent);
		}

		public override byte[] GenerateSummaryContent()
		{
			formProcessing.ShowLogMessage(
				string.Format(
					"Update file {0}{1}.",
					MappingName,
					GetDefaultExtension(),
					CultureInfo.InvariantCulture
					),
				0,
				IconKey.Xml
				);

			if (generatedSuccessfully)
			{
				formProcessing.ShowLogMessage("Generating successfully.", 100, IconKey.Success);
			}
			else
			{
				formProcessing.ShowLogMessage("Generating failed.", 100, IconKey.Failure);
			}

			formProcessing.buttonOk.Enabled = true;
			formProcessing.buttonOk.Focus();

			formProcessing.Update();

			return Encoding.UTF8.GetBytes(mappingContent);
		}

		public override string GetDefaultExtension()
		{
			return ".Output.xml";
		}

		public override IEnumerator<FileItem> GetEnumerator()
		{
			if (formProcessing != null)
			{
				formProcessing.Close();
			}

			formProcessing = new FormProcessing();
			formProcessing.Show();
			formProcessing.Update();

			formProcessing.ShowLogMessage("Start generating.", IconKey.Start);

			List<FileItem> list = new List<FileItem>();

			try
			{
				formProcessing.ShowLogMessage("Getting database schema.", IconKey.Schema);

				formProcessing.Cursor = Cursors.WaitCursor;

				Mapping newMapping = MappingFactory.CreateMapping(this.InputFilePath, this.InputFileContents);
				Mapping oldMapping = GetLocalMapping(newMapping);

				formProcessing.Cursor = Cursors.Default;

				formProcessing.ShowLogMessage("Generating mapping.", IconKey.Dbsx);

				list.Add(new FileItem(this.MappingName, ItemType.CacheSettingTemplate, !Equalizer.IsEqual(oldMapping.DataAccessNamespaces, newMapping.DataAccessNamespaces)));
				list.Add(new FileItem(this.MappingName, ItemType.ConnectionStrings, !Equalizer.IsEqual(oldMapping.ConnectionStringName, newMapping.ConnectionStringName)));
				list.Add(new FileItem(this.MappingName, ItemType.Document, !Equalizer.IsEqual(oldMapping.Database, newMapping.Database)));
				list.Add(new FileItem(this.MappingName, ItemType.Enums, !Equalizer.IsEqual(oldMapping.Enums, newMapping.Enums)));

				foreach (ModelNamespace ns in newMapping.ModelNamespaces)
				{
					if (ns.Models.Count > 0)
					{
						if (oldMapping.ModelNamespaces.Contains(ns.Name))
						{
							list.Add(new FileItem(this.MappingName, ns.Name, ItemType.Models, !Equalizer.IsEqual(oldMapping.ModelNamespaces[ns.Name], ns)));
						}
						else
						{
							list.Add(new FileItem(this.MappingName, ns.Name, ItemType.Models, true));
						}
					}
				}

				foreach (DataAccessNamespace ns in newMapping.DataAccessNamespaces)
				{
					if (ns.DataAccesses.Count > 0)
					{
						if (oldMapping.DataAccessNamespaces.Contains(ns.Name))
						{
							list.Add(new FileItem(this.MappingName, ns.Name, ItemType.DataAccess, !Equalizer.IsEqual(oldMapping.DataAccessNamespaces[ns.Name], ns)));
						}
						else
						{
							list.Add(new FileItem(this.MappingName, ns.Name, ItemType.DataAccess, true));
						}
					}
				}

				mappingContent = Serializer.Serialize(newMapping);

				generatedSuccessfully = true;

				processBarStep = (100 - formProcessing.progressBarGenerating.Value) / list.Count;
			}
			catch (Exception ex)
			{
				formProcessing.Cursor = Cursors.Default;

				base.LogException(ex);

				mappingContent = ex.Message;

				generatedSuccessfully = false;

				formProcessing.ShowLogMessage("Error: " + ex.Message, IconKey.Error);

				list.Clear();
			}

			return list.GetEnumerator();
		}

		protected override string GetFileName(FileItem item)
		{
			return item.FileName;
		}

		protected override void OnError(Exception ex)
		{
			formProcessing.ShowLogMessage("Error: " + ex.Message, 100, IconKey.Error);

			formProcessing.buttonOk.Enabled = true;

			formProcessing.Update();
		}

		protected override void OnFileGenerated(string filePath, bool isNewAdded)
		{
			string fileName = Path.GetFileName(filePath);

			string action = isNewAdded ? "Add" : "Update";

			formProcessing.ShowLogMessage(
				string.Format("{0} file {1}.", action, fileName),
				processBarStep,
				Path.GetExtension(fileName).TrimStart('.'));
		}

		private Mapping GetLocalMapping(Mapping newMapping)
		{
			string schemaFile = Path.Combine(Path.GetDirectoryName(base.InputFilePath), MappingName + GetDefaultExtension());

			Mapping mapping;

			try
			{
				mapping = (Mapping)Serializer.Deserialize(typeof(Mapping), File.ReadAllText(schemaFile));
			}
			catch (InvalidOperationException)
			{
				mapping = newMapping;
			}

			return mapping;
		}

		private void LaunchUpdater()
		{
			string executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			string updaterPath = Path.Combine(executingDirectory, "DbSharper.Updater.exe");

			Process.Start(updaterPath);
		}

		#endregion Methods
	}
}