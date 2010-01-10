using System;
using System.Data;

namespace DbSharper.Library.Schema
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class ColumnAttribute : Attribute
	{
		#region Fields

		private DbType dbType;
		private string foreignKeyName;
		private string name;
		private string primaryKeyName;

		#endregion Fields

		#region Constructors

		public ColumnAttribute(string name, DbType dbType)
		{
			this.name = name;
			this.dbType = dbType;
		}

		#endregion Constructors

		#region Properties

		public DbType DbType
		{
			get { return dbType; }
			set { dbType = value; }
		}

		public string ForeignKeyName
		{
			get { return foreignKeyName; }
			set { foreignKeyName = value; }
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public string PrimaryKeyName
		{
			get { return primaryKeyName; }
			set { primaryKeyName = value; }
		}

		#endregion Properties
	}
}