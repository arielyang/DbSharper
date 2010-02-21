using System.Xml.Serialization;

using DbSharper2.Schema.Infrastructure;

namespace DbSharper2.Schema.Database
{
	[XmlType("procedure")]
	public class Procedure : ISchema
	{
		#region Constructors

		public Procedure()
		{
			this.Parameters = new NamedCollection<Parameter>();
		}

		#endregion Constructors

		#region Properties

		[XmlIgnore]
		public string Definition
		{
			get;
			set;
		}

		[XmlAttribute("description")]
		public string Description
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

		[XmlElement("parameter")]
		public NamedCollection<Parameter> Parameters
		{
			get;
			set;
		}

		[XmlAttribute("schema")]
		public string Schema
		{
			get;
			set;
		}

		#endregion Properties

		#region Methods

		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.Schema))
			{
				return this.Name;
			}
			else
			{
				return this.Schema + "." + this.Name;
			}
		}

		#endregion Methods
	}
}