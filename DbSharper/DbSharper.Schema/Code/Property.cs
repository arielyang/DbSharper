namespace DbSharper.Schema.Code
{
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Data;
    using System.Drawing.Design;
    using System.Xml.Serialization;

	using DbSharper.Schema.Collections;
	using DbSharper.Schema.Enums;

    [XmlType("property")]
    [DefaultProperty("Description")]
    public class Property : IName
    {
        #region Properties

        [XmlAttribute("attributes")]
        [Category("Extension")]
        public string Attributes
        {
            get; set;
        }

        [XmlAttribute("canGetCollectionBy")]
        [ReadOnly(true)]
        public bool CanGetCollectionBy
        {
            get; set;
        }

        [XmlAttribute("canGetItemBy")]
        [ReadOnly(true)]
        public bool CanGetItemBy
        {
            get; set;
        }

        [XmlAttribute("column")]
        [ReadOnly(true)]
        public string Column
        {
            get; set;
        }

        [XmlAttribute("columnName")]
        [ReadOnly(true)]
        public string ColumnName
        {
            get; set;
        }

        [XmlAttribute("description")]
        [Category("Extension")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string Description
        {
            get; set;
        }

        [XmlIgnore]
        public bool HasDefault
        {
            get; set;
        }

        [XmlIgnore]
        public bool IsPrimaryKey
        {
            get; set;
        }

        [XmlAttribute("name")]
        [ReadOnly(true)]
        public string Name
        {
            get; set;
        }

        [XmlAttribute("nulls")]
        [ReadOnly(true)]
        public bool Nulls
        {
            get; set;
        }

        [XmlAttribute("referenceType")]
        [ReadOnly(true)]
        public string ReferenceType
        {
            get; set;
        }

        [XmlAttribute("size")]
        [ReadOnly(true)]
        public int Size
        {
            get; set;
        }

        [XmlAttribute("sqlDbType")]
        [ReadOnly(true)]
        public SqlDbType SqlDbType
        {
            get; set;
        }

        [XmlAttribute("type")]
        [ReadOnly(true)]
        public CommonType Type
        {
            get; set;
        }

        #endregion Properties
    }
}