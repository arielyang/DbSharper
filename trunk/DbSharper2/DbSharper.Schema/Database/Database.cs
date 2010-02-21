using System;
using System.Xml.Serialization;

using DbSharper2.Schema.Infrastructure;

namespace DbSharper2.Schema.Database
{
	[XmlType("database")]
	public class Database
	{
		#region Fields

		private SchemaNamedCollection<Procedure> procedures;
		private SchemaNamedCollection<Table> tables;
		private SchemaNamedCollection<View> views;

		#endregion Fields

		#region Constructors

		public Database()
		{
			this.tables = new SchemaNamedCollection<Table>();
			this.views = new SchemaNamedCollection<View>();
			this.procedures = new SchemaNamedCollection<Procedure>();
		}

		#endregion Constructors

		#region Properties

		[XmlArray("procedures")]
		[XmlArrayItem("procedure")]
		public SchemaNamedCollection<Procedure> Procedures
		{
			get { return procedures; }
			set { throw new NotImplementedException(); }
		}

		[XmlArray("tables")]
		[XmlArrayItem("table")]
		public SchemaNamedCollection<Table> Tables
		{
			get { return tables; }
			set { throw new NotImplementedException(); }
		}

		[XmlArray("views")]
		[XmlArrayItem("view")]
		public SchemaNamedCollection<View> Views
		{
			get { return views; }
			set { throw new NotImplementedException(); }
		}

		#endregion Properties
	}
}