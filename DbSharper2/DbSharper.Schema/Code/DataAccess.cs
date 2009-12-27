using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Xml.Serialization;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Code
{
	public class DataAccess : IName
	{
		#region Constructors

		public DataAccess()
		{
			this.Methods = new NamedCollection<Method>();
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

		[XmlElement("method")]
		public NamedCollection<Method> Methods
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

		[XmlAttribute("namespace")]
		public string Namespace
		{
			get;
			set;
		}

		#endregion Properties
	}
}