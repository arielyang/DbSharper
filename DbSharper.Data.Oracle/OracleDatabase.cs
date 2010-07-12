////namespace DbSharper.Data.Sql
////{
////    using System.Data.OracleClient;
////    using DbSharper.Library.Data;
////    /// <summary>
////    /// <para>Represents a Oracle database.</para>
////    /// </summary>
////    /// <remarks> 
////    /// <para>
////    /// Internally uses Oracle .NET Managed Provider from Microsoft (System.Data.OracleClient) to connect to the database.
////    /// </para>  
////    /// </remarks>
////    public class OracleDatabase : Database
////    {
////        #region Constructors
////        /// <summary>
////        /// Initializes a new instance of the <see cref="OracleDatabase"/> class with a connection string.
////        /// </summary>
////        public OracleDatabase()
////            : base(OracleClientFactory.Instance)
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