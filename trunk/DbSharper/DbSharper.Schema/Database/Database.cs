using System.Xml.Serialization;

using DbSharper.Schema.Collections;

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
			this.StoredProcedures = new SchemaNamedCollection<StoredProcedure>();
			this.Enumerations = new NamedCollection<Enumeration>();
		}

		#endregion Constructors

		#region Properties

		[XmlIgnore]
		public NamedCollection<Enumeration> Enumerations
		{
			get; set;
		}

		[XmlArray("storedProcedures")]
		[XmlArrayItem("storedProcedure")]
		public SchemaNamedCollection<StoredProcedure> StoredProcedures
		{
			get; set;
		}

		[XmlArray("tables")]
		[XmlArrayItem("table")]
		public SchemaNamedCollection<Table> Tables
		{
			get; set;
		}

		[XmlArray("views")]
		[XmlArrayItem("view")]
		public SchemaNamedCollection<View> Views
		{
			get; set;
		}

		#endregion Properties
	}
}