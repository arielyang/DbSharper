using DbSharper.Schema.Collections;

namespace DbSharper.Schema.Database
{
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