using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using DbSharper.Schema.Code;

namespace DbSharper.Schema
{
	internal static class MappingExtenderHelper
	{
		#region Methods

		internal static string GetPrimaryKeyNamesConnectedWithAnd(Model model)
		{
			const string connectorString ="And";

			StringBuilder sb = new StringBuilder();

			foreach (var property in model.Properties)
			{
				if (property.IsPrimaryKey)
				{
					sb.Append(property.ColumnName);
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
		/// <returns>Method paramter.</returns>
		internal static Parameter PropertyToParameter(Property property)
		{
			return new Parameter
			{
				Name = property.Name,
				Description = property.Description,
				Direction = ParameterDirection.Input,
				Size = property.Size,
				DbType = property.DbType,
				SqlName = "@" + property.Name,
				Type = property.Type
			};
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
				CommonType = property.Type.ToString()
			};
		}

		#endregion Methods
	}
}