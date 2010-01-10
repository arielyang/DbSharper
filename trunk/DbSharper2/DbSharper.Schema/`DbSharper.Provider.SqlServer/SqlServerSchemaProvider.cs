using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Text;

using DbSharper.Data.SqlServer;
using DbSharper.Schema.Database;
using DbSharper.Schema.Infrastructure;
using DbSharper.Schema.Provider;
using DbSharper.Schema.Utility;

namespace DbSharper.Schema
{
	[SchemaProvider("System.Data.SqlClient")]
	public class SqlServerSchemaProvider : SchemaProviderBase
	{
		#region Fields

		const string resourcePath = "_DbSharper.Provider.SqlServer.Resources.";

		private string connectionString;
		private Database.Database database;
		private ResourceFileManager resourceFileManager;

		#endregion Fields

		#region Methods

		public override Type GetDatabaseType()
		{
			return typeof(SqlServerDatabase);
		}

		public override DbType GetDbType(string dbTypeString)
		{
			//Value:0 SqlDbType.BigInt DbType.Int64
			//Value:1 SqlDbType.Binary DbType.Binary
			//Value:2 SqlDbType.Bit DbType.Boolean
			//Value:3 SqlDbType.Char DbType.AnsiString
			//Value:4 SqlDbType.DateTime DbType.DateTime
			//Value:5 SqlDbType.Decimal DbType.Decimal
			//Value:6 SqlDbType.Float DbType.Double
			//Value:7 SqlDbType.Image DbType.Binary
			//Value:8 SqlDbType.Int DbType.Int32
			//Value:9 SqlDbType.Money DbType.Currency
			//Value:10 SqlDbType.NChar DbType.String
			//Value:11 SqlDbType.NText DbType.String
			//Value:12 SqlDbType.NVarChar DbType.String
			//Value:13 SqlDbType.Real DbType.Single
			//Value:14 SqlDbType.UniqueIdentifier DbType.Guid
			//Value:15 SqlDbType.SmallDateTime DbType.DateTime
			//Value:16 SqlDbType.SmallInt DbType.Int16
			//Value:17 SqlDbType.SmallMoney DbType.Currency
			//Value:18 SqlDbType.Text DbType.AnsiString
			//Value:19 SqlDbType.Timestamp DbType.Binary
			//Value:20 SqlDbType.TinyInt DbType.Byte
			//Value:21 SqlDbType.VarBinary DbType.Binary
			//Value:22 SqlDbType.VarChar DbType.AnsiString
			//Value:23 SqlDbType.Variant DbType.Object
			//Value:24					 DbType.Object
			//Value:25 SqlDbType.Xml DbType.Xml
			//Value:26					 DbType.String
			//Value:27					 DbType.String
			//Value:28					 DbType.String
			//Value:29 SqlDbType.Udt DbType.Object
			//Value:30 SqlDbType.Structured DbType.Object
			//Value:31 SqlDbType.Date DbType.Date
			//Value:32 SqlDbType.Time DbType.Time
			//Value:33 SqlDbType.DateTime2 DbType.DateTime2
			//Value:34 SqlDbType.DateTimeOffset DbType.DateTimeOffset

			switch (dbTypeString)
			{
				case "bigint":
					// SqlDbType.BigInt;
					return DbType.Int64;
				case "binary":
				case "image":
				case "timestamp":
				case "varbinary":
					// SqlDbType.Binary;
					// SqlDbType.Image;
					// SqlDbType.Timestamp;
					// SqlDbType.VarBinary;
					return DbType.Binary;
				case "bit":
					// SqlDbType.Bit;
					return DbType.Boolean;
				case "char":
				case "text":
				case "varchar":
					// SqlDbType.Char;
					// SqlDbType.Text;
					// SqlDbType.VarChar;
					return DbType.AnsiString;
				case "datetime":
				case "smalldatetime":
					// SqlDbType.DateTime;
					// SqlDbType.SmallDateTime;
					return DbType.DateTime;
				case "decimal":
				case "numeric":
					// SqlDbType.Decimal;
					return DbType.Decimal;
				case "float":
					// SqlDbType.Float;
					return DbType.Double;
				case "int":
					// SqlDbType.Int;
					return DbType.Int32;
				case "money":
				case "smallmoney":
					// SqlDbType.Money;
					// SqlDbType.SmallMoney;
					return DbType.Currency;
				case "nchar":
				case "ntext":
				case "nvarchar":
					// SqlDbType.NChar;
					// SqlDbType.NText;
					// SqlDbType.NVarChar;
					return DbType.String;
				case "real":
					// SqlDbType.Real;
					return DbType.Single;
				case "uniqueidentifier":
					// SqlDbType.UniqueIdentifier;
					return DbType.Guid;
				case "smallint":
					// SqlDbType.SmallInt;
					return DbType.Int16;
				case "tinyint":
					// SqlDbType.TinyInt;
					return DbType.Byte;
				case "sql_variant":
					// SqlDbType.Variant;
					return DbType.Object;
				case "xml":
					// SqlDbType.Xml;
					return DbType.Xml;
				case "date":
					// SqlDbType.Date;
					return DbType.Date;
				case "time":
					// SqlDbType.Time;
					return DbType.Time;
				case "datetime2":
					// SqlDbType.DateTime2;
					return DbType.DateTime2;
				case "datetimeoffset":
					// SqlDbType.DateTimeOffset;
					return DbType.DateTimeOffset;
				default:
					throw new ArgumentException("Unknown dbType.", "dbType");
			}
		}

		public override string BuildParameterSqlName(string parameterName)
		{
			return "@" + parameterName;
		}

		public override string GetParameterName(string parameter)
		{
			//return parameter.TrimStart('@');
			return parameter.Substring(1); // Remove leading '@'.
		}

		public override Database.Database GetSchema(string connectionString)
		{
			this.connectionString = connectionString;

			this.database = new Database.Database();

			this.resourceFileManager = new ResourceFileManager(Assembly.GetExecutingAssembly());

			DataSet ds = this.GetSchemaDataSet();

			this.LoadDatabaseSchema(ds);

			this.InitializeDescriptionForEnumColumn();

			return this.database;
		}

		/// <summary>
		/// Build description of enum-type-column according to description of enum.
		/// </summary>
		/// <param name="description">Origin description of column</param>
		/// <param name="enumeration">Enumeration info</param>
		/// <returns>Description of enum-type-column</returns>
		private static string BuildEnumColumnDescription(string description, Enumeration enumeration)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(description);

			if (sb.Length != 0)
			{
				sb.Append(' ');
			}

			sb.Append(enumeration.Description);

			if (enumeration.Members.Count > 0)
			{
				if (sb.Length != 0)
				{
					sb.Append(": ");
				}

				foreach (var member in enumeration.Members)
				{
					sb.Append(member.Value);
					sb.Append('.');
					sb.Append(member.Description);
					sb.Append(", ");
				}

				sb.Length = sb.Length - 2;
			}

			return sb.ToString();
		}

		private void ExecuteSql(string commandText)
		{
			using (SqlConnection conn = new SqlConnection(this.connectionString))
			{
				using (SqlCommand cmd = new SqlCommand(commandText))
				{
					cmd.Connection = conn;

					conn.Open();

					cmd.ExecuteNonQuery();
				}
			}
		}

		private ParameterDirection GetParameterDirection(string direction)
		{
			switch (direction)
			{
				case "IN":
					return ParameterDirection.Input;
				case "OUT":
					return ParameterDirection.Output;
				case "INOUT":
					return ParameterDirection.InputOutput;
				default:
					throw new ArgumentException("Unknown parameter direction.", "direction");
			}
		}

		private DataSet GetSchemaDataSet()
		{
			string cmdText = resourceFileManager.ReadResourceAsString(resourcePath + "GetDatabaseSchema.sql");

			SqlDataAdapter da = new SqlDataAdapter(cmdText, this.connectionString);

			DataSet ds = new DataSet();
			ds.Locale = CultureInfo.InvariantCulture;

			da.Fill(ds);

			return ds;
		}

		private void InitializeDescriptionForEnumColumn()
		{
			NamedCollection<Enumeration> enumerations = this.database.Enumerations;

			foreach (var table in this.database.Tables)
			{
				foreach (var column in table.Columns)
				{
					if (enumerations.Contains(column.Name))
					{
						column.Description = BuildEnumColumnDescription(column.Description, enumerations[column.Name]);
					}
				}
			}
		}

		private void InitializeEnumTables()
		{
			this.ExecuteSql(resourceFileManager.ReadResourceAsString(resourcePath + "CreateEnumerationTable.sql"));
			this.ExecuteSql(resourceFileManager.ReadResourceAsString(resourcePath + "CreateEnumerationMemberTable.sql"));
		}

		private void LoadDatabaseSchema(DataSet ds)
		{
			DataRowCollection drs;

			Table table;
			View view;
			Procedure procedure;
			ForeignKey foreignKey;
			UniqueKey uniqueKey;
			DbType dbType;
			string specificDbType;
			string constraintName;
			string columnName;
			string key;

			#region [0] Tables

			drs = ds.Tables[0].Rows;

			foreach (DataRow dr in drs)
			{
				this.database.Tables.Add(
					new Table()
					{
						Schema = dr["TABLE_SCHEMA"].ToString(),
						Name = dr["TABLE_NAME"].ToString(),
						Description = string.Empty
					});
			}

			#endregion

			#region [1] Table Columns

			string defaultString;

			drs = ds.Tables[1].Rows;

			foreach (DataRow dr in drs)
			{
				table = this.database.Tables.GetItem(dr["TABLE_SCHEMA"].ToString(), dr["TABLE_NAME"].ToString());

				specificDbType = dr["DATA_TYPE"].ToString();

				dbType = GetDbType(specificDbType);

				defaultString = dr["COLUMN_DEFAULT"].ToString();

				if (defaultString.StartsWith("(", StringComparison.OrdinalIgnoreCase) && defaultString.EndsWith(")", StringComparison.OrdinalIgnoreCase))
				{
					defaultString = defaultString.Substring(1, defaultString.Length - 2);
				}

				table.Columns.Add(
					new Column()
					{
						Name = dr["COLUMN_NAME"].ToString(),
						DbType = dbType,
						SpecificDbType = specificDbType.ToLower(),
						Size = Convert.ToInt32(dr["CHARACTER_MAXIMUM_LENGTH"].ToString(), CultureInfo.InvariantCulture),
						Default = defaultString,
						Nullable = dr["IS_NULLABLE"].ToString() == "YES",
						Description = string.Empty,
					});
			}

			#endregion

			#region [2] Constraints

			drs = ds.Tables[2].Rows;

			foreach (DataRow dr in drs)
			{
				constraintName = dr["CONSTRAINT_NAME"].ToString();
				columnName = dr["COLUMN_NAME"].ToString();

				if (this.database.Tables.Contains(dr["TABLE_SCHEMA"].ToString(), dr["TABLE_NAME"].ToString()))
				{
					table = this.database.Tables.GetItem(dr["TABLE_SCHEMA"].ToString(), dr["TABLE_NAME"].ToString());

					switch (dr["CONSTRAINT_TYPE"].ToString())
					{
						case "PRIMARY KEY":
							{
								table.PrimaryKey.Name = constraintName;
								table.PrimaryKey.Columns.Add(table.Columns[columnName]);

								break;
							}
						case "FOREIGN KEY":
							{
								if (table.ForeignKeys.Contains(constraintName))
								{
									table.ForeignKeys[constraintName].Columns.Add(table.Columns[columnName]);
								}
								else
								{
									foreignKey = new ForeignKey() { Name = constraintName };
									foreignKey.Columns.Add(table.Columns[columnName]);

									if (this.database.Tables.Contains(dr["REFERENTIAL_TABLE_SCHEMA"].ToString(), dr["REFERENTIAL_TABLE_NAME"].ToString()))
									{
										foreignKey.ReferentialTableName = dr["REFERENTIAL_TABLE_SCHEMA"].ToString() + "." + dr["REFERENTIAL_TABLE_NAME"].ToString();
									}

									table.ForeignKeys.Add(foreignKey);
								}

								break;
							}
						case "UNIQUE KEY":
							{
								if (table.UniqueKeys.Contains(constraintName))
								{
									table.UniqueKeys[constraintName].Columns.Add(table.Columns[columnName]);
								}
								else
								{
									uniqueKey = new UniqueKey() { Name = constraintName };
									uniqueKey.Columns.Add(table.Columns[columnName]);

									table.UniqueKeys.Add(uniqueKey);
								}

								break;
							}
					}
				}
			}

			#endregion

			#region [3] Table Indexes

			drs = ds.Tables[3].Rows;

			foreach (DataRow dr in drs)
			{
				table = this.database.Tables.GetItem(dr["TABLE_SCHEMA"].ToString(), dr["TABLE_NAME"].ToString());

				key = dr["INDEX_NAME"].ToString();

				if (table.Indexes.Contains(key))
				{
					table.Indexes[key].Columns.Add(table.Columns[dr["COLUMN_NAME"].ToString()]);

				}
				else
				{
					table.Indexes.Add(
						new Index()
						{
							Name = key
						});
				}
			}

			#endregion

			#region [4] Views

			drs = ds.Tables[4].Rows;

			foreach (DataRow dr in drs)
			{
				this.database.Views.Add(
					new View()
					{
						Schema = dr["TABLE_SCHEMA"].ToString(),
						Name = dr["TABLE_NAME"].ToString()
					});
			}

			#endregion

			#region [5] View Columns

			drs = ds.Tables[5].Rows;

			foreach (DataRow dr in drs)
			{
				view = this.database.Views.GetItem(dr["TABLE_SCHEMA"].ToString(), dr["TABLE_NAME"].ToString());

				specificDbType = dr["DATA_TYPE"].ToString();

				dbType = GetDbType(specificDbType);

				view.Columns.Add(
					new Column()
					{
						Name = dr["COLUMN_NAME"].ToString(),
						DbType = dbType,
						SpecificDbType = specificDbType,
						Size = Convert.ToInt32(dr["CHARACTER_MAXIMUM_LENGTH"].ToString(), CultureInfo.InvariantCulture),
						Nullable = dr["IS_NULLABLE"].ToString() == "YES",
						Description = string.Empty
					});
			}

			#endregion

			#region [6] View Indexes

			drs = ds.Tables[6].Rows;

			foreach (DataRow dr in drs)
			{
				view = this.database.Views.GetItem(dr["TABLE_SCHEMA"].ToString(), dr["TABLE_NAME"].ToString());

				key = dr["INDEX_NAME"].ToString();

				if (view.Indexes.Contains(key))
				{
					view.Indexes[key].Columns.Add(view.Columns[dr["COLUMN_NAME"].ToString()]);
				}
				else
				{
					view.Indexes.Add(
						new Index()
						{
							Name = key
						});
				}
			}

			#endregion

			#region [7] Procedures

			drs = ds.Tables[7].Rows;

			foreach (DataRow dr in drs)
			{
				this.database.Procedures.Add(
					new Procedure()
					{
						Schema = dr["SPECIFIC_SCHEMA"].ToString(),
						Name = dr["SPECIFIC_NAME"].ToString(),
						Definition = dr["ROUTINE_DEFINITION"].ToString(),
						Description = string.Empty
					});
			}

			#endregion

			#region [8] Procedure Parameters

			drs = ds.Tables[8].Rows;

			foreach (DataRow dr in drs)
			{
				if (this.database.Procedures.Contains(dr["SPECIFIC_SCHEMA"].ToString(), dr["SPECIFIC_NAME"].ToString()))
				{
					procedure = this.database.Procedures.GetItem(dr["SPECIFIC_SCHEMA"].ToString(), dr["SPECIFIC_NAME"].ToString());

					specificDbType = dr["DATA_TYPE"].ToString();
					dbType = GetDbType(specificDbType);

					procedure.Parameters.Add(
						new Database.Parameter()
						{
							Name = dr["PARAMETER_NAME"].ToString(),
							DbType = GetDbType(specificDbType),
							SpecificDbType = specificDbType,
							Size = Convert.ToInt32(dr["CHARACTER_MAXIMUM_LENGTH"].ToString(), CultureInfo.InvariantCulture),
							Direction = GetParameterDirection(dr["PARAMETER_MODE"].ToString())
						});
				}
			}

			#endregion

			#region [9] Descriptions

			string schema;
			string majorName;
			string minorName;
			string value;

			drs = ds.Tables[9].Rows;

			foreach (DataRow dr in drs)
			{
				schema = dr["Schema"].ToString();
				majorName = dr["MajorName"].ToString();
				minorName = dr["MinorName"].ToString();
				value = dr["Value"].ToString() ?? string.Empty;

				switch (dr["BaseType"].ToString().Trim())
				{
					case "U":
						{
							if (string.IsNullOrEmpty(minorName))
							{
								this.database.Tables.GetItem(schema, majorName).Description = value;
							}
							else
							{
								this.database.Tables.GetItem(schema, majorName).Columns[minorName].Description = value;
							}

							break;
						}
					case "V":
						{
							this.database.Views.GetItem(schema, majorName).Description = value;

							break;
						}
					case "P":
						{
							this.database.Procedures.GetItem(schema, majorName).Description = value;

							break;
						}
				}
			}

			#endregion

			#region [10] Enumeratioins

			drs = ds.Tables[10].Rows;

			if (drs.Count > 0)
			{
				if (drs[0][0].ToString() == "-1")
				{
					InitializeEnumTables();

					return;
				}
			}

			foreach (DataRow dr in drs)
			{
				this.database.Enumerations.Add(
					new Enumeration
					{
						Name = dr["Name"].ToString(),
						BaseType = dr["BaseType"].ToString(),
						HasFlagsAttribute = Convert.ToBoolean(dr["HasFlagsAttribute"].ToString(), CultureInfo.InvariantCulture),
						Description = dr["Description"].ToString() ?? string.Empty
					});
			}

			#endregion

			#region [11] EnumeratioinMembers

			drs = ds.Tables[11].Rows;

			foreach (DataRow dr in drs)
			{
				this.database.Enumerations[dr["EnumerationName"].ToString()].Members.Add(
					new EnumerationMember
					{
						Name = dr["Name"].ToString(),
						Value = Convert.ToInt32(dr["Value"].ToString(), CultureInfo.InvariantCulture),
						Description = dr["Description"].ToString() ?? string.Empty
					});
			}

			#endregion
		}

		#endregion Methods
	}
}