using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

using DbSharper2.Schema;
using DbSharper2.Schema.Code;
using DbSharper2.Schema.Infrastructure;
using DbSharper2.Schema.Utility;
using System.Security.Policy;
using System.Security.Permissions;
using System.Security;
using System.Diagnostics;

namespace DbSharper2.CodeGenerator
{
	public class GeneratorEngine
	{
		#region Fields

		public Action<string> AfterFileItemAdded;
		public Action<string> BeforeFileItemAdded;
		public Action<string> BeforeFileItemDeleted;
		public Action<Exception> ExceptionThrown;
		public Action<int> ProgressChanged;

		private string defaultExtension;
		private string defaultNamespace;
		private bool firstRunOfThisGeneratorVersion;
		private string inputFileContent;
		private string inputFilePath;
		private string mappingContent;
		private string mappingName;
		private List<string> oldFileNames;
		private int progress;
		private int progressStep;
		private List<string> references;

		#endregion Fields

		#region Constructors

		public GeneratorEngine(string inputFilePath, string inputFileContent, string defaultExtension, string defaultNamespace, List<string> references, List<string> oldFileNames)
		{
			this.inputFilePath = inputFilePath;
			this.inputFileContent = inputFileContent;
			this.defaultExtension = defaultExtension;
			this.defaultNamespace = defaultNamespace;
			this.references = references;
			this.oldFileNames = oldFileNames;

			this.mappingName = Path.GetFileNameWithoutExtension(inputFilePath);

			DateTime assemblyCreationDateTime = new FileInfo(Assembly.GetExecutingAssembly().Location).CreationTime;
			DateTime lastGenerationDateTime = GetLastGenerationDateTime();

			this.firstRunOfThisGeneratorVersion = assemblyCreationDateTime > lastGenerationDateTime;
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
			var emumerations = GetAllEnumerations();

			Mapping newMapping = MappingFactory.CreateMapping(this.inputFilePath, this.inputFileContent, emumerations);

			Mapping oldMapping = GetOldLocalMapping();

			mappingContent = Serializer.Serialize(newMapping);

			List<FileItem> fileItems = GetFileItems(newMapping, oldMapping);

			CleanUpOldFileItems(fileItems);

			GenerateFileItems(fileItems);

			return Encoding.UTF8.GetBytes(mappingContent);
		}

		private void CleanUpOldFileItems(List<FileItem> fileItems)
		{
			string filePath;

			foreach (var fileName in this.oldFileNames)
			{
				if (fileName == mappingName + defaultExtension)
				{
					continue;
				}

				if (fileItems.Find(fileItem => fileItem.FileName == fileName) == null)
				{
					filePath = Path.Combine(Path.GetDirectoryName(this.inputFilePath), fileName);

					if (BeforeFileItemDeleted != null)
					{
						BeforeFileItemDeleted(filePath);
					}

					File.Delete(fileName);
				}
			}
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
				if (fileItem.IsChanged)
				{
					fileContent = GenerateFileFromTemplate(fileItem);
					filePath = Path.Combine(Path.GetDirectoryName(this.inputFilePath), fileItem.FileName);

					if (BeforeFileItemAdded != null)
					{
						BeforeFileItemAdded(fileItem.FileName);
					}

					if (File.Exists(filePath))
					{
						File.Delete(filePath);
					}

					File.WriteAllText(filePath, fileContent, Encoding.UTF8);

					if (ProgressChanged != null)
					{
						this.Progress += progressStep;

						ProgressChanged(progress);
					}

					if (AfterFileItemAdded != null)
					{
						AfterFileItemAdded(filePath);
					}
				}
			}
		}

		private List<Enumeration> GetEnumerationFromAssembly(string assemblyFilePath)
		{
			if (string.IsNullOrEmpty(assemblyFilePath))
			{
				throw new ArgumentNullException("assemblyFileName");
			}

			if (!File.Exists(assemblyFilePath))
			{
				throw new FileNotFoundException("Can not find assembly file.", assemblyFilePath);
			}

			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationBase = Path.GetDirectoryName(typeof(AssemblyLoader).Assembly.Location);
			//setup.ShadowCopyFiles = "true";
			//setup.ShadowCopyDirectories = @"e:\test\";

			PermissionSet ps = new PermissionSet(PermissionState.Unrestricted);

			var appDomain = AppDomain.CreateDomain("TempDomain", null, setup, ps);

			appDomain.ReflectionOnlyAssemblyResolve += (sender, args) =>
			{
				return Assembly.ReflectionOnlyLoad(args.Name);
			};

			Type loaderType = typeof(AssemblyLoader);

			var loader = (AssemblyLoader)appDomain.CreateInstanceAndUnwrap(loaderType.Assembly.FullName, loaderType.FullName);

			var enumerations = loader.GetPublicEnumTypes(assemblyFilePath);

			AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);

			AppDomain.Unload(appDomain);

			return enumerations;
		}

		private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			var n = args.Name;

			var asm = Assembly.Load(n);

			return asm;
		}

		private NamedCollection<Enumeration> GetAllEnumerations()
		{
			NamedCollection<Enumeration> emumerations = new NamedCollection<Enumeration>();

			foreach (var reference in this.references)
			{
				if (File.Exists(reference))
				{
					GetEnumerationFromAssembly(reference).ForEach(
						enumeratioin => emumerations.Add(enumeratioin)
						);
				}
			}

			return emumerations;
		}

		private List<FileItem> GetFileItems(Mapping newMapping, Mapping oldMapping)
		{
			List<FileItem> list = new List<FileItem>();

			try
			{
				list.Add(new FileItem(this.mappingName, FileItemType.CacheSettingTemplate, this.firstRunOfThisGeneratorVersion || oldMapping == null || !Equalizer.IsEqual(oldMapping.DataAccessNamespaces, newMapping.DataAccessNamespaces)));
				list.Add(new FileItem(this.mappingName, FileItemType.ConnectionStrings, this.firstRunOfThisGeneratorVersion || oldMapping == null || !Equalizer.IsEqual(oldMapping.ConnectionStringName, newMapping.ConnectionStringName)));
				list.Add(new FileItem(this.mappingName, FileItemType.Document, this.firstRunOfThisGeneratorVersion || oldMapping == null || !Equalizer.IsEqual(oldMapping.Database, newMapping.Database)));

				foreach (ModelNamespace ns in newMapping.ModelNamespaces)
				{
					if (ns.Models.Count > 0)
					{
						if (oldMapping != null && oldMapping.ModelNamespaces.Contains(ns.Name))
						{
							list.Add(new FileItem(this.mappingName, ns.Name, FileItemType.Models, this.firstRunOfThisGeneratorVersion || !Equalizer.IsEqual(oldMapping.ModelNamespaces[ns.Name], ns)));
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
							list.Add(new FileItem(this.mappingName, ns.Name, FileItemType.DataAccess, this.firstRunOfThisGeneratorVersion || !Equalizer.IsEqual(oldMapping.DataAccessNamespaces[ns.Name], ns)));
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

		private DateTime GetLastGenerationDateTime()
		{
			string mappingFilePath = Path.ChangeExtension(this.inputFilePath, this.defaultExtension);

			if (!File.Exists(mappingFilePath))
			{
				return DateTime.Now;
			}

			return new FileInfo(mappingFilePath).CreationTime;
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