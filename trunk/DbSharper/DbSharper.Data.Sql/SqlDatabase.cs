////namespace DbSharper.Data.Sql
////{
////    using System.Data.SqlClient;

////    using DbSharper.Library.Data;

////    /// <summary>
////    /// <para>Represents a SQL Server database.</para>
////    /// </summary>
////    /// <remarks> 
////    /// <para>
////    /// Internally uses SQL Server .NET Managed Provider from Microsoft (System.Data.SqlClient) to connect to the database.
////    /// </para>  
////    /// </remarks>
////    public class SqlDatabase : Database
////    {
////        #region Constructors

////        /// <summary>
////        /// Initializes a new instance of the <see cref="SqlDatabase"/> class with a connection string.
////        /// </summary>
////        public SqlDatabase()
////            : base(SqlClientFactory.Instance)
////        {
////        }

////        #endregion Constructors

////        #region Methods

////        /// <summary>
////        /// Builds a value parameter name for the current database.
////        /// </summary>
////        /// <param name="name">The name of the parameter.</param>
////        /// <returns>A correctly formated parameter name.</returns>
////        public override string BuildParameterName(string name)
////        {
////            if (name[0] != '@')
////            {
////                return "@" + name;
////            }

////            return name;
////        }

////        #endregion Methods
////    }
////}