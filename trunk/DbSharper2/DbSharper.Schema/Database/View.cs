using System.Xml.Serialization;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Database
{
	[XmlType("view")]
	public class View : IColumns
	{
		#region Constructors

		public View()
		{
			this.Columns = new NamedCollection<Column>();
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

		[XmlAttribute("schema")]
		public string Schema
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