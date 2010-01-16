using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Text;

using DbSharper.Schema.Code;
using DbSharper.Schema.Infrastructure;
using DbSharper.Schema.Provider;

namespace DbSharper.Schema
{
	internal static class MappingExtenderHelper
	{
		#region Fields

		private const string returnResult = "ReturnResult";

		#endregion Fields

		#region Methods

		internal static Result BuildReturnResult(SchemaProviderBase provider)
		{
			return new Result
				{
					Name = returnResult,
					Type = "global::DbSharper.Library.Data.ReturnResult",
					Description = "Return result.",
					IsOutputParameter = true
				};
		}

		internal static Parameter BuildReturnResultParameter(SchemaProviderBase provider)
		{
			return new Parameter
				{
					Name = returnResult,
					CamelCaseName = returnResult.ToCamelCase(),
					SqlName = provider.BuildParameterSqlName(returnResult),
					Description = "Return result.",
					EnumType = "global::DbSharper.Library.Data.ReturnResult",
					DbType = DbType.Int32,
					Size = 0,
					Type = "global::System.Int32",
					Direction = ParameterDirection.ReturnValue,

				};
		}

		internal static string GetPrimaryKeyNamesConnectedWithAnd(Model model)
		{
			return GetPrimaryKeyNamesConnectedWithAnd(model, false);
		}

		internal static string GetPrimaryKeyNamesConnectedWithAnd(Model model, bool isList)
		{
			const string connectorString = "And";
			const string listString = "List";

			StringBuilder sb = new StringBuilder();

			foreach (var property in model.Properties)
			{
				if (property.IsPrimaryKey && !property.IsExtended)
				{
					sb.Append(property.Name);

					if (isList)
					{
						sb.Append(listString);
					}

					sb.Append(connectorString);
				}
			}

			if (sb.Length > 0)
			{
				sb.Length -= connectorString.Length;
			}

			return sb.ToString();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		internal static Collection<Property> GetSinglePrimaryKeyProperty(Model model)
		{
			Collection<Property> list = new Collection<Property>();

			foreach (var property in model.Properties)
			{
				if (property.IsPrimaryKey)
				{
					list.Add(property);
				}
			}

			return list;
		}

		internal static bool HasSinglePrimaryKeyProperty(Model model)
		{
			int count = 0;

			foreach (var property in model.Properties)
			{
				if (property.IsPrimaryKey)
				{
					count++;
				}
			}

			return count == 1;
		}

		/// <summary>
		/// Transform a model property to a method parameter.
		/// </summary>
		/// <param name="property">Model property.</param>
		/// <param name="isList"></param>
		/// <returns>Method paramter.</returns>
		internal static Parameter PropertyToParameter(SchemaProviderBase provider, Property property, bool isList)
		{
			try
			{
				string name = isList ? property.Name + "List" : property.Name;

				return new Parameter
				{
					Name = name,
					CamelCaseName = name.ToCamelCase(),
					Description = property.Description,
					Direction = ParameterDirection.Input,
					Size = property.Size,
					DbType = property.DbType,
					SqlName = provider.BuildParameterSqlName(property.Name),
					Type = isList ? string.Format(CultureInfo.InvariantCulture, "global::System.Collections.Generic.List<global::System.{0}>", property.Type) : property.Type
				};

			}
			catch (ArgumentException)
			{
				return null;
			}
		}

		internal static Parameter PropertyToParameter(SchemaProviderBase provider, Property property)
		{
			return PropertyToParameter(provider, property, false);
		}

		#endregion Methods

		#region Other

		///// <summary>
		///// Transform a primary key model property to an inserted result.
		///// </summary>
		///// <param name="property">Primary key model property.</param>
		///// <returns>Inserted result.</returns>
		//internal static Result PropertyToResult(Property property)
		//{
		//	return new Result
		//	{
		//		Name = "Inserted" + property.Name,
		//		Description = "Inserted " + property.Name,
		//		IsOutputParameter = true,
		//		Type = property.Type.ToString()
		//	};
		//}

		#endregion Other
	}
}