using System.Globalization;

using DbSharper.Schema.Enums;

namespace DbSharper.Schema.Configuration
{
	internal sealed class Condition
	{
		#region Properties

		public ConditionType ConditionType
		{
			get; set;
		}

		public string Name
		{
			get; set;
		}

		public string Prefix
		{
			get; set;
		}

		public bool TrimPrefix
		{
			get; set;
		}

		#endregion Properties

		#region Methods

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this.Name))
			{
				return string.Format(CultureInfo.InvariantCulture, "{0} = {1}", this.ConditionType, this.Name);
			}
			else if (!string.IsNullOrEmpty(this.Prefix))
			{
				if (this.TrimPrefix)
				{
					return string.Format(CultureInfo.InvariantCulture, "{0} = {1} and trim {1}", this.ConditionType, this.Prefix);
				}
				else
				{
					return string.Format(CultureInfo.InvariantCulture, "{0} = {1}", this.ConditionType, this.Prefix);
				}
			}

			return "Unknown condition.";
		}

		#endregion Methods
	}
}