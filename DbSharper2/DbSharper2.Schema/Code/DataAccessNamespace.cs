using System.Xml.Serialization;

using DbSharper2.Schema.Infrastructure;

namespace DbSharper2.Schema.Code
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