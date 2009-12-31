using System;
using System.Xml;
using System.Xml.Serialization;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Database
{
	public class Constraint : IName
	{
		#region Fields

		private NamedCollection<Column> columns;

		#endregion Fields

		#region Constructors

		public Constraint()
		{
			this.columns = new NamedCollection<Column>();
		}

		#endregion Constructors

		#region Properties

		[XmlElement("column")]
		public NamedCollection<Column> Columns
		{
			get { return this.columns; }
			set { throw new NotImplementedException(); }
		}

		[XmlAttribute("name")]
		public string Name
		{
			get;
			set;
		}

		#endregion Properties

		#region Methods

		public override string ToString()
		{
			return this.Name;
		}

		#endregion Methods
	}
}