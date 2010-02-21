using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

using DbSharper2.Schema.Code;
using DbSharper2.Schema.Configuration;
using DbSharper2.Schema.Database;
using DbSharper2.Schema.Infrastructure;
using DbSharper2.Schema.Provider;

namespace DbSharper2.Schema
{
	public static class MappingFactory
	{
		#region Fields

		private static Mapping mapping;
		private static MappingRuleManager mappingRuleManager;
		private static SchemaProviderBase provider;
		private static Regex regexGetListMethod = new Regex(@"^Get\w*List", RegexOptions.Compiled | RegexOptions.Singleline);
		private static Regex regexListType = new Regex("List<(?<Type>.+)>$", RegexOptions.Compiled | RegexOptions.Singleline);
		private static Regex regexMethodResults = new Regex(@"--\s*<results>\s*(?:\r\n)+(?:--\s*(?<Type>[A-Z]\w+.[A-Z]\w+Model|List<[A-Z]\w+.[A-Z]\w+Model>)\s*(?<Name>[A-Z]\w+);\s*(?:\r\n)+)+--\s*</results>", RegexOptions.Compiled | RegexOptions.Multiline);

		#endregion Fields

		#region Methods

		/// <summary>
		/// Create a mapping.
		/// </summary>
		/// <param name="mappingConfigFile">Mapping configuration file.</param>
		/// <param name="mappingConfigContent">Mapping configuration file content.</param>
		/// <returns>Mapping.</returns>
		public static Mapping CreateMapping(string mappingConfigFile, string mappingConfigContent, NamedCollection<Enumeration> enumerations)
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

			ConnectionStringSettings settings = MappingHelper.GetConnectionStringSettings(configFile, connectionStringName);

			if (string.IsNullOrEmpty(settings.ConnectionString))
			{
				// TODO: Embed string into resource file later.
				throw new DbSharperException(string.Format(CultureInfo.InvariantCulture, "Value of connection string named \"{0}\" is null or empty.", connectionStringName));
			}

			if (string.IsNullOrEmpty(settings.ProviderName))
			{
				// TODO: Embed string into resource file later.
				throw new DbSharperException(string.Format(CultureInfo.InvariantCulture, "ProviderName of connection string named \"{0}\" is null or empty.", connectionStringName));
			}

			provider = SchemaProviderFactory.Create(settings.ProviderName);

			Database.Database database = provider.GetSchema(settings.ConnectionString);

			mapping = GetMapping(database, connectionStringName, provider.GetDatabaseType().FullName, enumerations);

			MappingExtender extender = new MappingExtender(mapping, provider);

			//extender.Extend();

			return mapping;
		}

