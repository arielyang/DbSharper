using System;
using System.Data;

namespace DbSharper2.Library.Schema
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class ColumnAttribute : Attribute
	{
		#region Fields

		private DbType dbType;
		private bool isPrimaryKey;
		private string name;

		#endregion Fields

		#region Constructors

		public ColumnAttribute(string name, DbType dbType, bool isPrimaryKey)
		{
			this.name = name;
			this.dbType = dbType;
			this.isPrimaryKey = isPrimaryKey;
		}

		#endregion Constructors

		#region Properties

		public DbType DbType
		{
			get { return dbType; }
			set { dbType = value; }
		}

		public bool IsPrimaryKey
		{
			get { return isPrimaryKey; }
			set { isPrimaryKey = value; }
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		#endregion Properties
	}
}