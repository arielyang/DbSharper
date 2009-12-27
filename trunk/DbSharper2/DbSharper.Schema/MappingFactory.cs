using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

using DbSharper.Schema.Code;
using DbSharper.Schema.Configuration;
using DbSharper.Schema.Database;
using DbSharper.Schema.Infrastructure;
using DbSharper.Schema.Provider;

namespace DbSharper.Schema
{
	public static class MappingFactory
	{
		#region Fields

		private static Database.Database database;
		private static Mapping mapping;
		private static MappingRuleManager mappingRuleManager;
		private static SchemaProviderBase provider;
		private static Regex regexGetList = new Regex(@"^Get\w*List", RegexOptions.Compiled | RegexOptions.Singleline);
		private static Regex regexResults = new Regex(@"--\s*<results>\s*(?:\r\n)+(?:--\s*(?<Type>[A-Z]\w+(?:Item|Collection))\s+(?<Name>[A-Z]\w+);\s*(?:\r\n)+)+--\s*</results>", RegexOptions.Compiled | RegexOptions.Multiline);

		#endregion Fields

		#region Methods

		///// <summary>
		///// Create a mapping.
		///// </summary>
		///// <param name="mappingConfigFile">Mapping configuration file.</param>
		///// <returns>Mapping.</returns>
		//public static Mapping CreateMapping(string mappingConfigFile)
		//{
		//    if (string.IsNullOrEmpty(mappingConfigFile))
		//    {
		//        throw new ArgumentNullException("mappingConfigFile");
		//    }

		//    if (!File.Exists(mappingConfigFile))
		//    {
		//        // TODO: Embed string into resource file later.
		//        throw new FileNotFoundException(
		//            string.Format(CultureInfo.InvariantCulture, "{0}is not found.", mappingConfigFile),
		//            mappingConfigFile);
		//    }

		//    string mappingConfigContent = File.ReadAllText(mappingConfigFile);

		//    return CreateMapping(mappingConfigFile, mappingConfigContent);
		//}

		/// <summary>
		/// Create a mapping.
		/// </summary>
		/// <param name="mappingConfigFile">Mapping configuration file.</param>
		/// <param name="mappingConfigContent">Mapping configuration file content.</param>
		/// <returns>Mapping.</returns>
		public static Mapping CreateMapping(string mappingConfigFile, string mappingConfigContent)
		{
			if (string.IsNullOrEmpty(mappingConfigFile))
			{
				throw new ArgumentNullException("mappingConfigFile");
			}

			if (string.IsNullOrEmpty(mappingConfigContent))
			{
				throw new ArgumentNullException("mappingConfigContent");
			}

			if (!File.Exists(mappingConfigFile))
			{
				// TODO: Embed string into resource file later.
				throw new FileNotFoundException(
					string.Format(CultureInfo.InvariantCulture, "{0} is not found.", mappingConfigFile),
					mappingConfigFile);
			}

			MappingConfiguration mappingConfig = new MappingConfiguration(mappingConfigContent);

			mappingRuleManager = new MappingRuleManager(mappingConfig);

			string configFile = Path.Combine(Path.GetDirectoryName(mappingConfigFile), mappingConfig.ConfigFile);

			string connectionStringName = MappingHelper.GetConnectionStringName(mappingConfigFile);
			string connectionStringValue = MappingHelper.GetConnectionStringValue(configFile, connectionStringName);
			string connestionStringProviderName = MappingHelper.GetConnectionStringProviderName(configFile, connectionStringName);

			provider = SchemaProviderFactory.Create(connestionStringProviderName);

			database = provider.GetSchema(connectionStringValue);

			mapping = GetMapping(database, connectionStringName);

			//MappingExtender extender = new MappingExtender(mapping);

			//extender.Extend();

			return mapping;
		}

		private static void AddModel(IColumns databaseObject)
		{
			string schema;

			if (mappingRuleManager.IsIncluded(databaseObject))
			{
				schema = databaseObject.Schema.ToPascalCase();

				if (!mapping.ModelNamespaces.Contains(schema))
				{
					mapping.ModelNamespaces.Add(new ModelNamespace() { Name = schema });
				}

				Model model = new Model()
				{
					Namespace = schema,
					Name = mappingRuleManager.TrimPrefix(databaseObject).ToPascalCase(),
					Description = databaseObject.Description,
					IsView = databaseObject is View,
					MappingSource = databaseObject.Name
				};

				LoadProperties(model.Properties, databaseObject);

				mapping.ModelNamespaces[schema].Models.Add(model);
			}
		}

