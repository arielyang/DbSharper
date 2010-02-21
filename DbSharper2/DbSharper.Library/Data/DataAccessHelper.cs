using System;
using System.Data.Common;
using System.Text;

using DbSharper2.Library.Schema;

namespace DbSharper2.Library.Data
{
	public static class DataAccessHelper
	{
		#region Methods

		public static void Update<TModel>(Database db, TModel model)
			where TModel : ModelBase
		{
			if (model.ChangedProperties == null)
			{
				return;
			}

			string tableName = DatabaseSchema.GetTableName(typeof(TModel));

			if (tableName == null)
			{
				return;
			}

			var columns = DatabaseSchema.GetColumns(typeof(TModel));

			DbCommand dbCommand = db.GetSqlStringCommand(" ");

			StringBuilder sb = new StringBuilder();

			sb.AppendLine("UPDATE");
			sb.Append('\t');
			sb.AppendLine(tableName);
			sb.Append("SET");

			ColumnAttribute column;

			foreach (string propertyName in model.ChangedProperties)
			{
				column = columns[propertyName];

				db.AddInParameter(
					dbCommand,
					db.BuildParameterName(column.Name),
					column.DbType,
					model.GetPropertyValue(propertyName)
					);

				if (column.IsPrimaryKey)
				{
					sb.AppendLine();
					sb.Append('\t');
					sb.Append(column.Name);
					sb.Append(" = ");
					sb.Append(db.BuildParameterName(column.Name));
					sb.Append(",");
				}
			}

			sb.Length -= 1;

			sb.AppendLine();
			sb.Append("WHERE");

			foreach (var kvp in columns)
			{
				column = kvp.Value;

				if (!column.IsPrimaryKey)
				{
					if (!model.ChangedProperties.Contains(column.Name))
					{
						// TODO: Embed string into resource file later.
						throw new ArgumentException("Primary key column is null.", "TModel." + column.Name);
					}

					sb.AppendLine();
					sb.Append('\t');
					sb.Append(column.Name);
					sb.Append(" = ");
					sb.Append(db.BuildParameterName(column.Name));
					sb.Append(" AND");
				}
			}

			sb.Length -= 4; // " AND".Length == 4;

			dbCommand.CommandText = sb.ToString();

			db.ExecuteNonQuery(dbCommand);
		}

		#endregion Methods
	}
}