namespace DbSharper.Schema.Configuration
{
	internal sealed class DataAccessSection : SectionBase
	{
		#region Constructors

		public DataAccessSection(string classMethodMask)
			: base()
		{
			this.ClassMethodMask = classMethodMask;
		}

		#endregion Constructors

		#region Properties

		public string ClassMethodMask
		{
			get;
			private set;
		}

		#endregion Properties
	}
}