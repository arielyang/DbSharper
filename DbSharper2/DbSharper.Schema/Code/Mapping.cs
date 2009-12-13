using System.Xml.Serialization;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Code
{
	[XmlRoot("mapping")]
	public class Mapping
	{
		#region Constructors

		internal Mapping()
		{
			this.ModelNamespaces = new NamedCollection<ModelNamespace>();
			this.DataAccessNamespaces = new NamedCollection<DataAccessNamespace>();
			this.Enumerations = new NamedCollection<Enumeration>();
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

		[XmlArray("enums")]
		[XmlArrayItem("enum")]
		public NamedCollection<Enumeration> Enumerations
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

		internal Model GetModel(string name)
		{
			foreach (var nameSpace in this.ModelNamespaces)
			{
				foreach (var m in nameSpace.Models)
				{
					if (m.Name == name)
					{
						return m;
					}
				}
			}

			return null;
		}

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

		#endregion Methods
	}
}