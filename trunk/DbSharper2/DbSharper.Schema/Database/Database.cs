using System.Xml.Serialization;

using DbSharper.Schema.Code;
using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Database
{
	[XmlType("database")]
	public class Database
	{
		#region Constructors

		public Database()
		{
			this.Tables = new SchemaNamedCollection<Table>();
			this.Views = new SchemaNamedCollection<View>();
			this.Procedures = new SchemaNamedCollection<Procedure>();
			this.Enumerations = new NamedCollection<Enumeration>();
		}

		#endregion Constructors

		#region Properties

		[XmlIgnore]
		public NamedCollection<Enumeration> Enumerations
		{
			get;
			set;
		}

		[XmlArray("procedures")]
		[XmlArrayItem("procedure")]
		public SchemaNamedCollection<Procedure> Procedures
		{
			get;
			set;
		}

		[XmlArray("tables")]
		[XmlArrayItem("table")]
		public SchemaNamedCollection<Table> Tables
		{
			get;
			set;
		}

		[XmlArray("views")]
		[XmlArrayItem("view")]
		public SchemaNamedCollection<View> Views
		{
			get;
			set;
		}

		#endregion Properties
	}
}