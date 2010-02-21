using System.Xml.Serialization;

using DbSharper2.Schema.Infrastructure;

namespace DbSharper2.Schema.Database
{
	[XmlType("index")]
	public class Index : IName
	{
		#region Constructors

		public Index()
		{
			this.Columns = new NamedCollection<Column>();
		}

		#endregion Constructors

		#region Properties

		[XmlElement("parameter")]
		public NamedCollection<Column> Columns
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

		#endregion Properties

		#region Methods

		public override string ToString()
		{
			return this.Name;
		}

		#endregion Methods
	}
}