namespace DbSharper.Library.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
	using System.Globalization;

    /// <summary>
    /// Base class of item.
    /// </summary>
    public abstract class ItemBase
    {
        #region Fields

        // Container of changed property Names.
        private List<string> changedPropertyNames;

        // Current field name.
        private string currentFieldName;

        // Container of fields which have not mapping properties.
        private Dictionary<string, object> extendedFields;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected ItemBase()
        {
        }

        /// <summary>
        /// Enumerate each field and evalute relative property.
        /// </summary>
        /// <param name="record">DataRecord.</param>
        protected ItemBase(IDataRecord record)
        {
            LoadFields(record);
        }

        #endregion Constructors

        #region Properties

		/// <summary>
		/// Fields is not defined in properties.
		/// </summary>
		protected Dictionary<string, object> ExtendedFields
		{
			get { return this.extendedFields; }
		}

        #endregion Properties

        #region Methods

        /// <summary>
        /// Determines whether contains the specified field.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <returns>true if contains an field with the specified name; otherwise, false.</returns>
        public bool ContainsField(string fieldName)
        {
			if (string.IsNullOrEmpty(fieldName))
			{
				throw new ArgumentNullException("fieldName");
			}

			if (this.extendedFields == null)
			{
				return false;
			}

            return this.extendedFields.ContainsKey(fieldName);
        }

        /// <summary>
        /// Get value of property by property name.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns>Value of property.</returns>
        public abstract object GetPropertyValue(string propertyName);

        /// <summary>
        /// Return the value of the specified field not defined in property.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Value of field.</returns>
        public T GetValue<T>(string fieldName)
        {
			if (string.IsNullOrEmpty(fieldName))
			{
				throw new ArgumentNullException("fieldName");
			}

			if (!this.ContainsField(fieldName))
			{
				return default(T);
			}

            object field = this.extendedFields[fieldName];

            if (field == DBNull.Value)
            {
                return default(T);
            }

            return (T)field;
        }

        /// <summary>
        /// Return whether the specified field is set to null.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <returns>true if the specified field is set to null; otherwise, false.</returns>
        public bool IsDBNull(string fieldName)
        {
			if (string.IsNullOrEmpty(fieldName))
			{
				throw new ArgumentNullException("fieldName");
			}

			if (!this.ContainsField(fieldName))
			{
				throw new InvalidOperationException(
					string.Format(
						CultureInfo.InvariantCulture,
						"Field '{0}' does not exists.",
						fieldName));
			}

			return this.extendedFields[fieldName] == DBNull.Value;
        }

        /// <summary>
        /// Evalute field value to relative property.
        /// </summary>
        /// <param name="record">DataRecord.</param>
        /// <param name="fieldName">Field Name.</param>
        /// <param name="index">Index in DataRecord.</param>
        public abstract void LoadField(IDataRecord record, string fieldName, int index);

        /// <summary>
        /// Enumerate each field and evalute relative property.
        /// </summary>
        /// <param name="record">DataRecord.</param>
        internal void LoadFields(IDataRecord record)
        {
            for (int i = 0, count = record.FieldCount; i < count; i++)
            {
                this.currentFieldName = record.GetName(i);

                LoadField(record, GetPrimaryFieldName(), i);
            }
        }

        /// <summary>
        /// Add value to fields container.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <param name="value">Value of field.</param>
        protected void AddValue(string fieldName, object value)
        {
			if (string.IsNullOrEmpty(fieldName))
			{
				throw new ArgumentNullException("fieldName");
			}

            if (this.extendedFields == null)
            {
                this.extendedFields = new Dictionary<string, object>();
            }

            this.extendedFields.Add(fieldName, value);
        }

        /// <summary>
        /// Return part of field name after "_", e.g. "Id" of "Member_Id".
        /// </summary>
        /// <returns>string.Empty if thre is no "_" in field name.</returns>
        protected string GetSecondaryFieldName()
        {
            int index = this.currentFieldName.IndexOf('_');

            if (index < 0)
            {
                return string.Empty;
            }

            return this.currentFieldName.Substring(index + 1);
        }

        /// <summary>
        /// Set changed status of property.
        /// </summary>
        /// <param name="name">Property name.</param>
        protected void SetPropertyChanged(string name)
        {
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}

            if (this.changedPropertyNames == null)
            {
                this.changedPropertyNames = new List<string>();
            }

            if (!this.changedPropertyNames.Contains(name))
            {
                this.changedPropertyNames.Add(name);
            }
        }

        /// <summary>
        /// Return part of field name before "_", e.g. "Member" of "Member_Id".
        /// </summary>
        /// <returns>Field name itself if thre is no "_" in field name.</returns>
        private string GetPrimaryFieldName()
        {
            int index = this.currentFieldName.IndexOf('_');

			if (index == 0)
			{
				throw new InvalidOperationException(
					string.Format(
						CultureInfo.InvariantCulture,
						"Can not find primary field name \"{0}\" before '_'.",
						this.currentFieldName));
			}

            if (index < 0)
            {
                return this.currentFieldName;
            }

            return this.currentFieldName.Substring(0, index);
        }

        #endregion Methods
    }
}