using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Xml.Serialization;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Code
{
	[XmlType("model")]
	public class Model : IName
	{
		#region Constructors

		public Model()
		{
			this.Properties = new NamedCollection<Property>();
		}

		#endregion Constructors

		#region Properties

		[XmlAttribute("description")]
		[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
		public string Description
		{
			get;
			set;
		}

		[XmlAttribute("isView")]
		[ReadOnly(true)]
		public bool IsView
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

		[XmlElement("property")]
		[Browsable(false)]
		public NamedCollection<Property> Properties
		{
			get;
			set;
		}

		[XmlAttribute("namespace")]
		public string Namespace
		{
			get;
			set;
		}

		/// <summary>
		/// Relative table or view name.
		/// </summary>
		[XmlIgnore]
		internal string MappingSource
		{
			get;
			set;
		}

		#endregion Properties
	}
}