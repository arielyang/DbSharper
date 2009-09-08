namespace DbSharper.Schema
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;

    using DbSharper.Schema.Collections;
    using DbSharper.Schema.Database;

	public class SchemaProvider
    {
        #region Fields

        private string m_connectionString;
        private Database.Database database;

        #endregion Fields

        #region Methods

        public Database.Database GetSchema(string connectionString)
        {
            this.m_connectionString = connectionString;

            this.InitializeEnumTables();

            this.database = new Database.Database();

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

        private static string GetSchemaSqlText()
        {
            StringBuilder sb = new StringBuilder();

            // 0.Get tables.
            sb.AppendLine(@"SELECT
                                TABLE_SCHEMA,
                                TABLE_NAME
                            FROM INFORMATION_SCHEMA.TABLES
                            WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME <> 'sysdiagrams'
                            ORDER BY TABLE_SCHEMA, TABLE_NAME;");

            // 1.Get table columns.
            sb.AppendLine(@"SELECT
                                C.TABLE_SCHEMA,
                                C.TABLE_NAME,
                                C.COLUMN_NAME,
                                C.DATA_TYPE,
                                ISNULL(C.CHARACTER_MAXIMUM_LENGTH, 0) AS CHARACTER_MAXIMUM_LENGTH,
                                C.COLUMN_DEFAULT,
                                C.IS_NULLABLE
                            FROM INFORMATION_SCHEMA.COLUMNS C
                            INNER JOIN INFORMATION_SCHEMA.TABLES T ON T.TABLE_SCHEMA = C.TABLE_SCHEMA AND T.TABLE_NAME = C.TABLE_NAME
                            WHERE T.TABLE_TYPE = 'BASE TABLE' AND T.TABLE_NAME <> 'sysdiagrams'
                            ORDER BY C.TABLE_SCHEMA, C.TABLE_NAME, C.ORDINAL_POSITION;");

            // 2.Get constraints.
            sb.AppendLine(@"SELECT
                                C.TABLE_SCHEMA,
                                C.TABLE_NAME,
                                C.CONSTRAINT_TYPE,
                                C.CONSTRAINT_NAME,
                                U.COLUMN_NAME,
                                R.TABLE_NAME AS REFERENTIAL_TABLE_NAME,
                                R.TABLE_SCHEMA AS REFERENTIAL_TABLE_SCHEMA
                            FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS C
                            LEFT OUTER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE U ON C.TABLE_SCHEMA = U.TABLE_SCHEMA AND C.TABLE_NAME = U.TABLE_NAME AND C.CONSTRAINT_NAME = U.CONSTRAINT_NAME
                            LEFT OUTER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS F ON C.CONSTRAINT_NAME = F.CONSTRAINT_NAME
                            LEFT OUTER JOIN INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE R ON F.UNIQUE_CONSTRAINT_NAME = R.CONSTRAINT_NAME
                            WHERE
								CONSTRAINT_TYPE <> 'CHECK' AND
								C.TABLE_NAME <> 'sysdiagrams'
                            ORDER BY C.TABLE_SCHEMA, C.TABLE_NAME, C.CONSTRAINT_NAME, U.ORDINAL_POSITION;");

            // 3.Get table indexes.
            sb.AppendLine(@"SELECT
                                OBJECT_SCHEMA_NAME(C.[object_id]) AS TABLE_SCHEMA,
                                OBJECT_NAME(C.[object_id]) AS TABLE_NAME,
                                I.name AS INDEX_NAME,
                                S.name AS COLUMN_NAME
                            FROM sys.index_columns C
                            LEFT OUTER JOIN sys.indexes I ON C.[object_id] = I.[object_id] AND C.index_id = I.index_id
                            LEFT OUTER JOIN sys.columns S ON C.[object_id] = S.[object_id] AND C.column_id = S.column_id
                            WHERE
                                OBJECTPROPERTY(C.[object_id], 'IsUserTable') = 1 AND
                                OBJECT_NAME(C.[object_id]) <> 'sysdiagrams' AND
                                I.[type] in (1, 2) AND
                                I.is_primary_key = 0
                            ORDER BY
                                OBJECT_SCHEMA_NAME(C.[object_id]),
                                OBJECT_NAME(C.[object_id]),
                                C.index_id;");

            // 4.Get views.
            sb.AppendLine(@"SELECT
                                TABLE_SCHEMA,
                                TABLE_NAME
                            FROM INFORMATION_SCHEMA.TABLES
                            WHERE TABLE_TYPE = 'VIEW'
                            ORDER BY TABLE_SCHEMA, TABLE_NAME;");

            // 5.Get view columns.
            sb.AppendLine(@"SELECT
                                C.TABLE_SCHEMA,
                                C.TABLE_NAME,
                                C.COLUMN_NAME,
                                C.DATA_TYPE,
                                ISNULL(C.CHARACTER_MAXIMUM_LENGTH, 0) AS CHARACTER_MAXIMUM_LENGTH,
                                C.IS_NULLABLE
                            FROM INFORMATION_SCHEMA.COLUMNS C
                            INNER JOIN INFORMATION_SCHEMA.TABLES T ON T.TABLE_SCHEMA = C.TABLE_SCHEMA AND T.TABLE_NAME = C.TABLE_NAME
                            WHERE T.TABLE_TYPE = 'VIEW'
                            ORDER BY C.TABLE_SCHEMA, C.TABLE_NAME, C.ORDINAL_POSITION;");

            // 6.Get view idnexes.
            sb.AppendLine(@"SELECT
                                OBJECT_SCHEMA_NAME(C.[object_id]) AS TABLE_SCHEMA,
                                OBJECT_NAME(C.[object_id]) AS TABLE_NAME,
                                I.name AS INDEX_NAME,
                                S.name AS COLUMN_NAME
                            FROM sys.index_columns C
                            LEFT OUTER JOIN sys.indexes I ON C.[object_id] = I.[object_id] AND C.index_id = I.index_id
                            LEFT OUTER JOIN sys.columns S ON C.[object_id] = S.[object_id] AND C.column_id = S.column_id
                            WHERE
                                OBJECTPROPERTY(C.[object_id], 'IsView') = 1 AND
                                OBJECT_NAME(C.[object_id]) <> 'sysdiagrams' AND
                                I.[type] in (1, 2) AND
                                I.is_primary_key = 0
                            ORDER BY
                                OBJECT_SCHEMA_NAME(C.[object_id]),
                                OBJECT_NAME(C.[object_id]),
                                C.index_id;");

            // 7.Get procedures.
            sb.AppendLine(@"SELECT
                                SPECIFIC_SCHEMA,
                                SPECIFIC_NAME,
                                ROUTINE_DEFINITION
                            FROM INFORMATION_SCHEMA.ROUTINES
                            WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME NOT IN ('sp_upgraddiagrams', 'sp_helpdiagrams', 'sp_helpdiagramdefinition', 'sp_alterdiagram', 'sp_creatediagram', 'sp_dropdiagram', 'sp_renamediagram')
                            ORDER BY SPECIFIC_SCHEMA, SPECIFIC_NAME;");

            // 8.Get parameters.
            sb.AppendLine(@"SELECT
                                SPECIFIC_SCHEMA,
                                SPECIFIC_NAME,
                                PARAMETER_NAME,
                                DATA_TYPE,
                                ISNULL(CHARACTER_MAXIMUM_LENGTH, 0) AS CHARACTER_MAXIMUM_LENGTH,
                                PARAMETER_MODE
                            FROM INFORMATION_SCHEMA.PARAMETERS
                            ORDER BY SPECIFIC_SCHEMA, SPECIFIC_NAME, ORDINAL_POSITION;");

            // 9.Get descriptions.
            sb.AppendLine(@"SELECT
                                OBJECTPROPERTYEX(major_id, 'BaseType') AS BaseType,
                                OBJECT_SCHEMA_NAME(major_id) AS [Schema],
                                OBJECT_NAME(major_id) AS MajorName,
                                COL_NAME(major_id, minor_id) AS MinorName,
                                [value] AS [Value]
                            FROM sys.extended_properties
                            WHERE [name] = 'MS_Description';");

            // 10.Get enumerations and enumeration members.
            sb.AppendLine(@"IF
                                EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Ref_Enumeration') AND
                                EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Ref_EnumerationMember')
                            BEGIN
                                SELECT
                                    [Name],
                                    BaseType,
                                    HasFlagsAttribute,
                                    Description
                                FROM Ref_Enumeration;
                                SELECT
                                    EnumerationName,
                                    [Name],
                                    [Value],
                                    Description
                                FROM Ref_EnumerationMember
                                ORDER BY EnumerationName, [Value];
                            END");

            return sb.ToString();
        }

        private void ExecuteSql(string commandText)
        {
            using (SqlConnection conn = new SqlConnection(this.m_connectionString))
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
            string cmdText = GetSchemaSqlText();

            SqlDataAdapter da = new SqlDataAdapter(cmdText, this.m_connectionString);

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
            ResourceFileManager manager = new ResourceFileManager();

            this.ExecuteSql(manager.ReadResourceString("Resources.CreateEnumerationTable.sql"));
            this.ExecuteSql(manager.ReadResourceString("Resources.CreateEnumerationMemberTable.sql"));
        }

        private void LoadDatabaseSchema(DataSet ds)
        {
            DataRowCollection drs;

            Table table;
            View view;
            StoredProcedure procedure;
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
                this.database.StoredProcedures.Add(
                    new StoredProcedure()
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
                if (this.database.StoredProcedures.Contains(dr["SPECIFIC_SCHEMA"].ToString(), dr["SPECIFIC_NAME"].ToString()))
                {
                    procedure = this.database.StoredProcedures.GetItem(dr["SPECIFIC_SCHEMA"].ToString(), dr["SPECIFIC_NAME"].ToString());

                    sqlDbType = MappingHelper.GetSqlDbType(dr["DATA_TYPE"].ToString());

                    procedure.Parameters.Add(
                        new Parameter()
                        {
                            Name = dr["PARAMETER_NAME"].ToString(),
                            SqlDbType = sqlDbType,
                            Size = Convert.ToInt32(dr["CHARACTER_MAXIMUM_LENGTH"].ToString(), CultureInfo.InvariantCulture),
                            Direction = MappingHelper.GetParameterDirection(dr["PARAMETER_MODE"].ToString())
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
                            this.database.StoredProcedures.GetItem(schema, majorName).Description = value;

                            break;
                        }
                }
            }

            #endregion

            #region [10] Enumeratioins

            drs = ds.Tables[10].Rows;

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