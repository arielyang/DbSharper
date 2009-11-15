using System.ComponentModel;
using System.Xml.Serialization;

using DbSharper.Schema.Collections;

namespace DbSharper.Schema.Code
{
	[XmlType("enum")]
	[DefaultValue("Name")]
	public class Enum : IName
	{
		#region Constructors

		public Enum()
		{
			this.Members = new NamedCollection<EnumMember>();
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
		public NamedCollection<EnumMember> Members
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