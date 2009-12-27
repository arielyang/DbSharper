using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing.Design;
using System.Xml.Serialization;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Code
{
	[XmlType("parameter")]
	[DefaultProperty("Description")]
	public class Parameter : IName
	{
		#region Properties

		[XmlAttribute("camelCaseName")]
		[ReadOnly(true)]
		public string CamelCaseName
		{
			get;
			set;
		}

		[XmlAttribute("dbType")]
		[ReadOnly(true)]
		public DbType DbType
		{
			get; set;
		}

		[XmlAttribute("description")]
		[Category("Extension")]
		[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		public string Description
		{
			get; set;
		}

		[XmlAttribute("direction")]
		[ReadOnly(true)]
		public ParameterDirection Direction
		{
			get; set;
		}

		[XmlAttribute("enumType")]
		[ReadOnly(true)]
		public string EnumType
		{
			get; set;
		}

		[XmlAttribute("name")]
		[ReadOnly(true)]
		public string Name
		{
			get; set;
		}

		[XmlAttribute("size")]
		[ReadOnly(true)]
		public int Size
		{
			get; set;
		}

		[XmlAttribute("sqlName")]
		[ReadOnly(true)]
		public string SqlName
		{
			get; set;
		}

		[XmlAttribute("type")]
		[ReadOnly(true)]
		public CommonType Type
		{
			get; set;
		}

		#endregion Properties
	}
}