using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Globalization;
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

		//[XmlAttribute("namespace")]
		[XmlIgnore]
		public string Namespace
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

		/// <summary>
		/// Relative table or view name.
		/// </summary>
		[XmlIgnore]
		internal string MappingName
		{
			get;
			set;
		}

		#endregion Properties

		#region Methods

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", this.Namespace, this.Name);
		}

		#endregion Methods
	}
}