using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace DbSharper.CodeGenerator
{
	public sealed class FileItem : IChangable
	{
		#region Fields

		private string fileName;
		private bool isChanged;
		private string name;
		private string templateFilePath;
		private ItemType type;

		#endregion Fields

		#region Constructors

		public FileItem(string mappingName, ItemType type, bool isChanged)
		{
			switch (type)
			{
				case ItemType.CacheSettingTemplate:
					{
						name = "CacheSettingTemplate";
						fileName = string.Format("{0}.{1}.config", mappingName, name, CultureInfo.InvariantCulture);
						break;
					}
				case ItemType.ConnectionStrings:
					{
						name = "ConnectionStrings";
						fileName = string.Format("{0}.{1}.cs", mappingName, name, CultureInfo.InvariantCulture);
						break;
					}
				case ItemType.Document:
					{
						name = "Document";
						fileName = string.Format("{0}.{1}.html", mappingName, name, CultureInfo.InvariantCulture);
						break;
					}
				case ItemType.Enums:
					{
						name = "Enums";
						fileName = string.Format("{0}.{1}.cs", mappingName, name, CultureInfo.InvariantCulture);
						break;
					}
				default:
					throw new ArgumentOutOfRangeException("type");
			}

			this.isChanged = isChanged;
			this.templateFilePath = GetTemplatePath(string.Format(@"Templates\DbSharper.{0}.xslt", name));
			this.type = type;
		}

		public FileItem(string mappingName, string name, ItemType type, bool isChanged)
		{
			switch (type)
			{
				case ItemType.DataAccess:
					{
						this.fileName = string.Format("{0}.DataAccess.{1}.cs", mappingName, name, CultureInfo.InvariantCulture);
						this.templateFilePath = GetTemplatePath(@"Templates\DbSharper.DataAccess.xslt");
						break;
					}
				case ItemType.Models:
					{
						this.fileName = string.Format("{0}.Models.{1}.cs", mappingName, name, CultureInfo.InvariantCulture);
						this.templateFilePath = GetTemplatePath(@"Templates\DbSharper.Models.xslt");
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

		public string FileName
		{
			get
			{
				return fileName;
			}
		}

		public bool IsChanged
		{
			get
			{
				return isChanged;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		public string TemplateFilePath
		{
			get
			{
				return templateFilePath;
			}
		}

		public ItemType Type
		{
			get
			{
				return type;
			}
		}

		#endregion Properties

		#region Methods

		private string GetTemplatePath(string filePath)
		{
			string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			return Path.Combine(assemblyPath, filePath);
		}

		#endregion Methods
	}
}