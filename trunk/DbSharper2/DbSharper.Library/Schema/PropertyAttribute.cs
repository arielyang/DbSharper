using System;
using System.Data;

namespace DbSharper.Library.Schema
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public sealed class PropertyAttribute : Attribute
	{
		#region Fields

		private string columnName;
		private bool hasDefault;
		private bool isFk;
		private bool isPk;
		private SqlDbType sqlDbType;

		#endregion Fields

		#region Constructors

		public PropertyAttribute(string columnName, SqlDbType sqlDbType, bool isPk, bool isFk, bool hasDefault)
		{
			this.columnName = columnName;
			this.sqlDbType = sqlDbType;
			this.isPk = isPk;
			this.isFk = isFk;
			this.hasDefault = hasDefault;
		}

		#endregion Constructors

		#region Properties

		public string ColumnName
		{
			get { return columnName; }
			set { columnName = value; }
		}

		public bool HasDefault
		{
			get { return hasDefault; }
			set { hasDefault = value; }
		}

		public bool IsFk
		{
			get { return isFk; }
			set { isFk = value; }
		}

		public bool IsPk
		{
			get { return isPk; }
			set { isPk = value; }
		}

		public SqlDbType SqlDbType
		{
			get { return sqlDbType; }
			set { sqlDbType = value; }
		}

		#endregion Properties
	}
}