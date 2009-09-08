namespace DbSharper.Schema.Code
{
    using System.ComponentModel;
    using System.Xml.Serialization;

    using DbSharper.Schema.Collections;

    [XmlType("member")]
    [DefaultProperty("Name")]
    public class EnumMember : IName
    {
        #region Properties

        [XmlAttribute("description")]
        [ReadOnly(true)]
        public string Description
        {
            get; set;
        }

        [XmlAttribute("name")]
        [ReadOnly(true)]
        public string Name
        {
            get; set;
        }

        [XmlAttribute("value")]
        [ReadOnly(true)]
        public int Value
        {
            get; set;
        }

        #endregion Properties
    }
}