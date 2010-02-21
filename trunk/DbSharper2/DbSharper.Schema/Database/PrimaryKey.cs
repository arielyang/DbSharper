using System.Xml.Serialization;

namespace DbSharper2.Schema.Database
{
	[XmlType("primaryKey")]
	public class PrimaryKey : Constraint
	{
		#region Constructors

		public PrimaryKey()
			: base()
		{
		}

		#endregion Constructors
	}
}