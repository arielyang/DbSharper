using System;

namespace DbSharper.Library.Schema
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class ModelAttribute : Attribute
	{
		#region Fields

		private string tableName;

		#endregion Fields

		#region Constructors

		public ModelAttribute(string tableName)
		{
			this.tableName = tableName;
		}

		#endregion Constructors

		#region Properties

		public string TableName
		{
			get { return tableName; }
			set { tableName = value; }
		}

		#endregion Properties
	}
}