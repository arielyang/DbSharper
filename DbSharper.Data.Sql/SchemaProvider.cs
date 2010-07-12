namespace DbSharper.Data.Sql
{
    using System.Data;

    using DbSharper.Library.Schema;
    using DbSharper.Schema;

    [ProviderName("System.Data.SqlClient")]
    public class SchemaProvider : SchemaProviderBase
    {
        #region Methods

        public override DbType GetDbType(string dbType)
        {
            throw new System.NotImplementedException();
        }

        protected override string GetSchemaSqlText()
        {
            throw new System.NotImplementedException();
        }

        protected override void InitializeDatabase()
        {
            throw new System.NotImplementedException();
        }

        #endregion Methods
    }
}