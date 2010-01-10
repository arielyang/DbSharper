using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing.Design;
using System.Xml.Serialization;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Code
{
	[XmlType("property")]
	[DefaultProperty("Description")]
	public class Property : IName
	{
		#region Properties

		[XmlAttribute("camelCaseName")]
		[ReadOnly(true)]
		public string CamelCaseName
		{
			get;
			set;
		}

		//[XmlAttribute("canGetCollectionBy")]
		//[ReadOnly(true)]
		//public bool CanGetCollectionBy
		//{
		//    get; set;
		//}

		//[XmlAttribute("canGetItemBy")]
		//[ReadOnly(true)]
		//public bool CanGetItemBy
		//{
		//    get; set;
		//}

		[XmlAttribute("primaryKeyName")]
		[ReadOnly(true)]
		public string PrimaryKeyName
		{
			get;
			set;
		}

		[XmlAttribute("foreignKeyName")]
		[ReadOnly(true)]
		public string ForeignKeyName
		{
			get;
			set;
		}

		[XmlAttribute("columnName")]
		[ReadOnly(true)]
		public string ColumnName
		{
			get; set;
		}

		[XmlAttribute("dbType")]
		[ReadOnly(true)]
		public DbType DbType
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

		[XmlAttribute("enumType")]
		[ReadOnly(true)]
		public string EnumType
		{
			get;
			set;
		}

		[XmlAttribute("hasDefault")]
		[ReadOnly(true)]
		public bool HasDefault
		{
			get; set;
		}

		[XmlAttribute("isExtended")]
		[ReadOnly(true)]
		public bool IsExtended
		{
			get; set;
		}

		[XmlIgnore]
		public bool IsPrimaryKey
		{
			get
			{
				return !string.IsNullOrEmpty(this.PrimaryKeyName);
			}
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

		[XmlAttribute("refPkName")]
		[ReadOnly(true)]
		public string RefPkName
		{
			get;
			set;
		}

		[XmlAttribute("size")]
		[ReadOnly(true)]
		public int Size
		{
			get; set;
		}

		[XmlAttribute("type")]
		[ReadOnly(true)]
		public string Type
		{
			get; set;
		}

		#endregion Properties

		#region Methods

		public override string ToString()
		{
			return this.Name;
		}

		#endregion Methods
	}
}