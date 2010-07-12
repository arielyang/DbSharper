using System.Xml.Serialization;

namespace DbSharper.Schema.Database
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