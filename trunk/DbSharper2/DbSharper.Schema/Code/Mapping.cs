using System.Xml.Serialization;

using DbSharper.Schema.Database;
using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Code
{
	[XmlRoot("mapping")]
	public class Mapping
	{
		#region Constructors

		internal Mapping()
		{
			this.DataAccessNamespaces = new NamedCollection<DataAccessNamespace>();
			this.ModelNamespaces = new NamedCollection<ModelNamespace>();
		}

		#endregion Constructors

		#region Properties

		[XmlAttribute("connectionStringName")]
		public string ConnectionStringName
		{
			get;
			set;
		}

		[XmlArray("dataAccesses")]
		[XmlArrayItem("namespace")]
		public NamedCollection<DataAccessNamespace> DataAccessNamespaces
		{
			get;
			set;
		}

		[XmlElement("database")]
		public Database.Database Database
		{
			get;
			set;
		}

		[XmlAttribute("databaseType")]
		public string DatabaseType
		{
			get;
			set;
		}

		[XmlArray("models")]
		[XmlArrayItem("namespace")]
		public NamedCollection<ModelNamespace> ModelNamespaces
		{
			get;
			set;
		}

		#endregion Properties

		#region Methods

		internal bool ContainsModel(string name)
		{
			foreach (var nameSpace in this.ModelNamespaces)
			{
				if (nameSpace.Models.Contains(name))
				{
					return true;
				}
			}

			return false;
		}

		internal Model GetModel(string schema, string name)
		{
			if (!this.ModelNamespaces.Contains(schema))
			{
				return null;
			}

			ModelNamespace nameSpace = this.ModelNamespaces[schema];

			if (!nameSpace.Models.Contains(name))
			{
				return null;
			}

			return nameSpace.Models[name];
		}

		internal Model GetModel(string mappingTableName)
		{
			NamedCollection<Model> models;

			foreach (var modelNamespace in ModelNamespaces)
			{
				models = modelNamespace.Models;

				foreach (var model in models)
				{
					if (model.MappingName == mappingTableName)
					{
						return model;
					}
				}
			}

			return null;
		}

		#endregion Methods

		#region Other

		//internal Model GetModel(string name)
		//{
		//	foreach (var nameSpace in this.ModelNamespaces)
		//	{
		//		foreach (var m in nameSpace.Models)
		//		{
		//			if (m.Name == name)
		//			{
		//				return m;
		//			}
		//		}
		//	}
		//	return null;
		//}

		#endregion Other
	}
}