using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Text;

using DbSharper.Schema.Code;
using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema
{
	internal static class MappingExtenderHelper
	{
		#region Methods

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
				if (property.IsPrimaryKey)
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
		internal static Parameter PropertyToParameter(Property property, bool isList)
		{
			try
			{
				return new Parameter
				{
					Name = property.Name,
					Description = property.Description,
					Direction = ParameterDirection.Input,
					Size = property.Size,
					DbType = property.DbType,
					SqlName = "@" + property.Name,
					Type = isList ? string.Format(CultureInfo.InvariantCulture, "IList<{0}>", property.Type) : property.Type
				};

			}
			catch (ArgumentException)
			{
				return null;
			}
		}

		internal static Parameter PropertyToParameter(Property property)
		{
			return PropertyToParameter(property, false);
		}

		/// <summary>
		/// Transform a primary key model property to an inserted result.
		/// </summary>
		/// <param name="property">Primary key model property.</param>
		/// <returns>Inserted result.</returns>
		internal static Result PropertyToResult(Property property)
		{
			return new Result
			{
				Name = "Inserted" + property.Name,
				Description = "Inserted " + property.Name,
				IsOutputParameter = true,
				Type = property.Type.ToString()
			};
		}

		#endregion Methods
	}
}