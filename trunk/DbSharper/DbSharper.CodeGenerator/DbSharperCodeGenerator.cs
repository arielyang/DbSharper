namespace DbSharper.CodeGenerator
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Reflection;
	using System.Runtime.InteropServices;
	using System.Text;
	using System.Windows.Forms;
	using System.Xml;
	using System.Xml.Xsl;

	using DbSharper.Schema;
	using DbSharper.Schema.Code;

	[Guid("7EF57AF1-ED7A-48F6-B4A2-8BF0C4521375")]
	public class DbSharperCodeGenerator : VsMultipleFileGenerator<string>
	{
		#region Fields

		private string content;
		private FormProcessing form;
		private bool generatedSuccessfully;
		private int step;
		private string[] versionInfo;

		#endregion Fields

		#region Constructors

		public DbSharperCodeGenerator()
		{
			try
			{
				string currentVersion = UpdateService.GetExecutingVersion();

				UpdateService service = new UpdateService();

				versionInfo = service.GetLatestVersionInfo();

				if (versionInfo[0] != null)
				{
					if (currentVersion != versionInfo[0])
					{
						cancelGenerating = true;
					}
				}
			}
			catch (Exception ex)
			{
				LogException(ex);
			}
		}

		#endregion Constructors

		#region Methods

		public override byte[] GenerateContent(string element)
		{
			string templateFileName = GetTemplateFileName(element);
			string codeContent = string.Empty;

			StringWriter outputWriter = null;

			try
			{
				outputWriter = new StringWriter();

				XmlDocument sourceDocument = new XmlDocument();
				sourceDocument.LoadXml(content);

				XsltArgumentList args = new XsltArgumentList();
				args.AddParam("defaultNamespace", string.Empty, this.DefaultNamespace + "." + Path.GetFileNameWithoutExtension(this.InputFilePath));
				args.AddParam("namespace", string.Empty, GetNamespace(element));

				XslCompiledTransform transformer;

				transformer = new XslCompiledTransform();
				transformer.Load(templateFileName, new XsltSettings(false, true), new XmlUrlResolver());
				transformer.Transform(sourceDocument, args, outputWriter);
			}
			catch (Exception ex)
			{
				outputWriter.WriteLine("/*");
				outputWriter.WriteLine("\tERROR: Unable to generate output for template:");
				outputWriter.WriteLine("\t'{0}'", this.InputFilePath);
				outputWriter.WriteLine("\tUsing template:");
				outputWriter.WriteLine("\t'{0}'", templateFileName);
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
			ShowMessage(string.Format("Update file {0}{1}", GetName(), GetDefaultExtension()), 0, "xml");

			if (generatedSuccessfully)
			{
				ShowMessage("Generating successfully.", 0, "Success");
			}
			else
			{
				ShowMessage("Generating failed.", 0, "Failed");
			}

			form.progressBarGenerating.Value = 100;
			form.buttonOk.Enabled = true;
			form.buttonOk.Focus();
			form.Update();

			return Encoding.UTF8.GetBytes(content);
		}

		public override string GetDefaultExtension()
		{
			return ".Output.xml";
		}

		public override IEnumerator<string> GetEnumerator()
		{
			if (form != null)
			{
				form.Close();
			}

			form = new FormProcessing();
			form.Show();
			form.Update();

			ShowMessage("Start generating.", 10, "Start");

			List<string> list = new List<string>();

			list.Add("CacheSettingTemplate");
			list.Add("ConnectionStrings");
			list.Add("Document");
			list.Add("Enums");

			try
			{
				ShowMessage("Getting database schema.", 10, "Schema");

				Mapping mapping = MappingFactory.CreateMapping(this.InputFilePath, this.InputFileContents);

				ShowMessage("Generating mapping.", 10, "Dbsx");

				foreach (ModelNamespace ns in mapping.ModelNamespaces)
				{
					if (ns.Models.Count > 0)
					{
						list.Add("Models." + ns.Name);
					}
				}

				foreach (DataAccessNamespace ns in mapping.DataAccessNamespaces)
				{
					if (ns.DataAccesses.Count > 0)
					{
						list.Add("DataAccess." + ns.Name);
					}
				}

				content = Serializer.Serialize(mapping);

				generatedSuccessfully = true;
			}
			catch (Exception ex)
			{
				LogException(ex);

				try
				{
					content = Serializer.Serialize(ex);
				}
				catch (InvalidOperationException)
				{
					content = ex.Message;
				}

				ShowMessage("Error: " + ex.Message, 10, "Error");

				generatedSuccessfully = false;
			}

			step = (100 - form.progressBarGenerating.Value) / list.Count;

			return list.GetEnumerator();
		}

		protected override string GetFileName(string element)
		{
			string connectionStringName = Path.GetFileNameWithoutExtension(this.InputFilePath);

			switch (element)
			{
				case "CacheSettingTemplate":
					return connectionStringName + ".CacheSettingTemplate.config";
				case "Document":
					return connectionStringName + ".Document.html";
				default:
					return string.Format("{0}.{1}.cs", connectionStringName, element);
			}
		}

		protected override void OnCanceling()
		{
			base.OnCanceling();

			string message = GetLatestVersionInformation(versionInfo);

			MessageBox.Show(message, "DbSharper CodeGenerator", MessageBoxButtons.OK, MessageBoxIcon.Information);

			LaunchUpdater();
		}

		protected override void OnError(Exception ex)
		{
			base.OnError(ex);

			ShowMessage("Error: " + ex.Message, 10, "Error");

			form.progressBarGenerating.Value = 100;
			form.buttonOk.Enabled = true;
			form.Update();
		}

		protected override void OnGenerateFile(string filePath)
		{
			base.OnGenerateFile(filePath);

			string fileName = Path.GetFileName(filePath);

			string action = oldFileNames.Contains(fileName) ? "Update" : "Add";

			ShowMessage(string.Format("{0} file {1}.", action, fileName), step, GetExtension(filePath));
		}

		private string GetExtension(string fileName)
		{
			int i = fileName.LastIndexOf('.');

			if (i < 0)
			{
				return string.Empty;
			}

			return fileName.Substring(i + 1);
		}

		private string GetLatestVersionInformation(string[] versionInfo)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("New verion {0} released.", versionInfo[0]);
			sb.AppendLine();
			sb.AppendLine();
			sb.Append(versionInfo[1]);

			return sb.ToString();
		}

		private string GetName()
		{
			string fileName = Path.GetFileName(this.InputFilePath);

			return fileName.Substring(0, fileName.IndexOf('.'));
		}

		private string GetNamespace(string element)
		{
			int i = element.IndexOf('.');

			if (i < 0)
			{
				return string.Empty;
			}

			return element.Substring(i + 1);
		}

		private string GetTemplateFileName(string element)
		{
			string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string fileName;

			if (element.StartsWith("DataAccess."))
			{
				fileName = @"Templates\DbSharper.DataAccess.xslt";
			}
			else if (element.StartsWith("Models."))
			{
				fileName = @"Templates\DbSharper.Models.xslt";
			}
			else
			{
				fileName = string.Format(@"Templates\DbSharper.{0}.xslt", element);
			}

			return Path.Combine(assemblyPath, fileName);
		}

		private void LaunchUpdater()
		{
			string updaterPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DbSharper.Updater.exe");

			Process.Start(updaterPath);
		}

		private void ShowMessage(string message, int step, string iconKey)
		{
			form.listViewMessage.Items.Add(" " + message, iconKey);

			if (form.progressBarGenerating.Value + step < 100)
			{
				form.progressBarGenerating.Value += step;
			}
			else
			{
				form.progressBarGenerating.Value = 100;
			}

			form.Update();
		}

		#endregion Methods
	}
}