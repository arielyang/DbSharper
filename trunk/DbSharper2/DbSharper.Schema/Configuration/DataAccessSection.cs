namespace DbSharper.Schema.Configuration
{
	internal sealed class DataAccessSection : SectionBase
	{
		#region Constructors

		public DataAccessSection(string methodMask)
			: base()
		{
			this.MethodMask = methodMask;
		}

		#endregion Constructors

		#region Properties

		public string MethodMask
		{
			get; private set;
		}

		#endregion Properties
	}
}