namespace DbSharper2.Schema.Configuration
{
	internal sealed class ModelSection : SectionBase
	{
		#region Constructors

		public ModelSection(bool autoDiscoverReference)
			: base()
		{
			this.AutoDiscoverReference = autoDiscoverReference;
		}

		#endregion Constructors

		#region Properties

		public bool AutoDiscoverReference
		{
			get;
			private set;
		}

		#endregion Properties
	}
}