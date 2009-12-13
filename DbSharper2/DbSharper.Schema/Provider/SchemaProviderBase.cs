using System.Data;
using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Provider
{
	public abstract class SchemaProviderBase
	{
		#region Fields

		protected string connectionString;
		protected Database.Database database;

		#endregion Fields

		#region Methods

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