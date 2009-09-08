namespace DbSharper.Schema.Database
{
    using System.Xml.Serialization;

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