		private static Method BuildMethod(Procedure procedure, string className, string methodName)
		{
			Method method = new Method()
			{
				Name = methodName.ToPascalCase(),
				CommandText = string.Format(CultureInfo.InvariantCulture, "[{0}].[{1}]", procedure.Schema, procedure.Name),
				CommandType = CommandType.StoredProcedure,
				Description = procedure.Description,
				MethodType = MappingHelper.GetMethodType(methodName)
			};

			LoadParameters(method.Parameters, procedure);
			LoadResults(method.Results, className, method, procedure);

			return method;
		}

		private static bool CanGetCollectionBy(IColumns databaseObject, string columnName)
		{
			if (databaseObject is Table)
			{
				Table table = databaseObject as Table;

				foreach (ForeignKey fk in table.ForeignKeys)
				{
					if (fk.Columns.Count == 1 && fk.Columns[0].Name == columnName)
					{
						return true;
					}
				}
			}

			foreach (Index idx in databaseObject.Indexes)
			{
				if (idx.Columns.Count == 1 && idx.Columns[0].Name == columnName)
				{
					return true;
				}
			}

			return false;
		}

		private static bool CanGetItemBy(Table table, string columnName)
		{
			if (table.PrimaryKey.Columns.Count == 1 && table.PrimaryKey.Columns[0].Name == columnName)
			{
				return true;
			}

			foreach (UniqueKey uk in table.UniqueKeys)
			{
				if (uk.Columns.Count == 1 && uk.Columns[0].Name == columnName)
				{
					return true;
				}
			}

			return false;
		}

		private static Mapping GetMapping(Database.Database database, string connectionStringName)
		{
			string schema;

			mapping = new Mapping();
			mapping.ConnectionStringName = connectionStringName;
			mapping.Database = database;

			foreach (Table table in database.Tables)
			{
				AddModel(table);
			}

			foreach (View view in database.Views)
			{
				AddModel(view);
			}

			foreach (Procedure procedure in database.Procedures)
			{
				if (mappingRuleManager.IsIncluded(procedure))
				{
					var classMethod = mappingRuleManager.GetClassMethod(procedure);

					if (classMethod == null)
					{
						continue;
					}

					schema = procedure.Schema.ToPascalCase();

					if (!mapping.DataAccessNamespaces.Contains(schema))
					{
						mapping.DataAccessNamespaces.Add(new DataAccessNamespace() { Name = schema });
					}

					DataAccessNamespace nameSpace = mapping.DataAccessNamespaces[schema];

					if (!nameSpace.DataAccesses.Contains(classMethod.ClassName))
					{
						nameSpace.DataAccesses.Add(
							new DataAccess()
							{
								Namespace = schema,
								Name = classMethod.ClassName,
								Description = string.Format(CultureInfo.InvariantCulture, "Access data about {0}.", classMethod.ClassName)
							});
					}

					Method method = BuildMethod(procedure, classMethod.ClassName, classMethod.MethodName);

					nameSpace.DataAccesses[classMethod.ClassName].Methods.Add(method);
				}
			}

			return mapping;
		}

		private static string GetModelForMethod(string schema, string modelName)
		{
			Model model = mapping.GetModel(schema, modelName);

			if (model == null)
			{
				if (mapping.ContainsModel(modelName))
				{
					return string.Format(
						CultureInfo.InvariantCulture,
						"Models.{0}.{1}",
						schema,
						modelName);
				}
			}

			return modelName;
		}

		private static string GetName(IColumns databaseObject, string columnName)
		{
			if (databaseObject is Table)
			{
				var foreignKeys = (databaseObject as Table).ForeignKeys;

				foreach (ForeignKey fk in foreignKeys)
				{
					if (fk.Columns.Count == 1 && fk.Columns[0].Name == columnName)
					{
						return columnName.TrimId().ToPascalCase();
					}
				}
			}

			if (columnName.EndsWith("_Id", StringComparison.OrdinalIgnoreCase))
			{
				string referenceName = columnName.TrimId();

				foreach (Table tb in database.Tables)
				{
					if (mappingRuleManager.TrimPrefix(tb) == referenceName)
					{
						return referenceName.ToPascalCase();
					}
				}
			}

			return columnName.ToPascalCase();
		}

		private static string GetReferenceType(IColumns databaseObject, string columnName)
		{
			if (databaseObject is Table)
			{
				var foreignKeys = (databaseObject as Table).ForeignKeys;

				foreach (ForeignKey fk in foreignKeys)
				{
					if (fk.Columns.Count == 1 && fk.Columns[0].Name == columnName)
					{
						Table tb = database.Tables[fk.ReferentialTable];

						return "Models." + tb.Schema.ToPascalCase() + "." + mappingRuleManager.TrimPrefix(tb).ToPascalCase() + "Model";
					}
				}
			}

			if (columnName.EndsWith("_Id", StringComparison.OrdinalIgnoreCase))
			{
				string referenceName = columnName.TrimId();

				foreach (Table tb in database.Tables)
				{
					if (mappingRuleManager.TrimPrefix(tb) == referenceName)
					{
						return "Models." + tb.Schema.ToPascalCase() + "." + referenceName.ToPascalCase() + "Model";
					}
				}
			}

			CommonType commonType = (databaseObject as IColumns).Columns[columnName].DbType.ToCommonType();

			return MappingHelper.GetCommonTypeString(commonType);
		}

