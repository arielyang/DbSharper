using System.Xml.Serialization;

namespace DbSharper.Schema.Database
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

		[XmlAttribute("referentialTable")]
		public string ReferentialTable
		{
			get; set;
		}

		#endregion Properties
	}
}