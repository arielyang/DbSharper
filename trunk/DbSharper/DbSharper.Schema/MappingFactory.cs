using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

using DbSharper.Schema.Code;
using DbSharper.Schema.Collections;
using DbSharper.Schema.Configuration;
using DbSharper.Schema.Database;
using DbSharper.Schema.Enums;

namespace DbSharper.Schema
{
	public static class MappingFactory
	{
		#region Fields

		private static Database.Database database;
		private static Mapping mapping;
		private static MappingConfiguration mappingConfig;
		private static SchemaProvider provider;
		private static Regex regexGetList = new Regex(@"^Get\w*List", RegexOptions.Compiled | RegexOptions.Singleline);
		private static Regex regexResults = new Regex(@"--\s*<results>\s*(?:\r\n)+(?:--\s*(?<Type>[A-Z]\w+(?:Item|Collection))\s+(?<Name>[A-Z]\w+);\s*(?:\r\n)+)+--\s*</results>", RegexOptions.Compiled | RegexOptions.Multiline);

		#endregion Fields

		#region Methods

		public static Mapping CreateMapping(string mappingConfigFile, string mappingConfigContent)
		{
			mappingConfig = new MappingConfiguration(mappingConfigContent);

			string configFile = Path.Combine(Path.GetDirectoryName(mappingConfigFile), mappingConfig.ConfigFile);

			string connectionStringName = GetConnectionStringName(mappingConfigFile);
			string connectionStringValue = GetConnectionStringValue(configFile, connectionStringName);

			provider = new SchemaProvider();

			database = provider.GetSchema(connectionStringValue);

			mapping = GetMapping(database, mappingConfig, connectionStringName);

			////MappingExtender extender = new MappingExtender(mapping);

			////extender.Extend();

			return mapping;
		}

		private static Method BuildMethod(StoredProcedure procedure, string className, string methodName)
		{
			Method method = new Method()
			{
				Name = MappingHelper.GetPascalCase(methodName),
				CommandText = string.Format(CultureInfo.InvariantCulture, "[{0}].[{1}]", procedure.Schema, procedure.Name),
				CommandType = CommandType.StoredProcedure,
				Description = procedure.Description,
				MethodType = MappingHelper.GetMethodType(methodName)
			};

			LoadParameters(method.Parameters, procedure);
			LoadResults(method.Results, className, method, procedure);

			return method;
		}

		private static bool CanGetCollectionBy(Table table, string columnName)
		{
			foreach (ForeignKey fk in table.ForeignKeys)
			{
				if (fk.Columns.Count == 1 && fk.Columns[0].Name == columnName)
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

		private static string GetConnectionStringName(string mappingConfigFile)
		{
			return Path.GetFileNameWithoutExtension(mappingConfigFile);
		}

		private static string GetConnectionStringValue(string configFile, string connectionStringName)
		{
			if (string.IsNullOrEmpty(configFile))
			{
				throw new ArgumentException("\"configFile\" can not be null or empty.", "configFile");
			}

			XmlDocument doc = new XmlDocument();

			doc.Load(configFile);

			XmlNode node = doc.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, "/configuration/connectionStrings/add[@name='{0}']/@connectionString", connectionStringName));

			if (node == null)
			{
				throw new DbSharperException(string.Format(CultureInfo.InvariantCulture, "Can not find connection string named \"{0}\".", connectionStringName));
			}

			return node.Value;
		}

