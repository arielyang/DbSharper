using System;
using System.Data;

namespace DbSharper.Schema.Provider
{
	public abstract class SchemaProviderBase
	{
		#region Methods

		public abstract Type GetDatabaseType();

		public abstract DbType GetDbType(string dbType);

		/// <summary>
		/// Get name of the parameter.
		/// </summary>
		/// <param name="parameter">Parameter.</param>
		/// <returns>Name</returns>
		public abstract string GetParameterName(string parameter);

		public abstract Database.Database GetSchema(string connectionString);

		#endregion Methods
	}
}