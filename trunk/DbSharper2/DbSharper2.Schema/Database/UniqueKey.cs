using System.Xml.Serialization;

namespace DbSharper2.Schema.Database
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