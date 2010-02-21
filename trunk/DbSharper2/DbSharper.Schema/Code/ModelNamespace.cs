using System.Xml.Serialization;

using DbSharper2.Schema.Infrastructure;

namespace DbSharper2.Schema.Code
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

		#region Methods

		public override string ToString()
		{
			return this.Name;
		}

		#endregion Methods
	}
}