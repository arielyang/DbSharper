using System.ComponentModel;
using System.Xml.Serialization;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Code
{
	[XmlType("enum")]
	[DefaultValue("Name")]
	public class Enumeration : IName
	{
		#region Constructors

		public Enumeration()
		{
			this.Members = new NamedCollection<EnumerationMember>();
		}

		#endregion Constructors

		#region Properties

		[XmlAttribute("baseType")]
		public string BaseType
		{
			get;
			set;
		}

		[XmlAttribute("description")]
		[ReadOnly(true)]
		public string Description
		{
			get;
			set;
		}

		[XmlAttribute("hasFlagsAttribute")]
		public bool HasFlagsAttribute
		{
			get;
			set;
		}

		[XmlElement("member")]
		public NamedCollection<EnumerationMember> Members
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
	}
}