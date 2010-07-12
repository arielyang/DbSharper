#region Header

//namespace DbSharper.Library.Data
//{
//	using System;
//	using System.Data;
//	using System.Data.Common;
//	using System.Transactions;
//	using DbSharper.Library.Properties;
//	/// <summary>
//	/// Represents an abstract database that commands can be run against.
//	/// </summary>
//	/// <remarks>
//	/// The <see cref="Database"/> class leverages the provider factory model from ADO.NET. A database instance holds
//	/// a reference to a concrete <see cref="DbProviderFactory"/> object to which it forwards the creation of ADO.NET objects.
//	/// </remarks>
//	public abstract class Database
//	{
//		#region Fields
//		private readonly DbProviderFactory dbProviderFactory;
//		private string connectionString;
//		#endregion Fields
//		#region Constructors
//		/// <summary>
//		/// Initializes a new instance of the <see cref="Database"/> class with a connection string and a <see cref="DbProviderFactory"/>.
//		/// </summary>
//		/// <param name="dbProviderFactory">A <see cref="DbProviderFactory"/> object.</param>
//		protected Database(DbProviderFactory dbProviderFactory)
//		{
//			if (dbProviderFactory == null) throw new ArgumentNullException("dbProviderFactory");
//			this.dbProviderFactory = dbProviderFactory;
//		}
//		#endregion Constructors
//		#region Properties
//		/// <summary>
//		/// <para>Gets or sets the string used to open a database.</para>
//		/// </summary>
//		/// <value>
//		/// <para>The string used to open a database.</para>
//		/// </value>
//		/// <seealso cref="DbConnection.ConnectionString"/>
//		public string ConnectionString
//		{
//			get { return this.connectionString; }
//			set { this.connectionString = value; }
//		}
//		#endregion Properties
//		#region Methods
//		/// <summary>
//		/// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
//		/// </summary>
//		/// <param name="command">The commmand to add the parameter.</param>
//		/// <param name="name"><para>The name of the parameter.</para></param>
//		/// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
//		/// <param name="value"><para>The value of the parameter.</para></param>
//		public void AddInParameter(DbCommand command,
//			string name,
//			DbType dbType,
//			object value)
//		{
//			AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
//		}
//		/// <summary>
//		/// Adds a new Out <see cref="DbParameter"/> object to the given <paramref name="command"/>.
//		/// </summary>
//		/// <param name="command">The command to add the out parameter.</param>
//		/// <param name="name"><para>The name of the parameter.</para></param>
//		/// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
//		/// <param name="size"><para>The maximum size of the data within the column.</para></param>
//		public void AddOutParameter(DbCommand command,
//			string name,
//			DbType dbType,
//			int size)
//		{
//			AddParameter(command, name, dbType, size, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
//		}
//		/// <summary>
//		/// Adds a new Out <see cref="DbParameter"/> object to the given <paramref name="command"/>.
//		/// </summary>
//		/// <param name="command">The command to add the out parameter.</param>
//		/// <param name="name"><para>The name of the parameter.</para></param>
//		/// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
//		public void AddOutParameter(DbCommand command,
//			string name,
//			DbType dbType)
//		{
//			AddOutParameter(command, name, dbType, 0);
//		}
//		/// <summary>
//		/// Builds a value parameter name for the current database.
//		/// </summary>
//		/// <param name="name">The name of the parameter.</param>
//		/// <returns>A correctly formated parameter name.</returns>
//		public virtual string BuildParameterName(string name)
//		{
//			return name;
//		}
//		/// <summary>
//		/// <para>Creates a connection for this database.</para>
//		/// </summary>
//		/// <returns>
//		/// <para>The <see cref="DbConnection"/> for this database.</para>
//		/// </returns>
//		/// <seealso cref="DbConnection"/>
//		public virtual DbConnection CreateConnection()
//		{
//			DbConnection newConnection = dbProviderFactory.CreateConnection();
//			newConnection.ConnectionString = ConnectionString;
//			return newConnection;
//		}
//		/// <summary>
//		/// <para>Executes the <paramref name="command"/> and returns the number of rows affected.</para>
//		/// </summary>
//		/// <param name="command">
//		/// <para>The command that contains the query to execute.</para>
//		/// </param>
//		/// <seealso cref="IDbCommand.ExecuteScalar"/>
//		public virtual int ExecuteNonQuery(DbCommand command)
//		{
//			using (ConnectionWrapper wrapper = GetOpenConnection())
//			{
//				PrepareCommand(command, wrapper.Connection);
//				return DoExecuteNonQuery(command);
//			}
//		}
//		/// <summary>
//		/// <para>Executes the <paramref name="command"/> within the given <paramref name="transaction" />, and returns the number of rows affected.</para>
//		/// </summary>
//		/// <param name="command">
//		/// <para>The command that contains the query to execute.</para>
//		/// </param>
//		/// <param name="transaction">
//		/// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
//		/// </param>
//		/// <seealso cref="IDbCommand.ExecuteScalar"/>
//		public virtual int ExecuteNonQuery(DbCommand command,
//			DbTransaction transaction)
//		{
//			PrepareCommand(command, transaction);
//			return DoExecuteNonQuery(command);
//		}
//		/// <summary>
//		/// <para>Executes the <paramref name="command"/> and returns an <see cref="IDataReader"></see> through which the result can be read.
//		/// It is the responsibility of the caller to close the connection and reader when finished.</para>
//		/// </summary>
//		/// <param name="command">
//		/// <para>The command that contains the query to execute.</para>
//		/// </param>
//		/// <returns>
//		/// <para>An <see cref="IDataReader"/> object.</para>
//		/// </returns>
//		public virtual IDataReader ExecuteReader(DbCommand command)
//		{
//			ConnectionWrapper wrapper = GetOpenConnection(false);
//			try
//			{
//				//
//				// JS-L: I moved the PrepareCommand inside the try because it can fail.
//				//
//				PrepareCommand(command, wrapper.Connection);
//				//
//				// If there is a current transaction, we'll be using a shared connection, so we don't
//				// want to close the connection when we're done with the reader.
//				//
//				if (Transaction.Current != null)
//					return DoExecuteReader(command, CommandBehavior.Default);
//				else
//					return DoExecuteReader(command, CommandBehavior.CloseConnection);
//			}
//			catch
//			{
//				wrapper.Connection.Close();
//				throw;
//			}
//		}
//		/// <summary>
//		/// <para>Executes the <paramref name="command"/> within a transaction and returns an <see cref="IDataReader"></see> through which the result can be read.
//		/// It is the responsibility of the caller to close the connection and reader when finished.</para>
//		/// </summary>
//		/// <param name="command">
//		/// <para>The command that contains the query to execute.</para>
//		/// </param>
//		/// <param name="transaction">
//		/// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
//		/// </param>
//		/// <returns>
//		/// <para>An <see cref="IDataReader"/> object.</para>
//		/// </returns>
//		public virtual IDataReader ExecuteReader(DbCommand command,
//			DbTransaction transaction)
//		{
//			PrepareCommand(command, transaction);
//			return DoExecuteReader(command, CommandBehavior.Default);
//		}
//		/// <summary>
//		/// <para>Creates a <see cref="DbCommand"/> for a SQL query.</para>
//		/// </summary>
//		/// <param name="query"><para>The text of the query.</para></param>
//		/// <returns><para>The <see cref="DbCommand"/> for the SQL query.</para></returns>
//		public DbCommand GetSqlStringCommand(string query)
//		{
//			if (string.IsNullOrEmpty(query)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "query");
//			return CreateCommandByCommandType(CommandType.Text, query);
//		}
//		/// <summary>
//		/// <para>Creates a <see cref="DbCommand"/> for a stored procedure.</para>
//		/// </summary>
//		/// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
//		/// <returns><para>The <see cref="DbCommand"/> for the stored procedure.</para></returns>
//		public virtual DbCommand GetStoredProcCommand(string storedProcedureName)
//		{
//			if (string.IsNullOrEmpty(storedProcedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "storedProcedureName");
//			return CreateCommandByCommandType(CommandType.StoredProcedure, storedProcedureName);
//		}
//		internal DbConnection GetNewOpenConnection()
//		{
//			DbConnection connection = null;
//			try
//			{
//				connection = CreateConnection();
//				connection.Open();
//			}
//			catch
//			{
//				if (connection != null)
//					connection.Close();
//				throw;
//			}
//			return connection;
//		}
//		/// <summary>
//		/// Executes the query for <paramref name="command"/>.
//		/// </summary>
//		/// <param name="command">The <see cref="DbCommand"/> representing the query to execute.</param>
//		/// <returns>The quantity of rows affected.</returns>
//		protected static int DoExecuteNonQuery(DbCommand command)
//		{
//			int rowsAffected = command.ExecuteNonQuery();
//			return rowsAffected;
//		}
//		/// <summary>
//		/// <para>Assigns a <paramref name="connection"/> to the <paramref name="command"/> and discovers parameters if needed.</para>
//		/// </summary>
//		/// <param name="command"><para>The command that contains the query to prepare.</para></param>
//		/// <param name="connection">The connection to assign to the command.</param>
//		protected static void PrepareCommand(DbCommand command,
//			DbConnection connection)
//		{
//			if (command == null) throw new ArgumentNullException("command");
//			if (connection == null) throw new ArgumentNullException("connection");
//			command.Connection = connection;
//		}
//		/// <summary>
//		/// <para>Assigns a <paramref name="transaction"/> to the <paramref name="command"/> and discovers parameters if needed.</para>
//		/// </summary>
//		/// <param name="command"><para>The command that contains the query to prepare.</para></param>
//		/// <param name="transaction">The transaction to assign to the command.</param>
//		protected static void PrepareCommand(DbCommand command,
//			DbTransaction transaction)
//		{
//			if (command == null) throw new ArgumentNullException("command");
//			if (transaction == null) throw new ArgumentNullException("transaction");
//			PrepareCommand(command, transaction.Connection);
//			command.Transaction = transaction;
//		}
//		/// <summary>
//		/// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
//		/// </summary>
//		/// <param name="command">The command to add the parameter.</param>
//		/// <param name="name"><para>The name of the parameter.</para></param>
//		/// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
//		/// <param name="size"><para>The maximum size of the data within the column.</para></param>
//		/// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
//		/// <param name="nullable"><para>Avalue indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
//		/// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
//		/// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
//		/// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
//		/// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
//		/// <param name="value"><para>The value of the parameter.</para></param>
//		protected virtual void AddParameter(DbCommand command,
//			string name,
//			DbType dbType,
//			int size,
//			ParameterDirection direction,
//			bool nullable,
//			byte precision,
//			byte scale,
//			string sourceColumn,
//			DataRowVersion sourceVersion,
//			object value)
//		{
//			DbParameter parameter = CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
//			command.Parameters.Add(parameter);
//		}
//		/// <summary>
//		/// Configures a given <see cref="DbParameter"/>.
//		/// </summary>
//		/// <param name="param">The <see cref="DbParameter"/> to configure.</param>
//		/// <param name="name"><para>The name of the parameter.</para></param>
//		/// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
//		/// <param name="size"><para>The maximum size of the data within the column.</para></param>
//		/// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
//		/// <param name="nullable"><para>Avalue indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
//		/// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
//		/// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
//		/// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
//		/// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
//		/// <param name="value"><para>The value of the parameter.</para></param>
//		protected virtual void ConfigureParameter(DbParameter param,
//			string name,
//			DbType dbType,
//			int size,
//			ParameterDirection direction,
//			bool nullable,
//			byte precision,
//			byte scale,
//			string sourceColumn,
//			DataRowVersion sourceVersion,
//			object value)
//		{
//			param.DbType = dbType;
//			param.Size = size;
//			param.Value = value ?? DBNull.Value;
//			param.Direction = direction;
//			param.IsNullable = nullable;
//			param.SourceColumn = sourceColumn;
//			param.SourceVersion = sourceVersion;
//		}
//		/// <summary>
//		/// <para>Adds a new instance of a <see cref="DbParameter"/> object.</para>
//		/// </summary>
//		/// <param name="name"><para>The name of the parameter.</para></param>
//		/// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
//		/// <param name="size"><para>The maximum size of the data within the column.</para></param>
//		/// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
//		/// <param name="nullable"><para>Avalue indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
//		/// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
//		/// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
//		/// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
//		/// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
//		/// <param name="value"><para>The value of the parameter.</para></param>
//		/// <returns>A newly created <see cref="DbParameter"/> fully initialized with given parameters.</returns>
//		protected DbParameter CreateParameter(string name,
//			DbType dbType,
//			int size,
//			ParameterDirection direction,
//			bool nullable,
//			byte precision,
//			byte scale,
//			string sourceColumn,
//			DataRowVersion sourceVersion,
//			object value)
//		{
//			DbParameter param = CreateParameter(name);
//			ConfigureParameter(param, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
//			return param;
//		}
//		/// <summary>
//		/// <para>Adds a new instance of a <see cref="DbParameter"/> object.</para>
//		/// </summary>
//		/// <param name="name"><para>The name of the parameter.</para></param>
//		/// <returns><para>An unconfigured parameter.</para></returns>
//		protected DbParameter CreateParameter(string name)
//		{
//			DbParameter param = dbProviderFactory.CreateParameter();
//			param.ParameterName = BuildParameterName(name);
//			return param;
//		}
//		/// <summary>
//		///		Get's a "wrapped" connection that will be not be disposed if a transaction is
//		///		active (created by creating a <see cref="TransactionScope"/> instance). The
//		///		connection will be disposed when no transaction is active.
//		/// </summary>
//		/// <returns></returns>
//		protected ConnectionWrapper GetOpenConnection()
//		{
//			return GetOpenConnection(true);
//		}
//		/// <summary>
//		///		Get's a "wrapped" connection that will be not be disposed if a transaction is
//		///		active (created by creating a <see cref="TransactionScope"/> instance). The
//		///		connection can be disposed when no transaction is active.
//		/// </summary>
//		/// <param name="disposeInnerConnection">
//		///		Determines if the connection will be disposed when there isn't an active
//		///		transaction.
//		/// </param>
//		/// <returns>The wrapped connection.</returns>
//		protected ConnectionWrapper GetOpenConnection(bool disposeInnerConnection)
//		{
//			DbConnection connection = TransactionScopeConnections.GetConnection(this);
//			if (connection != null)
//				return new ConnectionWrapper(connection, false);
//			else
//				return new ConnectionWrapper(GetNewOpenConnection(), disposeInnerConnection);
//		}
//		private static IDataReader DoExecuteReader(DbCommand command,
//			CommandBehavior cmdBehavior)
//		{
//			IDataReader reader = command.ExecuteReader(cmdBehavior);
//			return reader;
//		}
//		/// <summary>
//		/// <para>Adds a new instance of a <see cref="DbParameter"/> object to the command.</para>
//		/// </summary>
//		/// <param name="command">The command to add the parameter.</param>
//		/// <param name="name"><para>The name of the parameter.</para></param>
//		/// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
//		/// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
//		/// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
//		/// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
//		/// <param name="value"><para>The value of the parameter.</para></param>
//		private void AddParameter(DbCommand command,
//			string name,
//			DbType dbType,
//			ParameterDirection direction,
//			string sourceColumn,
//			DataRowVersion sourceVersion,
//			object value)
//		{
//			AddParameter(command, name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
//		}
//		private DbCommand CreateCommandByCommandType(CommandType commandType,
//			string commandText)
//		{
//			DbCommand command = dbProviderFactory.CreateCommand();
//			command.CommandType = commandType;
//			command.CommandText = commandText;
//			return command;
//		}
//		#endregion Methods
//		#region Nested Types
//		/// <summary>
//		///		This is a helper class that is used to manage the lifetime of a connection for various
//		///		Execute methods. We needed this class to support implicit transactions created with
//		///		the <see cref="TransactionScope"/> class. In this case, the various Execute methods
//		///		need to use a shared connection instead of a new connection for each request in order
//		///		to prevent a distributed transaction.
//		/// </summary>
//		protected sealed class ConnectionWrapper : IDisposable
//		{
//			#region Fields
//			readonly DbConnection connection;
//			readonly bool disposeConnection;
//			#endregion Fields
//			#region Constructors
//			/// <summary>
//			///		Create a new "lifetime" container for a <see cref="DbConnection"/> instance.
//			/// </summary>
//			/// <param name="connection">The connection</param>
//			/// <param name="disposeConnection">
//			///		Whether or not to dispose of the connection when this class is disposed.
//			///	</param>
//			public ConnectionWrapper(DbConnection connection,
//				bool disposeConnection)
//			{
//				this.connection = connection;
//				this.disposeConnection = disposeConnection;
//			}
//			#endregion Constructors
//			#region Properties
//			/// <summary>
//			///		Gets the actual connection.
//			/// </summary>
//			public DbConnection Connection
//			{
//				get { return connection; }
//			}
//			#endregion Properties
//			#region Methods
//			/// <summary>
//			///		Dispose the wrapped connection, if appropriate.
//			/// </summary>
//			public void Dispose()
//			{
//				if (disposeConnection)
//					connection.Dispose();
//			}
//			#endregion Methods
//		}
//		#endregion Nested Types
//	}
//}

#endregion Header