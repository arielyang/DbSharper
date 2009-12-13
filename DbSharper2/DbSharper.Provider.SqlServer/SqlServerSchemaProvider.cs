using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

using DbSharper.Schema.Code;
using DbSharper.Schema.Database;
using DbSharper.Schema.Infrastructure;
using DbSharper.Schema.Utility;
using DbSharper.Schema.Provider;

namespace DbSharper.Schema
{
	public class SqlServerSchemaProvider : SchemaProviderBase
	{
		#region Fields

		private string connectionString;
		private Database.Database database;
		private ResourceFileManager resourceFileManager;

		#endregion Fields

		#region Methods

		public Database.Database GetSchema(string connectionString)
		{
			this.connectionString = connectionString;

			this.database = new Database.Database();

			this.resourceFileManager = new ResourceFileManager();

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

		private DataSet GetSchemaDataSet()
		{
			string cmdText = resourceFileManager.ReadResourceString("Resources.GetDatabaseSchema.sql");

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
			this.ExecuteSql(resourceFileManager.ReadResourceString("Resources.CreateEnumerationTable.sql"));
			this.ExecuteSql(resourceFileManager.ReadResourceString("Resources.CreateEnumerationMemberTable.sql"));
		}

		private void LoadDatabaseSchema(DataSet ds)
		{
			DataRowCollection drs;

			Table table;
			View view;
			Procedure procedure;
			ForeignKey foreignKey;
			UniqueKey uniqueKey;
			SqlDbType sqlDbType;
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

				sqlDbType = MappingHelper.GetSqlDbType(dr["DATA_TYPE"].ToString());

				defaultString = dr["COLUMN_DEFAULT"].ToString();

				if (defaultString.StartsWith("(", StringComparison.OrdinalIgnoreCase) && defaultString.EndsWith(")", StringComparison.OrdinalIgnoreCase))
				{
					defaultString = defaultString.Substring(1, defaultString.Length - 2);
				}

				table.Columns.Add(
					new Column()
					{
						Name = dr["COLUMN_NAME"].ToString(),
						SqlDbType = sqlDbType,
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
										foreignKey.ReferentialTable = dr["REFERENTIAL_TABLE_SCHEMA"].ToString() + "." + dr["REFERENTIAL_TABLE_NAME"].ToString();
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

				sqlDbType = MappingHelper.GetSqlDbType(dr["DATA_TYPE"].ToString());

				view.Columns.Add(
					new Column()
					{
						Name = dr["COLUMN_NAME"].ToString(),
						SqlDbType = sqlDbType,
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

					sqlDbType = MappingHelper.GetSqlDbType(dr["DATA_TYPE"].ToString());

					procedure.Parameters.Add(
						new Parameter()
						{
							Name = dr["PARAMETER_NAME"].ToString(),
							SqlDbType = sqlDbType,
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

		private DbType GetDbType(string dbTypeString)
		{
			return DbType.AnsiString;
		}

		/// <summary>
		/// Get SqlDbType enum according to sqlDbType string from database.
		/// </summary>
		/// <param name="sqlDbType">SqlDbType string from database</param>
		/// <returns>SqlDbType enum</returns>
		public static SqlDbType GetSqlDbType(string sqlDbType)
		{
			switch (sqlDbType)
			{
				case "bigint":
					return SqlDbType.BigInt;
				case "binary":
					return SqlDbType.Binary;
				case "bit":
					return SqlDbType.Bit;
				case "char":
					return SqlDbType.Char;
				case "datetime":
					return SqlDbType.DateTime;
				case "decimal":
				case "numeric":
					return SqlDbType.Decimal;
				case "float":
					return SqlDbType.Float;
				case "image":
					return SqlDbType.Image;
				case "int":
					return SqlDbType.Int;
				case "money":
					return SqlDbType.Money;
				case "nchar":
					return SqlDbType.NChar;
				case "ntext":
					return SqlDbType.NText;
				case "nvarchar":
					return SqlDbType.NVarChar;
				case "real":
					return SqlDbType.Real;
				case "uniqueidentifier":
					return SqlDbType.UniqueIdentifier;
				case "smalldatetime":
					return SqlDbType.SmallDateTime;
				case "smallint":
					return SqlDbType.SmallInt;
				case "smallmoney":
					return SqlDbType.SmallMoney;
				case "text":
					return SqlDbType.Text;
				case "timestamp":
					return SqlDbType.Timestamp;
				case "tinyint":
					return SqlDbType.TinyInt;
				case "varbinary":
					return SqlDbType.VarBinary;
				case "varchar":
					return SqlDbType.VarChar;
				case "sql_variant":
					return SqlDbType.Variant;
				case "xml":
					return SqlDbType.Xml;
				case "date":
					return SqlDbType.Date;
				case "time":
					return SqlDbType.Time;
				case "datetime2":
					return SqlDbType.DateTime2;
				case "datetimeoffset":
					return SqlDbType.DateTimeOffset;
				default:
					throw new ArgumentException("Unknown sqlDbType.", "sqlDbType");
			}
		}



		#endregion Methods

		public override string GetParameterName(string parameter)
		{
			return parameter.TrimStart('@');
		}
	}
}