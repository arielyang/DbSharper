using System.Xml.Serialization;

using DbSharper.Schema.Collections;

namespace DbSharper.Schema.Database
{
	[XmlType("table")]
	public class Table : IColumns
	{
		#region Constructors

		public Table()
		{
			this.Columns = new NamedCollection<Column>();
			this.PrimaryKey = new PrimaryKey();
			this.ForeignKeys = new NamedCollection<ForeignKey>();
			this.UniqueKeys = new NamedCollection<UniqueKey>();
			this.Indexes = new NamedCollection<Index>();
		}

		#endregion Constructors

		#region Properties

		[XmlArray("columns")]
		[XmlArrayItem("column")]
		public NamedCollection<Column> Columns
		{
			get;
			set;
		}

		[XmlAttribute("description")]
		public string Description
		{
			get;
			set;
		}

		[XmlArray("foreignKeys")]
		[XmlArrayItem("foreignKey")]
		public NamedCollection<ForeignKey> ForeignKeys
		{
			get;
			set;
		}

		[XmlArray("indexes")]
		[XmlArrayItem("index")]
		public NamedCollection<Index> Indexes
		{
			get;
			set;
		}

		[XmlAttribute("name")]
		public string Name
		{
			get;
			set;
		}

		[XmlElement("primaryKey")]
		public PrimaryKey PrimaryKey
		{
			get;
			set;
		}

		[XmlAttribute("schema")]
		public string Schema
		{
			get;
			set;
		}

		[XmlArray("uniqueKeys")]
		[XmlArrayItem("uniqueKey")]
		public NamedCollection<UniqueKey> UniqueKeys
		{
			get;
			set;
		}

		#endregion Properties

		#region Methods

		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.Schema))
			{
				return this.Name;
			}
			else
			{
				return this.Schema + "." + this.Name;
			}
		}

		#endregion Methods
	}
}