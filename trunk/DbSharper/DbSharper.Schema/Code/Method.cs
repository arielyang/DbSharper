namespace DbSharper.Schema.Code
{
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Data;
    using System.Drawing.Design;
    using System.Xml.Serialization;

    using DbSharper.Schema.Collections;
	using DbSharper.Schema.Enums;

    [XmlType("method")]
    public class Method : IName
    {
        #region Constructors

        public Method()
        {
            this.Parameters = new NamedCollection<Parameter>();
            this.Results = new NamedCollection<Result>();
        }

        #endregion Constructors

        #region Properties

        [XmlAttribute("commandText")]
        [ReadOnly(true)]
        public string CommandText
        {
            get; set;
        }

        [XmlAttribute("commandType")]
        [ReadOnly(true)]
        public CommandType CommandType
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

        [XmlAttribute("methodType")]
        [ReadOnly(true)]
        public MethodType MethodType
        {
            get; set;
        }

        [XmlAttribute("name")]
        [ReadOnly(true)]
        public string Name
        {
            get; set;
        }

        [XmlArray("parameters")]
        [XmlArrayItem("parameter")]
        [Browsable(false)]
        public NamedCollection<Parameter> Parameters
        {
            get; set;
        }

        [XmlArray("results")]
        [XmlArrayItem("result")]
        [Browsable(false)]
        public NamedCollection<Result> Results
        {
            get; set;
        }

        #endregion Properties
    }
}