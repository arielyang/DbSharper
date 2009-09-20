using System.Xml.Serialization;

namespace DbSharper.Schema.Database
{
	[XmlType("uniqueKey")]
	public class UniqueKey : Constraint
	{
		#region Constructors

		public UniqueKey()
			: base()
		{
		}

		#endregion Constructors
	}
}