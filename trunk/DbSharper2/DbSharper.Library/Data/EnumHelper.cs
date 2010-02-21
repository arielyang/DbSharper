using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace DbSharper2.Library.Data
{
	/// <summary>
	/// A helper class to get additional information from an enum type.
	/// </summary>
	public static class EnumHelper
	{
		#region Fields

		private static volatile Dictionary<string, Dictionary<string, string>> cache = new Dictionary<string, Dictionary<string, string>>();
		private static object syncRoot = new object();

		#endregion Fields

		#region Methods

		/// <summary>
		/// Convert an enum type to a data binding object.
		/// </summary>
		/// <param name="enumType">Type of enum.</param>
		/// <returns>Data binding object.</returns>
		public static IList<KeyValuePair<string, int>> GetDataSource(Type enumType)
		{
			if (enumType.BaseType != typeof(Enum))
			{
				throw new ArgumentException("Type enumType must inherit from System.Enum.");
			}

			// Get all fields of the enum.
			FieldInfo[] fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);

			// Create a new instance of binding object.
			IList<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();

			DescriptionAttribute attribute;
			int enumValue;

			foreach (var field in fields)
			{
				// Get attribute of the enum field.
				attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

				// Get value of the enum field.
				enumValue = Convert.ToInt32(Enum.Parse(enumType, field.Name), CultureInfo.InvariantCulture);

				if (attribute != null)
				{
					list.Add(new KeyValuePair<string, int>(attribute.Description, enumValue));
				}
				else // If the enum has not description information, using it's field name instead.
				{
					list.Add(new KeyValuePair<string, int>(field.Name, enumValue));
				}
			}

			return list;
		}

		/// <summary>
		/// Get description attribute of relative enum type and member name.
		/// </summary>
		/// <param name="enumType">Enum type.</param>
		/// <param name="memberName">Enum member name.</param>
		/// <returns>Description of enum member.</returns>
		public static string GetDescription(Type enumType, string memberName)
		{
			string enumTypeName = enumType.FullName;

			if (!cache.ContainsKey(enumTypeName))
			{
				lock (syncRoot)
				{
					if (!cache.ContainsKey(enumTypeName))
					{
						FieldInfo[] fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);

						Dictionary<string, string> descriptions = new Dictionary<string, string>();

						DescriptionAttribute attribute;

						foreach (var field in fields)
						{
							attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

							if (attribute != null)
							{
								descriptions.Add(field.Name, attribute.Description);
							}
							else
							{
								descriptions.Add(field.Name, field.Name);
							}
						}

						cache.Add(enumTypeName, descriptions);
					}
				}
			}

			return cache[enumTypeName][memberName];
		}

		#endregion Methods
	}
}