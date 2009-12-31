using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Xml.Serialization;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Code
{
	[XmlType("resultItem")]
	[DefaultProperty("Description")]
	public class Result : IName
	{
		#region Properties

		[XmlAttribute("description")]
		[Category("Extension")]
		[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		public string Description
		{
			get; set;
		}

		[XmlAttribute("isOutputParameter")]
		[ReadOnly(true)]
		public bool IsOutputParameter
		{
			get; set;
		}

		[XmlAttribute("name")]
		[ReadOnly(true)]
		public string Name
		{
			get; set;
		}

		[XmlAttribute("type")]
		[ReadOnly(true)]
		public string Type
		{
			get; set;
		}

		#endregion Properties
	}
}