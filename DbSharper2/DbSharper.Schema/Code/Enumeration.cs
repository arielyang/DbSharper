using System;
using System.ComponentModel;
using System.Xml.Serialization;

using DbSharper2.Schema.Infrastructure;

namespace DbSharper2.Schema.Code
{
	[XmlType("enumeration")]
	[DefaultValue("Name")]
	[Serializable]
	public class Enumeration : IName
	{
		#region Fields

		private CommonType commonType;
		private string fullName;
		private string name;

		#endregion Fields

		#region Constructors

		public Enumeration()
		{
		}

		public Enumeration(Type type)
		{
			this.name = type.Name;
			this.fullName = type.FullName;
			this.CommonType = type.ToCommonType();
		}

		#endregion Constructors

		#region Properties

		public CommonType CommonType
		{
			get { return commonType; }
			set { commonType = value; }
		}

		public string FullName
		{
			get { return this.fullName; }
			set { this.fullName = value; }
		}

		[XmlAttribute("name")]
		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		#endregion Properties
	}
}