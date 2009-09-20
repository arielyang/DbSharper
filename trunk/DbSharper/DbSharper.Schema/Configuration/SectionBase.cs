using System;
using System.Collections.ObjectModel;

using DbSharper.Schema.Collections;
using DbSharper.Schema.Database;
using DbSharper.Schema.Enums;

namespace DbSharper.Schema.Configuration
{
	internal abstract class SectionBase
	{
		#region Constructors

		protected SectionBase()
		{
			this.SchemaIncludeConditions = new Collection<Condition>();
			this.SchemaExcludeConditions = new Collection<Condition>();
			this.IncludeConditions = new Collection<Condition>();
			this.ExcludeConditions = new Collection<Condition>();
		}

		#endregion Constructors

		#region Properties

		protected internal Collection<Condition> ExcludeConditions
		{
			get;
			private set;
		}

		protected internal Collection<Condition> IncludeConditions
		{
			get;
			private set;
		}

		protected internal Collection<Condition> SchemaExcludeConditions
		{
			get;
			private set;
		}

		protected internal Collection<Condition> SchemaIncludeConditions
		{
			get;
			private set;
		}

		#endregion Properties

		#region Methods

		public bool IsIncluded(ISchema databaseObject)
		{
			if (this.IsExcluded(databaseObject))
			{
				return false;
			}

			if (this.SchemaIncludeConditions.Count == 0 && this.IncludeConditions.Count == 0)
			{
				return true;
			}

			foreach (Condition condition in this.SchemaIncludeConditions)
			{
				if (string.Compare(databaseObject.Schema, condition.Name, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}

			foreach (Condition condition in this.IncludeConditions)
			{
				if (!ValidateCoditionType(condition, databaseObject))
				{
					continue;
				}

				if (!string.IsNullOrEmpty(condition.Name))
				{
					if (string.Compare(databaseObject.Name, condition.Name, StringComparison.OrdinalIgnoreCase) == 0 ||
						string.Compare(databaseObject.ToString(), condition.Name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return true;
					}
				}
				else if (!string.IsNullOrEmpty(condition.Prefix))
				{
					if (databaseObject.Name.StartsWith(condition.Prefix, StringComparison.OrdinalIgnoreCase) ||
						databaseObject.ToString().StartsWith(condition.Prefix, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}

			return false;
		}

		public string TrimName(IName databaseObject)
		{
			ConditionType databaseObjectType = ConditionType.None;

			if (databaseObject.GetType() == typeof(StoredProcedure))
			{
				databaseObjectType = ConditionType.Procedure;
			}
			else if (databaseObject.GetType() == typeof(Table))
			{
				databaseObjectType = ConditionType.Table;
			}
			else if (databaseObject.GetType() == typeof(View))
			{
				databaseObjectType = ConditionType.View;
			}

			foreach (Condition condition in this.IncludeConditions)
			{
				if (condition.ConditionType == databaseObjectType &&
					condition.TrimPrefix &&
					!string.IsNullOrEmpty(condition.Prefix) &&
					databaseObject.Name.StartsWith(condition.Prefix, StringComparison.OrdinalIgnoreCase))
				{
					string result = databaseObject.Name.Substring(condition.Prefix.Length);

					return result;
				}
			}

			return databaseObject.Name;
		}

		protected bool IsExcluded(ISchema databaseObject)
		{
			if (this.SchemaExcludeConditions.Count == 0 && this.ExcludeConditions.Count == 0)
			{
				return false;
			}

			foreach (Condition condition in this.SchemaExcludeConditions)
			{
				if (string.Compare(databaseObject.Schema, condition.Name, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}

			foreach (Condition condition in this.ExcludeConditions)
			{
				if (!ValidateCoditionType(condition, databaseObject))
				{
					continue;
				}

				if (!string.IsNullOrEmpty(condition.Name))
				{
					if (string.Compare(databaseObject.Name, condition.Name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return true;
					}
				}
				else if (!string.IsNullOrEmpty(condition.Prefix))
				{
					if (databaseObject.Name.StartsWith(condition.Prefix, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}

			return false;
		}

		private static bool ValidateCoditionType(Condition condition, IName databaseObject)
		{
			switch (condition.ConditionType)
			{
				case ConditionType.Table:
					return databaseObject is Table;
				case ConditionType.View:
					return databaseObject is View;
				case ConditionType.Procedure:
					return databaseObject is StoredProcedure;
				default:
					return true;
			}
		}

		#endregion Methods
	}
}