using System.Collections.ObjectModel;

namespace DbSharper2.Schema.Configuration
{
	internal abstract class SectionBase
	{
		#region Constructors

		protected SectionBase()
		{
			this.IncludeRules = new Collection<Rule>();
			this.ExcludeRules = new Collection<Rule>();
		}

		#endregion Constructors

		#region Properties

		internal Collection<Rule> ExcludeRules
		{
			get;
			set;
		}

		internal Collection<Rule> IncludeRules
		{
			get;
			set;
		}

		#endregion Properties
	}
}