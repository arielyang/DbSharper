using System.ComponentModel;
using System.Xml.Serialization;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Code
{
	[XmlType("member")]
	[DefaultProperty("Name")]
	public class EnumerationMember : IName
	{
		#region Properties

		[XmlAttribute("description")]
		[ReadOnly(true)]
		public string Description
		{
			get; set;
		}

		[XmlAttribute("name")]
		[ReadOnly(true)]
		public string Name
		{
			get; set;
		}

		[XmlAttribute("value")]
		[ReadOnly(true)]
		public int Value
		{
			get; set;
		}

		#endregion Properties
	}
}