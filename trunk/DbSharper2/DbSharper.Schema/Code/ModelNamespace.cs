using System.Xml.Serialization;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Code
{
	public class ModelNamespace : IName
	{
		#region Constructors

		public ModelNamespace()
		{
			this.Models = new NamedCollection<Model>();
		}

		#endregion Constructors

		#region Properties

		[XmlElement("model")]
		public NamedCollection<Model> Models
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