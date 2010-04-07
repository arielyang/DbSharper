using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;

using DbSharper2.Data.MySql;
using DbSharper2.Schema;
using DbSharper2.Schema.Database;
using DbSharper2.Schema.Provider;
using DbSharper2.Schema.Utility;

namespace DbSharper2.Provider.MySql
{
	[SchemaProvider("System.Data.SqlClient")]
	public class MySqlSchemaProvider : SchemaProviderBase
	{
		public override string BuildParameterSqlName(string parameterName)
		{
			throw new NotImplementedException();
		}

		public override Type GetDatabaseType()
		{
			throw new NotImplementedException();
		}

		public override DbType GetDbType(string dbTypeString)
		{
			throw new NotImplementedException();
		}

		public override string GetParameterName(string parameter)
		{
			throw new NotImplementedException();
		}

		public override Database GetSchema(string connectionString)
		{
			throw new NotImplementedException();
		}
	}
}
