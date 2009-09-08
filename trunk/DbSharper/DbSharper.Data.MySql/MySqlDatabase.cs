////namespace DbSharper.Data.MySql
////{
////    using DbSharper.Library.Data;

////    using global::MySql.Data.MySqlClient;

////    /// <summary>
////    /// <para>Represents a MySql database.</para>
////    /// </summary>
////    /// <remarks> 
////    /// <para>
////    /// Internally uses MySql .NET Managed Provider from MySql (MySql.Data.MySqlClient) to connect to the database.
////    /// </para>  
////    /// </remarks>
////    public class MySqlDatabase : Database
////    {
////        #region Constructors

////        /// <summary>
////        /// Initializes a new instance of the <see cref="MySqlDatabase"/> class with a connection string.
////        /// </summary>
////        public MySqlDatabase()
////            : base(MySqlClientFactory.Instance)
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
////            if (name[0] != '?')
////            {
////                return "?" + name;
////            }

////            return name;
////        }

////        #endregion Methods
////    }
////}