		private static string GetEnumType(DbType dbType, string columnName)
		{
			if (mapping.Database.Enumerations.Contains(columnName))
			{
				return "Enums." + columnName;
			}

			return null;
		}

		private static void LoadParameters(NamedCollection<Code.Parameter> parameters, Procedure procedure)
		{
			string parameterName;

			foreach (var parameter in procedure.Parameters)
			{
				parameterName = provider.GetParameterName(parameter.Name);

				parameters.Add(
					new Code.Parameter()
					{
						Name = parameterName.ToPascalCase(),
						CamelCaseName = parameterName.ToCamelCase(),
						SqlName = parameter.Name,
						DbType = parameter.DbType,
						Description = parameter.Description,
						Direction = parameter.Direction,
						Size = parameter.Size,
						Type = parameter.DbType.ToCommonType()
					});
			}
		}

		private static void LoadProperties(NamedCollection<Property> properties, IColumns databaseObject)
		{
			Table table;

			bool isView = databaseObject is View;

			if (isView)
			{
				table = null;
			}
			else
			{
				table = databaseObject as Table;
			}

			string name;
			string pascalName;
			Property property;

			foreach (Column column in databaseObject.Columns)
			{
				name = GetName(databaseObject, column.Name);
				pascalName = column.Name.ToPascalCase();

				property = new Property
					{
						Name = pascalName,
						Column = column.Name,
						CamelCaseName = pascalName.ToCamelCase(),
						Description = column.Description,
						Nulls = column.Nullable,
						Size = column.Size,
						DbType = column.DbType,
						Type = MappingHelper.GetCommonTypeString(column.DbType.ToCommonType()),
						EnumType = GetEnumType(column.DbType, column.Name),
						CanGetItemBy = isView ? false : CanGetItemBy(table, column.Name),
						CanGetCollectionBy = isView ? false : CanGetCollectionBy(databaseObject, column.Name),
						IsPrimaryKey = isView ? false : table.PrimaryKey.Columns.Contains(column.Name),
						HasDefault = isView ? false : !string.IsNullOrEmpty(column.Default.Trim()),
						IsExtended = false
					};

				properties.Add(property);

				if (name != pascalName && !databaseObject.Columns.Contains(name))
				{
					properties.Add(
						new Property
						{
							Name = name,
							Column = column.Name,
							CamelCaseName = name.ToCamelCase(),
							Description = column.Description,
							Nulls = column.Nullable,
							Size = column.Size,
							DbType = column.DbType,
							Type = GetReferenceType(databaseObject, column.Name),
							EnumType = null,
							CanGetItemBy = property.CanGetItemBy,
							CanGetCollectionBy = property.CanGetCollectionBy,
							IsPrimaryKey = property.IsPrimaryKey,
							HasDefault = property.HasDefault,
							IsExtended = true
						});
				}
			}
		}

		private static void LoadResults(NamedCollection<Result> results, string modelName, Method method, Procedure procedure)
		{
			Match matchResults = regexResults.Match(procedure.Definition);

			int length = matchResults.Groups["Name"].Captures.Count;

			if (length == 0 && method.MethodType == MethodType.ExecuteReader)
			{
				string typePostfix = regexGetList.IsMatch(method.Name) ? "Collection" : "Item";

				string modelNameWithSchema = GetModelForMethod(procedure.Schema, modelName);
				// TODO: Conflict Names.
				results.Add(
					new Result
					{
						Name = modelName + typePostfix,
						TypeName = modelNameWithSchema + typePostfix,
						Description = string.Empty,
						IsOutputParameter = false
					});
			}
			else
			{
				for (int i = 0; i < length; i++)
				{
					results.Add(
						new Result
						{
							Name = matchResults.Groups["Name"].Captures[i].Value.ToPascalCase(),
							TypeName = matchResults.Groups["Type"].Captures[i].Value,
							Description = string.Empty,
							IsOutputParameter = false
						});
				}
			}

			foreach (var parameter in method.Parameters)
			{
				if (parameter.Direction != ParameterDirection.Input)
				{
					results.Add(
						new Result
						{
							Name = parameter.Name,
							TypeName = parameter.Name == "ReturnResult" ? "ReturnResult" : MappingHelper.GetCommonTypeString(parameter.Type),
							Description = parameter.Description,
							IsOutputParameter = true
						});
				}
			}
		}

		#endregion Methods
	}
}