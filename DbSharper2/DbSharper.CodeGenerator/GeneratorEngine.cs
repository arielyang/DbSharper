using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

using DbSharper.Schema;
using DbSharper.Schema.Code;
using DbSharper.Schema.Utility;

namespace DbSharper.CodeGenerator
{
	public class GeneratorEngine
	{
		#region Fields

		public Action<string> AfterFileItemAdded;
		public Action<string> AfterFileItemDeleted;
		public Action<string> BeforeFileItemAdded;
		public Action<Exception> ExceptionThrown;
		public Action<int> ProgressChanged;

		private string defaultExtension;
		private string defaultNamespace;
		private string inputFileContent;
		private string inputFilePath;
		private string mappingContent;
		private string mappingName;

		//private Mapping mapping;
		private List<string> oldFileNames;
		private int progress;
		private int progressStep;

		#endregion Fields

		#region Constructors

		public GeneratorEngine(string inputFilePath, string inputFileContent, string defaultExtension, string defaultNamespace, List<string> oldFileNames)
		{
			this.inputFilePath = inputFilePath;
			this.inputFileContent = inputFileContent;
			this.defaultExtension = defaultExtension;
			this.defaultNamespace = defaultNamespace;
			this.oldFileNames = oldFileNames;

			this.mappingName = Path.GetFileNameWithoutExtension(inputFilePath);
		}

		#endregion Constructors

		#region Properties

		private int Progress
		{
			set
			{
				if (value > 100)
				{
					progress = 100;
				}
				else
				{
					progress = value;
				}
			}

			get
			{
				return progress;
			}
		}

		#endregion Properties

		#region Methods

		public byte[] Generate()
		{
			Mapping newMapping = MappingFactory.CreateMapping(this.inputFilePath, this.inputFileContent);
			Mapping oldMapping = GetOldLocalMapping();

			mappingContent = Serializer.Serialize(newMapping);

			List<FileItem> fileItems = GetFileItems(newMapping, oldMapping);

			GenerateFileItems(fileItems);

			return Encoding.UTF8.GetBytes(mappingContent);
		}

		private string GenerateFileFromTemplate(FileItem fileItem)
		{
			string codeContent;

			StringWriter outputWriter = null;
			XmlReader inputReader = null;

			try
			{
				outputWriter = new StringWriter();
				inputReader = XmlReader.Create(new StringReader(mappingContent));

				XsltArgumentList args = new XsltArgumentList();
				args.AddParam("defaultNamespace", string.Empty, this.defaultNamespace + "." + this.mappingName);
				args.AddParam("schemaNamespace", string.Empty, fileItem.Name);

				XslCompiledTransform transformer;

				transformer = new XslCompiledTransform();
				transformer.Load(fileItem.TemplateFilePath, new XsltSettings(false, true), new XmlUrlResolver());
				transformer.Transform(inputReader, args, outputWriter);
			}
			catch (Exception ex)
			{
				outputWriter.WriteLine("/*");
				outputWriter.WriteLine("\tERROR: Unable to generate output for template:");
				outputWriter.WriteLine("\t'{0}'", this.inputFilePath);
				outputWriter.WriteLine("\tUsing template:");
				outputWriter.WriteLine("\t'{0}'", fileItem.TemplateFilePath);
				outputWriter.WriteLine("");
				outputWriter.WriteLine(ex.ToString());
				outputWriter.WriteLine("*/");
			}
			finally
			{
				codeContent = outputWriter.ToString();

				outputWriter.Close();
				inputReader.Close();
			}

			return codeContent;
		}

		private void GenerateFileItems(List<FileItem> fileItems)
		{
			string fileContent;
			string filePath;

			foreach (var fileItem in fileItems)
			{
				if (BeforeFileItemAdded != null)
				{
					BeforeFileItemAdded(fileItem.FileName);
				}

				fileContent = GenerateFileFromTemplate(fileItem);
				filePath = Path.Combine(Path.GetDirectoryName(this.inputFilePath), fileItem.FileName);

				File.WriteAllText(filePath, fileContent, Encoding.UTF8);

				if (ProgressChanged != null)
				{
					this.Progress += progressStep;

					ProgressChanged(progress);
				}

				if (AfterFileItemAdded != null)
				{
					AfterFileItemAdded(fileItem.FileName);
				}
			}
		}

		private List<FileItem> GetFileItems(Mapping newMapping, Mapping oldMapping)
		{
			List<FileItem> list = new List<FileItem>();

			try
			{
				list.Add(new FileItem(this.mappingName, FileItemType.CacheSettingTemplate, oldMapping == null || !Equalizer.IsEqual(oldMapping.DataAccessNamespaces, newMapping.DataAccessNamespaces)));
				list.Add(new FileItem(this.mappingName, FileItemType.ConnectionStrings, oldMapping == null || !Equalizer.IsEqual(oldMapping.ConnectionStringName, newMapping.ConnectionStringName)));
				list.Add(new FileItem(this.mappingName, FileItemType.Document, oldMapping == null || !Equalizer.IsEqual(oldMapping.Database, newMapping.Database)));
				list.Add(new FileItem(this.mappingName, FileItemType.Enums, oldMapping == null || !Equalizer.IsEqual(oldMapping.Database.Enumerations, newMapping.Database.Enumerations)));

				foreach (ModelNamespace ns in newMapping.ModelNamespaces)
				{
					if (ns.Models.Count > 0)
					{
						if (oldMapping != null && oldMapping.ModelNamespaces.Contains(ns.Name))
						{
							list.Add(new FileItem(this.mappingName, ns.Name, FileItemType.Models, !Equalizer.IsEqual(oldMapping.ModelNamespaces[ns.Name], ns)));
						}
						else
						{
							list.Add(new FileItem(this.mappingName, ns.Name, FileItemType.Models, true));
						}
					}
				}

				foreach (DataAccessNamespace ns in newMapping.DataAccessNamespaces)
				{
					if (ns.DataAccesses.Count > 0)
					{
						if (oldMapping != null && oldMapping.DataAccessNamespaces.Contains(ns.Name))
						{
							list.Add(new FileItem(this.mappingName, ns.Name, FileItemType.DataAccess, !Equalizer.IsEqual(oldMapping.DataAccessNamespaces[ns.Name], ns)));
						}
						else
						{
							list.Add(new FileItem(this.mappingName, ns.Name, FileItemType.DataAccess, true));
						}
					}
				}

				progressStep = (100 - progress) / list.Count;

				progress += progressStep;

				if (ProgressChanged != null)
				{
					ProgressChanged(progress);
				}
			}
			catch (Exception ex)
			{
				ExceptionThrown(ex);

				//mappingContent = ex.Message;

				list.Clear();
			}

			return list;
		}

		private Mapping GetOldLocalMapping()
		{
			string schemaFile = Path.ChangeExtension(inputFilePath, defaultExtension);

			if (!File.Exists(schemaFile))
			{
				return null;
			}

			Mapping mapping;

			try
			{
				mapping = (Mapping)Serializer.Deserialize(typeof(Mapping), File.ReadAllText(schemaFile));
			}
			catch (InvalidOperationException)
			{
				mapping = null;
			}

			return mapping;
		}

		#endregion Methods
	}
}