		private static void AddDataAccess(Procedure procedure)
		{
			string schema;

			if (mappingRuleManager.IsIncluded(procedure))
			{
				var classMethod = mappingRuleManager.GetClassMethod(procedure);

				if (classMethod == null)
				{
					return;
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

				AddMethod(nameSpace, classMethod, procedure);
			}
		}

		private static void AddMethod(DataAccessNamespace nameSpace, ClassMethodContainer classMethod, Procedure procedure)
		{
			// No matched return result.
			if (classMethod.MethodName.StartsWith("Get", StringComparison.OrdinalIgnoreCase)
				&& !mapping.ContainsModel(classMethod.ClassName))
			{
				return;
			}

			Method method = new Method()
			{
				Name = classMethod.MethodName.ToPascalCase(),
				CommandText = string.Format(CultureInfo.InvariantCulture, "[{0}].[{1}]", procedure.Schema, procedure.Name),
				CommandType = CommandType.StoredProcedure,
				Description = procedure.Description,
				MethodType = MappingHelper.GetMethodType(classMethod.MethodName)
			};

			LoadParameters(method.Parameters, procedure);

			LoadResults(method, classMethod.ClassName, procedure);

			nameSpace.DataAccesses[classMethod.ClassName].Methods.Add(method);
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
					Name = mappingRuleManager.TrimPrefix(databaseObject).ToPascalCase().ToSingular(),
					Description = databaseObject.Description,
					IsView = databaseObject is View,
					MappingName = databaseObject.ToString()
				};

				LoadProperties(model.Properties, databaseObject);

				mapping.ModelNamespaces[schema].Models.Add(model);
			}
		}

		private static string DiscoverEnumType(CommonType commonType, string name)
		{
			if (mapping.Enumerations.Contains(name))
			{
				Enumeration enumeration = mapping.Enumerations[name];

				if (enumeration.CommonType == commonType)
				{
					return "global::" + enumeration.FullName;
				}
			}

			return null;
		}

		private static string DiscoverModelTypeForResult(string schema, string modelName)
		{
			Model model = mapping.GetModel(schema, modelName);

			if (model == null)
			{
				model = mapping.GetModel(modelName);
			}

			return string.Format(
				CultureInfo.InvariantCulture,
				"Models.{0}.{1}",
				model.Namespace,
				model.Name);
		}

		private static void DiscoverReferenceModelType(Model model, Property property, ref int index)
		{
			var tables = mapping.Database.Tables;

			string columnName = property.ColumnName;
			int originalIndex = index;

			Table referenceTable;
			Model referenceModel;
			string tableName;
			string primaryKeyColumnName;
			string propertyName;

			if (!model.IsView) // Is mapping from table.
			{
				Table table = mapping.Database.Tables[model.MappingName];

				if (table == null)
				{
					return;
				}

				var foreignKeys = table.ForeignKeys;

				foreach (var fk in foreignKeys)
				{
					if (fk.Columns.Count == 1 && fk.Columns[0].Name == columnName)
					{
						tableName = fk.ReferentialTableName;

						referenceTable = tables[tableName];
						referenceModel = mapping.GetModelByMappingTableName(tableName);

						if (referenceModel == null)
						{
							continue;
						}

						primaryKeyColumnName = referenceTable.PrimaryKey.Columns[0].Name.ToPascalCase();

						propertyName = columnName.TrimPrimaryKeyName(primaryKeyColumnName);

						if (string.IsNullOrEmpty(propertyName))
						{
							propertyName = referenceModel.Name;
						}
						else
						{
							propertyName = propertyName.ToPascalCase();
						}

						if (!model.Properties.Contains(propertyName))
						{
							model.Properties.Insert(
								index + 1,
								new Property
								{
									Name = propertyName,
									CamelCaseName = propertyName.ToCamelCase(),
									ColumnName = property.ColumnName,
									DbType = property.DbType,
									Type = string.Format(CultureInfo.InvariantCulture, "Models.{0}.{1}Model", referenceModel.Namespace, referenceModel.Name),
									EnumType = null,
									Nulls = false,
									Size = property.Size,
									Description = property.Description,
									HasDefault = property.HasDefault,
									IsPrimaryKey = property.IsPrimaryKey,
									RefPkName = primaryKeyColumnName,
									IsExtended = true
								});

							index++;
						}
					}
				}
			}

			if (index > originalIndex) // Found foreign key reference.
			{
				return;
			}

			// Not found foreign key reference.
			if (TrySplitColumnName(model.MappingName, property, out tableName, out primaryKeyColumnName))
			{
				referenceModel = mapping.GetModelByMappingTableName(tableName);

				if (referenceModel != null)
				{
					propertyName = columnName.TrimPrimaryKeyName(primaryKeyColumnName);

					if (string.IsNullOrEmpty(propertyName))
					{
						propertyName = referenceModel.Name;
					}
					else
					{
						propertyName = propertyName.ToPascalCase();
					}

					if (!model.Properties.Contains(propertyName))
					{
						model.Properties.Insert(
							index + 1,
							new Property
							{
								Name = propertyName,
								CamelCaseName = propertyName.ToCamelCase(),
								ColumnName = property.ColumnName,
								DbType = property.DbType,
								Type = string.Format(CultureInfo.InvariantCulture, "Models.{0}.{1}Model", referenceModel.Namespace.ToPascalCase(), referenceModel.Name),
								EnumType = null,
								Nulls = false,
								Size = property.Size,
								Description = property.Description,
								HasDefault = property.HasDefault,
								IsPrimaryKey = property.IsPrimaryKey,
								RefPkName = primaryKeyColumnName,
								IsExtended = true
							});

						index++;
					}
				}
			}
		}

		private static void DiscoverReferences()
		{
			NamedCollection<ModelNamespace> modelNamespaces = mapping.ModelNamespaces;
			NamedCollection<Model> models;
			NamedCollection<Property> properties;

			foreach (var modelNamespace in modelNamespaces)
			{
				models = modelNamespace.Models;

				foreach (var model in models)
				{
					properties = model.Properties;

					for (int i = 0; i < properties.Count; i++)
					{
						DiscoverReferenceModelType(model, properties[i], ref i);
					}
				}
			}
		}

		private static Mapping GetMapping(Database.Database database, string connectionStringName, string databaseType, NamedCollection<Enumeration> enumerations)
		{
			mapping = new Mapping();
			mapping.ConnectionStringName = connectionStringName;
			mapping.CamelCaseConnectionStringName = connectionStringName.ToCamelCase();
			mapping.DatabaseType = databaseType;
			mapping.Database = database;
			mapping.Enumerations = enumerations;

			foreach (var table in database.Tables)
			{
				AddModel(table);
			}

			foreach (var view in database.Views)
			{
				AddModel(view);
			}

			foreach (var procedure in database.Procedures)
			{
				AddDataAccess(procedure);
			}

			if (mappingRuleManager.IsAutoDiscoverReference())
			{
				DiscoverReferences();
			}

			return mapping;
		}

		private static void LoadParameters(NamedCollection<Code.Parameter> parameters, Procedure procedure)
		{
			string parameterName;

			var procedureParameters = procedure.Parameters;

			foreach (var parameter in procedureParameters)
			{
				parameterName = provider.GetParameterName(parameter.Name);

				parameters.Add(
					new Code.Parameter()
					{
						Name = parameterName.ToPascalCase(),
						CamelCaseName = parameterName.ToCamelCase(),
						SqlName = parameter.Name,
						DbType = parameter.DbType,
						Type = MappingHelper.GetCommonTypeString(parameter.DbType.ToCommonType()),
						EnumType = DiscoverEnumType(parameter.DbType.ToCommonType(), parameterName),
						Direction = parameter.Direction,
						Size = parameter.Size,
						Description = parameter.Description
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

			var columns = databaseObject.Columns;

			string name;
			CommonType commonType;

			foreach (var column in columns)
			{
				name = column.Name.ToPascalCase();
				commonType = column.DbType.ToCommonType();

				properties.Add(
					new Property
					{
						Name = name,
						CamelCaseName = column.Name.ToCamelCase(),
						ColumnName = column.Name,
						DbType = column.DbType,
						Type = MappingHelper.GetCommonTypeString(commonType),
						EnumType = DiscoverEnumType(commonType, name),
						Nulls = (commonType == CommonType.String || commonType == CommonType.Object) ? false : column.Nullable,
						Size = column.Size,
						Description = column.Description,
						HasDefault = isView ? false : !string.IsNullOrEmpty(column.Default.Trim()),
						IsPrimaryKey = isView ? false : table.PrimaryKey.Columns.Contains(column.Name),
						IsExtended = false
					});
			}
		}

		private static void LoadResults(Method method, string modelName, Procedure procedure)
		{
			var results = method.Results;

			if (method.MethodType == MethodType.ExecuteReader)
			{
				Match matchResults = null;

				int length;

				if (string.IsNullOrEmpty(procedure.Definition))
				{
					length = 0;
				}
				else
				{
					matchResults = regexMethodResults.Match(procedure.Definition);

					length = matchResults.Groups["Name"].Captures.Count;
				}

				if (length != 0) // As definition in procedure.
				{
					string type;

					Match matchListType;

					for (int i = 0; i < length; i++)
					{
						type = matchResults.Groups["Type"].Captures[i].Value;

						matchListType = regexListType.Match(type);

						if (matchListType.Success)
						{
							type = matchListType.Groups["Type"].Value;

							if (!type.StartsWith("Models.", StringComparison.OrdinalIgnoreCase))
							{
								type = "Models." + type;
							}

							type = string.Format(CultureInfo.InvariantCulture, "global::System.Collections.Generic.List<{0}>", type);
						}
						else
						{
							if (!type.StartsWith("Models.", StringComparison.OrdinalIgnoreCase))
							{
								type = "Models." + type;
							}
						}

						results.Add(
							new Result
							{
								Name = matchResults.Groups["Name"].Captures[i].Value.ToPascalCase(),
								Type = type,
								Description = string.Empty,
								IsOutputParameter = false
							});
					}
				}
				else // As procedure name.
				{
					bool isGetListMethod = regexGetListMethod.IsMatch(method.Name);

					// Get a model name like "Models.Site.User".
					string modelNameWithSchema = DiscoverModelTypeForResult(procedure.Schema, modelName);

					results.Add(
						new Result
						{
							Name = modelName + (isGetListMethod ? "List" : string.Empty),
							Type = isGetListMethod ? string.Format(CultureInfo.InvariantCulture, "global::System.Collections.Generic.List<{0}Model>", modelNameWithSchema) : modelNameWithSchema + "Model",
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
							Type = parameter.EnumType ?? parameter.Type,
							Description = parameter.Description,
							IsOutputParameter = true
						});
				}
			}
		}

		private static bool TrySplitColumnName(string mappingName, Property property, out string tableName, out string primaryKeyColumnName)
		{
			var tables = mapping.Database.Tables;

			Column column;
			string trimmedTableName;

			foreach (var table in tables)
			{
				if (table.PrimaryKey.Columns.Count == 1)
				{
					column = table.PrimaryKey.Columns[0];

					trimmedTableName = mappingRuleManager.TrimPrefix(table);
					primaryKeyColumnName = column.Name.ToPascalCase();

					if (column.DbType == property.DbType)
					{
						if ((
							string.Compare(primaryKeyColumnName, property.Name, StringComparison.OrdinalIgnoreCase) == 0
							&& primaryKeyColumnName.StartsWith(table.Name)
							&& table.ToString() != mappingName
							)
							|| string.Compare(trimmedTableName + primaryKeyColumnName, property.Name, StringComparison.OrdinalIgnoreCase) == 0
							|| string.Compare(trimmedTableName + "_" + primaryKeyColumnName, property.Name, StringComparison.OrdinalIgnoreCase) == 0
							|| string.Compare(trimmedTableName.ToSingular() + primaryKeyColumnName, property.Name, StringComparison.OrdinalIgnoreCase) == 0
							|| string.Compare(trimmedTableName.ToSingular() + "_" + primaryKeyColumnName, property.Name, StringComparison.OrdinalIgnoreCase) == 0)
						{
							tableName = table.ToString(); // Table name with schema.

							return true;
						}
					}
				}
			}

			tableName = null;
			primaryKeyColumnName = null;

			return false;
		}

		#endregion Methods
	}
}