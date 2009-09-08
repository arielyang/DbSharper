namespace DbSharper.Schema.Database
{
    using DbSharper.Schema.Collections;

	public class EnumerationMember : IName
    {
        #region Properties

        public string Description
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public int Value
        {
            get; set;
        }

        #endregion Properties
    }
}