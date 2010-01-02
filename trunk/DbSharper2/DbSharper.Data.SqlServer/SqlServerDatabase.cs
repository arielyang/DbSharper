// Move to DbSharper.Library.
/*
using System.Data.SqlClient;

using DbSharper.Library.Data;

namespace DbSharper.Data.SqlServer
{
	public class SqlServerDatabase : Database
	{
		#region Constructors

		public SqlServerDatabase()
			: base(SqlClientFactory.Instance)
		{
		}

		#endregion Constructors

		#region Methods

		public override string BuildParameterName(string name)
		{
			if (name[0] != '@')
			{
				return "@" + name;
			}

			return name;
		}

		#endregion Methods
	}
}
*/