using System.Xml.Serialization;

namespace DbSharper2.Schema.Database
{
	[XmlType("foreignKey")]
	public class ForeignKey : Constraint
	{
		#region Constructors

		public ForeignKey()
			: base()
		{
		}

		#endregion Constructors

		#region Properties

		[XmlAttribute("referentialTableName")]
		public string ReferentialTableName
		{
			get; set;
		}

		#endregion Properties
	}
}