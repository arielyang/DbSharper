using System;
using System.Globalization;

using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Configuration
{
	/// <summary>
	/// Mapping rule.
	/// </summary>
	internal sealed class Rule
	{
		#region Constructors

		internal Rule(RuleType ruleType, string name, string prefix, bool? trimPrefix)
		{
			// TODO: Embed string into resource file later.
			string exceptionMessage = "Invalid rule.";

			if (ruleType == RuleType.Schema)
			{
				if (string.IsNullOrEmpty(name) ||
					!string.IsNullOrEmpty(prefix) ||
					trimPrefix.HasValue)
				{
					throw new ArgumentException(exceptionMessage);
				}

			}
			else // Table, View, Procedure
			{
				if (!string.IsNullOrEmpty(name))
				{
					if (!string.IsNullOrEmpty(prefix) || trimPrefix.HasValue)
					{
						throw new ArgumentException(exceptionMessage);
					}
				}
				else if (!string.IsNullOrEmpty(prefix))
				{
					if (!string.IsNullOrEmpty(name))
					{
						throw new ArgumentException(exceptionMessage);
					}
				}
				else
				{
					throw new ArgumentException(exceptionMessage);
				}
			}

			this.RuleType = ruleType;
			this.Name = name;
			this.Prefix = prefix;
			this.TrimPrefix = trimPrefix;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Name string of a rule.
		/// </summary>
		internal string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Prefix string of a rule.
		/// </summary>
		internal string Prefix
		{
			get;
			set;
		}

		/// <summary>
		/// Rule type.
		/// </summary>
		internal RuleType RuleType
		{
			get;
			set;
		}

		/// <summary>
		/// If trim prefix of an object.
		/// </summary>
		internal bool? TrimPrefix
		{
			get;
			set;
		}

		#endregion Properties

		#region Methods

		/// <summary>
		/// Returns description of this rule.
		/// </summary>
		/// <returns>Description of this rule.</returns>
		public override string ToString()
		{
			// Prefix has high priority than name.
			if (!string.IsNullOrEmpty(this.Prefix))
			{
				if (this.TrimPrefix.HasValue && this.TrimPrefix.Value)
				{
					return string.Format(CultureInfo.InvariantCulture, "{0} = {1} and trim {1}", this.RuleType, this.Prefix);
				}
				else
				{
					return string.Format(CultureInfo.InvariantCulture, "{0} = {1}", this.RuleType, this.Prefix);
				}
			}
			else if (!string.IsNullOrEmpty(this.Name))
			{
				return string.Format(CultureInfo.InvariantCulture, "{0} = {1}", this.RuleType, this.Name);
			}

			return "Unknown condition.";
		}

		#endregion Methods
	}
}