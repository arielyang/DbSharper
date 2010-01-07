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
	}
}