namespace DbSharper.CodeGenerator
{
	public interface IChangable
	{
		#region Properties

		bool IsChanged
		{
			get;
		}

		#endregion Properties
	}
}