		private static Mapping GetMapping(Database.Database database, MappingConfiguration mappingConfig, string connectionStringName)
		{
			string schema;

			mapping = new Mapping();
			mapping.ConnectionStringName = connectionStringName;
			mapping.Database = database;

			foreach (var enumeration in database.Enumerations)
			{
				DbSharper.Schema.Code.Enum enm = new DbSharper.Schema.Code.Enum
					{
						Name = enumeration.Name,
						BaseType = enumeration.BaseType,
						HasFlagsAttribute = enumeration.HasFlagsAttribute,
						Description = enumeration.Description
					};

				LoadEnumMembers(enm.Members, enumeration);

				mapping.Enums.Add(enm);
			}

			foreach (Table table in database.Tables)
			{
				if (mappingConfig.Model.IsIncluded(table))
				{
					schema = MappingHelper.GetPascalCase(table.Schema);

					if (!mapping.ModelNamespaces.Contains(schema))
					{
						mapping.ModelNamespaces.Add(new ModelNamespace() { Name = schema });
					}

					Model model = new Model()
						{
							Name = MappingHelper.GetPascalCase(mappingConfig.Model.TrimName(table)),
							Description = table.Description,
							Schema = MappingHelper.GetPascalCase(table.Schema),
							IsView = false,
							MappingSource = table.Name
						};

					LoadProperties(model.Properties, table);

					mapping.ModelNamespaces[schema].Models.Add(model);
				}
			}

			foreach (View view in database.Views)
			{
				if (mappingConfig.Model.IsIncluded(view))
				{
					schema = MappingHelper.GetPascalCase(view.Schema);

					if (!mapping.ModelNamespaces.Contains(schema))
					{
						mapping.ModelNamespaces.Add(new ModelNamespace() { Name = schema });
					}

					Model model = new Model()
						{
							Name = MappingHelper.GetPascalCase(mappingConfig.Model.TrimName(view)),
							Description = view.Description,
							Schema = MappingHelper.GetPascalCase(view.Schema),
							IsView = true,
							MappingSource = view.Name
						};

					LoadProperties(model.Properties, view);

					mapping.ModelNamespaces[schema].Models.Add(model);
				}
			}

			foreach (StoredProcedure procedure in database.StoredProcedures)
			{
				if (mappingConfig.DataAccess.IsIncluded(procedure))
				{
					var classMethod = mappingConfig.DataAccess.GetClassMethod(procedure);

					if (classMethod == null)
					{
						continue;
					}

					schema = MappingHelper.GetPascalCase(procedure.Schema);

					if (!mapping.DataAccessNamespaces.Contains(schema))
					{
						mapping.DataAccessNamespaces.Add(new DataAccessNamespace() { Name = schema });
					}

					if (!mapping.DataAccessNamespaces[schema].DataAccesses.Contains(classMethod.ClassName))
					{
						mapping.DataAccessNamespaces[schema].DataAccesses.Add(
							new DataAccess()
							{
								Name = classMethod.ClassName,
								Description = string.Format(CultureInfo.InvariantCulture, "Access data about {0}.", classMethod.ClassName),
								Schema = MappingHelper.GetPascalCase(procedure.Schema)
							});
					}

					////if ((classMethod.MethodName == "Create" || classMethod.MethodName == "Update") && mapping.GetModel(classMethod.ClassName) != null)
					////{
					////    continue;
					////}

					mapping.DataAccessNamespaces[schema].DataAccesses[classMethod.ClassName].Methods.Add(
						BuildMethod(procedure, classMethod.ClassName, classMethod.MethodName));
				}
			}

			return mapping;
		}

		private static string GetModelForMethod(string schema, string modelName)
		{
			Model model = mapping.GetModel(schema, modelName);

			if (model == null)
			{
				model = mapping.GetModel(modelName);

				if (model != null)
				{
					return string.Format(
						CultureInfo.InvariantCulture,
						"Models.{0}.{1}",
						model.Schema,
						model.Name);
				}
			}

			return modelName;
		}

		private static string GetReferenceType(Table table, string columnName)
		{
			foreach (ForeignKey fk in table.ForeignKeys)
			{
				if (fk.Columns.Count == 1 && fk.Columns[0].Name == columnName)
				{
					Table tb = database.Tables[fk.ReferentialTable];

					return "Models." + MappingHelper.GetPascalCase(tb.Schema) + "." + MappingHelper.GetPascalCase(mappingConfig.Model.TrimName(tb)) + "Item";
				}
			}

			if (columnName.EndsWith("_Id", StringComparison.OrdinalIgnoreCase))
			{
				return columnName.Substring(0, columnName.Length - 3) + "Item";
			}

			if (database.Enumerations.Contains(columnName))
			{
				return "Enums." + columnName;
			}

			CommonType commonType = MappingHelper.GetCommonType(table.Columns[columnName].SqlDbType);

			return MappingHelper.GetCommonTypeString(commonType);
		}

