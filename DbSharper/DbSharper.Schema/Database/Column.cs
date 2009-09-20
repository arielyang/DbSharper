using System.Data;
using System.Globalization;
using System.Xml.Serialization;

using DbSharper.Schema.Collections;

namespace DbSharper.Schema.Database
{
	[XmlType("column")]
	public class Column : IName
	{
		#region Properties

		[XmlAttribute("default")]
		public string Default
		{
			get; set;
		}

		[XmlAttribute("description")]
		public string Description
		{
			get; set;
		}

		[XmlAttribute("name")]
		public string Name
		{
			get; set;
		}

		[XmlAttribute("nullable")]
		public bool Nullable
		{
			get; set;
		}

		[XmlAttribute("size")]
		public int Size
		{
			get; set;
		}

		[XmlAttribute("sqlDbType")]
		public SqlDbType SqlDbType
		{
			get; set;
		}

		#endregion Properties

		#region Methods

		public override string ToString()
		{
			return string.Format(
				CultureInfo.InvariantCulture,
				"{0} {1} {2} {3}",
				this.Name,
				this.SqlDbType,
				this.Size == 0 ? string.Empty : "(" + this.Size.ToString(CultureInfo.InvariantCulture) + ")",
				this.Default).Trim();
		}

		#endregion Methods
	}
}