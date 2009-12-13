using System;
using System.Data;
using System.Globalization;
using System.Text;
using DbSharper.Schema.Code;
using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema
{
	internal class MappingExtender
	{
		#region Fields

		private Mapping mapping;

		#endregion Fields

		#region Constructors

		internal MappingExtender(Mapping mapping)
		{
			this.mapping = mapping;
		}

		#endregion Constructors

		#region Delegates

		private delegate Method ExtendDelegate(Model model);

		#endregion Delegates

		#region Methods

		internal void Extend()
		{
			// Extend all namespaces.
			foreach (var ns in mapping.ModelNamespaces)
			{
				// Extend for all models.
				foreach (var model in ns.Models)
				{
					ExtendMethod(model, ExtendGetItemByPrimaryKeyMethod); // Get by id.
					ExtendMethod(model, ExtendGetItemByPrimaryKeyMethod); // Get by id with specific columns.
					ExtendMethod(model, ExtendGetItemByPrimaryKeyMethod); // Get list.
					ExtendMethod(model, ExtendGetItemByPrimaryKeyMethod); // Get list with specific columns.
					ExtendMethod(model, ExtendGetItemByPrimaryKeyMethod); // Get list by ids.
					ExtendMethod(model, ExtendGetItemByPrimaryKeyMethod); // Get list by ids with specific columns.
					ExtendMethod(model, ExtendGetItemByPrimaryKeyMethod); // Get list foreign keys or indexes.
					ExtendMethod(model, ExtendGetItemByPrimaryKeyMethod); // Get list foreign keys or indexes with specific columns.
					ExtendMethod(model, ExtendDeleteByPrimaryKeyMethod); // Delete by primary key(s).
					ExtendMethod(model, ExtendDeleteByPrimaryKeyMethod); // Delete by ids.
					ExtendMethod(model, ExtendDeleteByPrimaryKeyMethod); // Delete by foreign keys or indexes.
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private Method ExtendDeleteByPrimaryKeyMethod(Model model)
		{
			Method method = new Method
			{
				Name = "Delete",
				Description = string.Format(CultureInfo.InvariantCulture, "Delete {0} item by primary key. (Auto generated)", model.Name),
				CommandType = CommandType.Text,
				MethodType = MethodType.ExecuteNonQuery,
			};

			// Add parameters.
			foreach (var property in model.Properties)
			{
				if (property.IsPrimaryKey)
				{
					method.Parameters.Add(MappingExtenderHelper.PropertyToParameter(property));
				}
			}

			// Add result.
			method.Results.Add(
				new Result
				{
					Name = "ReturnResult",
					CommonType = "ReturnResult",
					Description = "Return result.",
					IsOutputParameter = true
				});

			return method;
		}

		private Method ExtendGetItemByPrimaryKeyMethod(Model model)
		{
			if (model.IsView)
			{
				return null;
			}

			Method method = new Method
			{
				Name = "GetBy" + MappingExtenderHelper.GetPrimaryKeyNamesConnectedWithAnd(model),
				Description = string.Format(CultureInfo.InvariantCulture, "Get {0} item by primary key. (Auto generated)", model.Name),
				CommandType = CommandType.Text,
				MethodType = MethodType.ExecuteReader
			};

			// Add parameters.
			foreach (var property in model.Properties)
			{
				if (property.IsPrimaryKey)
				{
					method.Parameters.Add(MappingExtenderHelper.PropertyToParameter(property));
				}
			}

			// Add result.
			method.Results.Add(
				new Result
				{
					Name = model.Name + "Item",
					CommonType = model.Name + "Item",
					Description = model.Name + " Item.",
					IsOutputParameter = false
				});

			StringBuilder sb = new StringBuilder();
			sb.AppendLine("SELECT");
			sb.AppendLine("FROM");
			foreach (var property in model.Properties)
			{
				if (property.ReferenceType != property.Type.ToString())
				{
				}
			}
			sb.AppendLine();
			sb.AppendLine("WHERE");
			foreach (var property in model.Properties)
			{
				if (property.IsPrimaryKey)
				{
					sb.AppendFormat("\t{0}.{1} = @{1} AND", model.MappingSource, property.Name);
					sb.AppendLine(); // Environment.NewLine
				}
			}
			sb.Length = sb.Length - " AND".Length - Environment.NewLine.Length;
			sb.AppendLine("\t{0} = @{0}");
			return method;
		}

		/// <summary>
		/// Extend a method for specific model.
		/// </summary>
		/// <param name="model">Model to extend.</param>
		/// <param name="extendMethod">Extend method delegate.</param>
		private void ExtendMethod(Model model, ExtendDelegate extendMethod)
		{
			Method method = extendMethod.Invoke(model);

			if (method == null)
			{
				return;
			}

			DataAccessNamespace dataAccessNamespace;
			DataAccess dataAccess;

			// Discover relative DataAccessNamespace or create a new one if it does not exist.
			if (mapping.DataAccessNamespaces.Contains(model.Schema))
			{
				dataAccessNamespace = mapping.DataAccessNamespaces[model.Schema];
			}
			else
			{
				dataAccessNamespace = new DataAccessNamespace { Name = model.Schema };

				mapping.DataAccessNamespaces.Add(dataAccessNamespace);
			}

			// Discover relative DataAccess or create a new one if it does not exist.
			if (dataAccessNamespace.DataAccesses.Contains(model.Name))
			{
				dataAccess = dataAccessNamespace.DataAccesses[model.Name];
			}
			else
			{
				dataAccess = new DataAccess
				{
					Name = model.Name,
					Schema = model.Schema,
					Description = string.Format(CultureInfo.InvariantCulture, "Auto generated class for model {0}.", model.Name)
				};

				dataAccessNamespace.DataAccesses.Add(dataAccess);
			}

			// Create an auto generated method for model if it does not exist.
			if (!dataAccess.Methods.Contains(method.Name))
			{
				dataAccess.Methods.Add(method);
			}
		}

		#endregion Methods

		#region Other

		/*

		private Method ExtendUpdateMethod(Model model)
		{
			if (model.IsView)
			{
				return null;
			}
			NamedCollection<Parameter> parameters = new NamedCollection<Parameter>();
			foreach (var property in model.Properties)
			{
				if (!property.HasDefault)
				{
					parameters.Add(PropertyToParameter(property));
				}
			}
			Method method = new Method
			{
				Name = "Update",
				Description = string.Format("Update a {0}.", model.Name),
				CommandType = CommandType.Text,
				MethodType = MethodType.ExecuteNonQuery,
				CommandText = "",
				Parameters = parameters
			};
			method.Results.Add(
				new Result
				{
					Name = "ReturnResult",
					Description = "ReturnResult",
					IsOutputParameter = false,
					CommonType = "ReturnResult"
				});
			return method;
		}

		private Method ExtendCreateMethod(Model model)
		{
			if (model.IsView)
			{
				return null;
			}
			NamedCollection<Parameter> parameters = new NamedCollection<Parameter>();
			foreach (var property in model.Properties)
			{
				parameters.Add(PropertyToParameter(property));
			}
			Method method = new Method
				{
					Name = "Create",
					Description = string.Format("Create a new {0}.", model.Name),
					CommandType = CommandType.Text,
					MethodType = MethodType.ExecuteNonQuery,
					CommandText = "",
					Parameters = parameters
				};
			if (HasSinglePrimaryKeyProperty(model)) // Only single primary key needs a returned inserted primary key.
			{
				Property property = GetSinglePriamryKeyProperty(model);
				Parameter parameter = PropertyToParameter(property);
				parameter.Name = "Inserted" + property.Name;
				parameter.SqlName = "@Inserted" + property.Name;
				parameter.Direction = ParameterDirection.InputOutput;
				method.Parameters.Add(parameter);
				method.Results.Add(PropertyToResult(property));
			}
			return method;
		}

		*/

		#endregion Other
	}
}