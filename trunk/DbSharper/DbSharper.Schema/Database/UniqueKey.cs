namespace DbSharper.Schema.Database
{
    using System.Xml.Serialization;

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