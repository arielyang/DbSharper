using System.Data;
using DbSharper.Library.Schema;
using DbSharper.Schema;

namespace DbSharper.Data.Sql
{
	[ProviderName("System.Data.SqlClient")]
	public class SchemaProvider : SchemaProviderBase
	{
		protected override string GetSchemaSqlText()
		{
			throw new System.NotImplementedException();
		}

		protected override void InitializeDatabase()
		{
			throw new System.NotImplementedException();
		}

		public override DbType GetDbType(string dbType)
		{
			throw new System.NotImplementedException();
		}
	}
}
