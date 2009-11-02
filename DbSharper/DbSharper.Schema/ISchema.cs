namespace DbSharper.Schema
{
	public interface ISchema : IName
	{
		#region Properties

		string Schema
		{
			get; set;
		}

		#endregion Properties
	}
}