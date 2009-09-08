namespace DbSharper.Schema.Database
{
    using System.Xml.Serialization;

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