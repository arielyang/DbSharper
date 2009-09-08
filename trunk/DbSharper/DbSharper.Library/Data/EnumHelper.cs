namespace DbSharper.Library.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;

	/// <summary>
	/// A helper class to get additional information from a enum type.
	/// </summary>
    public static class EnumHelper
    {
        #region Fields

        private static volatile Dictionary<string, Dictionary<string, string>> cache = new Dictionary<string, Dictionary<string, string>>();
        private static object syncRoot = new object();

        #endregion Fields

        #region Methods

        /// <summary>
        /// Convert a enum type to a data binding object.
        /// </summary>
        /// <param name="enumType">Type of enum.</param>
        /// <returns>Data binding object.</returns>
        public static IList<KeyValuePair<string, int>> GetDataSource(Type enumType)
        {
            if (enumType.BaseType != typeof(Enum))
            {
                throw new ArgumentException("Type enumType must inherit from System.Enum.");
            }

            DescriptionAttribute att;
            FieldInfo field;
            int value;

            FieldInfo[] fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);

            IList<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();

            for (int i = 0, length = fields.Length; i < length; i++)
            {
                field = fields[i];

                value = Convert.ToInt32(Enum.Parse(enumType, field.Name), CultureInfo.InvariantCulture);

                att = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (att != null)
                {
                    list.Add(new KeyValuePair<string, int>(att.Description, value));
                }
                else
                {
                    list.Add(new KeyValuePair<string, int>(field.Name, value));
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
            string typeName = enumType.FullName;

            if (!cache.ContainsKey(typeName))
            {
                lock (syncRoot)
                {
                    if (!cache.ContainsKey(typeName))
                    {
                        DescriptionAttribute att;
                        FieldInfo field;

                        FieldInfo[] fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);

                        Dictionary<string, string> descriptions = new Dictionary<string, string>();

                        for (int i = 0, length = fields.Length; i < length; i++)
                        {
                            field = fields[i];

                            att = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                            if (att != null)
                            {
                                descriptions.Add(field.Name, att.Description);
                            }
                            else
                            {
                                descriptions.Add(field.Name, field.Name);
                            }
                        }

                        cache.Add(typeName, descriptions);
                    }
                }
            }

            return cache[typeName][memberName];
        }

        #endregion Methods
    }
}