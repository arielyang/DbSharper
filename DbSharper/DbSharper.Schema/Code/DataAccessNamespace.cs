using System.Xml.Serialization;

using DbSharper.Schema.Collections;

namespace DbSharper.Schema.Code
{
	public class DataAccessNamespace : IName
	{
		#region Constructors

		public DataAccessNamespace()
		{
			this.DataAccesses = new NamedCollection<DataAccess>();
		}

		#endregion Constructors

		#region Properties

		[XmlElement("dataAccess")]
		public NamedCollection<DataAccess> DataAccesses
		{
			get; set;
		}

		[XmlAttribute("name")]
		public string Name
		{
			get; set;
		}

		#endregion Properties
	}
}