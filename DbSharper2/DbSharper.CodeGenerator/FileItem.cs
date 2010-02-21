using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace DbSharper2.CodeGenerator
{
	/// <summary>
	/// Generated file item.
	/// </summary>
	internal sealed class FileItem
	{
		#region Fields

		private string fileName;
		private bool isChanged;
		private string name;
		private string templateFilePath;
		private FileItemType type;

		#endregion Fields

		#region Constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="mappingName">Mapping file name.</param>
		/// <param name="type">File item type.</param>
		/// <param name="isChanged">If this file item is changed since last code generation.</param>
		public FileItem(string mappingName, FileItemType type, bool isChanged)
		{
			switch (type)
			{
				case FileItemType.CacheSettingTemplate:
					{
						name = "CacheSettingTemplate";
						fileName = string.Format("{0}.{1}.config", mappingName, name, CultureInfo.InvariantCulture);
						break;
					}
				case FileItemType.ConnectionStrings:
					{
						name = "ConnectionStrings";
						fileName = string.Format("{0}.{1}.cs", mappingName, name, CultureInfo.InvariantCulture);
						break;
					}
				case FileItemType.Document:
					{
						name = "Document";
						fileName = string.Format("{0}.{1}.html", mappingName, name, CultureInfo.InvariantCulture);
						break;
					}
				case FileItemType.Enums:
					{
						name = "Enums";
						fileName = string.Format("{0}.{1}.cs", mappingName, name, CultureInfo.InvariantCulture);
						break;
					}
				default:
					throw new ArgumentOutOfRangeException("type");
			}

			this.isChanged = isChanged;
			this.templateFilePath = GetTemplateFilePath(string.Format(@"Templates\DbSharper.{0}.xslt", name));
			this.type = type;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="mappingName">Mapping file name.</param>
		/// <param name="name">File item name.</param>
		/// <param name="type">File item type.</param>
		/// <param name="isChanged">If this file item is changed since last code generation.</param>
		public FileItem(string mappingName, string name, FileItemType type, bool isChanged)
		{
			switch (type)
			{
				case FileItemType.DataAccess:
					{
						this.fileName = string.Format("{0}.DataAccess.{1}.cs", mappingName, name, CultureInfo.InvariantCulture);
						this.templateFilePath = GetTemplateFilePath(@"Templates\DbSharper.DataAccess.xslt");
						break;
					}
				case FileItemType.Models:
					{
						this.fileName = string.Format("{0}.Models.{1}.cs", mappingName, name, CultureInfo.InvariantCulture);
						this.templateFilePath = GetTemplateFilePath(@"Templates\DbSharper.Models.xslt");
						break;
					}
				default:
					throw new ArgumentOutOfRangeException("type");
			}

			this.isChanged = isChanged;
			this.name = name;
			this.type = type;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// File name.
		/// </summary>
		public string FileName
		{
			get
			{
				return fileName;
			}
		}

		/// <summary>
		/// If this file item is changed since last code generation.
		/// </summary>
		public bool IsChanged
		{
			get
			{
				return isChanged;
			}
		}

		/// <summary>
		/// File item name.
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
		}

		/// <summary>
		/// Absolute template file path.
		/// </summary>
		public string TemplateFilePath
		{
			get
			{
				return templateFilePath;
			}
		}

		/// <summary>
		/// Type of file item.
		/// </summary>
		public FileItemType Type
		{
			get
			{
				return type;
			}
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Get absolute template file path.
		/// </summary>
		/// <param name="filePath">Relative template file path.</param>
		/// <returns>Absolute template file path.</returns>
		private string GetTemplateFilePath(string filePath)
		{
			string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			return Path.Combine(assemblyPath, filePath);
		}

		#endregion Methods
	}
}