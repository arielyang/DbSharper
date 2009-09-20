using System.Xml;
using System.Xml.Serialization;

using DbSharper.Schema.Collections;

namespace DbSharper.Schema.Database
{
	public class Constraint : IName
	{
		#region Constructors

		public Constraint()
		{
			this.Columns = new NamedCollection<Column>();
		}

		#endregion Constructors

		#region Properties

		[XmlElement("column")]
		public NamedCollection<Column> Columns
		{
			get; set;
		}

		[XmlAttribute("name")]
		public string Name
		{
			get; set;
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