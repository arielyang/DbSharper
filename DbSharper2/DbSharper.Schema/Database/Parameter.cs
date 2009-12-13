using System.Data;
using System.Globalization;
using System.Xml.Serialization;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Database
{
	[XmlType("sqlParameter")]
	public class Parameter : IName
	{
		#region Properties

		[XmlAttribute("description")]
		public string Description
		{
			get; set;
		}

		[XmlAttribute("direction")]
		public ParameterDirection Direction
		{
			get; set;
		}

		[XmlAttribute("name")]
		public string Name
		{
			get; set;
		}

		[XmlAttribute("size")]
		public int Size
		{
			get; set;
		}

		[XmlAttribute("dbType")]
		public DbType DbType
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
				this.DbType,
				this.Size == 0 ? string.Empty : this.Size.ToString(CultureInfo.InvariantCulture),
				this.Direction).Trim();
		}

		#endregion Methods
	}
}