		private static string GetReferenceType(View view, string columnName)
		{
			if (columnName.EndsWith("_Id", StringComparison.OrdinalIgnoreCase))
			{
				return columnName.Substring(0, columnName.Length - 3) + "Item";
			}

			CommonType commonType = MappingHelper.GetCommonType(view.Columns[columnName].SqlDbType);

			return MappingHelper.GetCommonTypeString(commonType);
		}

		private static void LoadEnumMembers(NamedCollection<EnumMember> members, Enumeration enumeration)
		{
			foreach (var enumerationMember in enumeration.Members)
			{
				members.Add(
					new EnumMember
					{
						Name = enumerationMember.Name,
						Value = enumerationMember.Value,
						Description = enumerationMember.Description
					});
			}
		}

		private static void LoadParameters(NamedCollection<Code.Parameter> parameters, StoredProcedure procedure)
		{
			string parameterName;

			foreach (var parameter in procedure.Parameters)
			{
				parameterName = MappingHelper.GetPascalCase(MappingHelper.GetParameterName(parameter.Name));

				parameters.Add(
					new Code.Parameter()
					{
						Name = parameterName,
						SqlName = parameter.Name,
						SqlDbType = parameter.SqlDbType,
						Description = parameter.Description,
						Direction = parameter.Direction,
						Size = parameter.Size,
						Type = MappingHelper.GetCommonType(parameter.SqlDbType)
					});
			}
		}

		private static void LoadProperties(NamedCollection<Property> properties, Table table)
		{
			foreach (Column column in table.Columns)
			{
				properties.Add(
					new Property
					{
						Name = MappingHelper.GetPascalCase(MappingHelper.TrimId(column.Name)),
						Column = column.Name,
						ColumnName = MappingHelper.GetPascalCase(column.Name),
						Description = column.Description,
						Nulls = column.Nullable,
						ReferenceType = GetReferenceType(table, column.Name),
						Size = column.Size,
						SqlDbType = column.SqlDbType,
						Type = MappingHelper.GetCommonType(column.SqlDbType),
						CanGetItemBy = CanGetItemBy(table, column.Name),
						CanGetCollectionBy = CanGetCollectionBy(table, column.Name),
						IsPrimaryKey = table.PrimaryKey.Columns.Contains(column.Name),
						HasDefault = !string.IsNullOrEmpty(column.Default.Trim()),
						Attributes = string.Empty
					});
			}
		}

		private static void LoadProperties(NamedCollection<Property> properties, View view)
		{
			foreach (Column column in view.Columns)
			{
				properties.Add(
					new Property
					{
						Name = MappingHelper.GetPascalCase(MappingHelper.TrimId(column.Name)),
						Column = column.Name,
						ColumnName = MappingHelper.GetPascalCase(column.Name),
						Description = column.Description,
						Nulls = column.Nullable,
						ReferenceType = GetReferenceType(view, column.Name),
						Size = column.Size,
						SqlDbType = column.SqlDbType,
						Type = MappingHelper.GetCommonType(column.SqlDbType),
						Attributes = string.Empty
					});
			}
		}

		private static void LoadResults(NamedCollection<Result> results, string modelName, Method method, StoredProcedure procedure)
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
						CommonType = modelNameWithSchema + typePostfix,
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
							Name = MappingHelper.GetPascalCase(matchResults.Groups["Name"].Captures[i].Value),
							CommonType = matchResults.Groups["Type"].Captures[i].Value,
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
							CommonType = parameter.Name == "ReturnResult" ? "ReturnResult" : MappingHelper.GetCommonTypeString(parameter.Type),
							Description = parameter.Description,
							IsOutputParameter = true
						});
				}
			}
		}

		#endregion Methods
	}
}