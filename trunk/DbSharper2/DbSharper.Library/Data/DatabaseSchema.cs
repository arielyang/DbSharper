using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

using DbSharper.Library.Schema;

namespace DbSharper.Library.Data
{
	public sealed class DatabaseSchema
	{
		#region Fields

		private static IDictionary<string, IDictionary<string, ColumnAttribute>> columns;
		private static object lockObject = new object();
		private static IDictionary<string, string> tables;

		#endregion Fields

		#region Constructors

		private DatabaseSchema()
		{
		}

		#endregion Constructors

		#region Methods

		public static IDictionary<string, ColumnAttribute> GetColumns(Type modelType)
		{
			if (columns == null)
			{
				lock (lockObject)
				{
					if (columns == null)
					{
						columns = new Dictionary<string, IDictionary<string, ColumnAttribute>>();

						Assembly assembly = Assembly.GetAssembly(modelType);

						Type[] types = assembly.GetTypes();

						foreach (Type type in types)
						{
							if (type.BaseType == typeof(ModelBase))
							{
								IDictionary<string, ColumnAttribute> cols = new Dictionary<string, ColumnAttribute>();

								PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

								foreach (PropertyInfo propertyInfo in propertyInfos)
								{
									object[] attributes = propertyInfo.GetCustomAttributes(typeof(ColumnAttribute), false);

									if (attributes.Length == 1)
									{
										cols.Add(propertyInfo.Name, (ColumnAttribute)attributes[0]);
									}
								}

								columns.Add(type.FullName, cols);
							}
						}
					}
				}
			}

			return columns[modelType.FullName];
		}

		public static string GetTableName(Type modelType)
		{
			if (tables == null)
			{
				lock (lockObject)
				{
					if (tables == null)
					{
						tables = new Dictionary<string, string>();

						Assembly assembly = Assembly.GetAssembly(modelType);

						Type[] types = assembly.GetTypes();

						foreach (Type type in types)
						{
							if (type.BaseType == typeof(ModelBase))
							{
								object[] attributes = type.GetCustomAttributes(typeof(TableAttribute), false);

								if (attributes.Length == 1)
								{
									tables.Add(type.FullName, ((TableAttribute)attributes[0]).Name);
								}
							}
						}
					}
				}
			}

			string fullName = modelType.FullName;

			if (tables.ContainsKey(fullName))
			{
				return tables[fullName];
			}

			return null;
		}

		#endregion Methods
	}
}