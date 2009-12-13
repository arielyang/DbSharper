namespace DbSharper.CodeGenerator
{
	internal interface IChangable
	{
		#region Properties

		bool IsChanged
		{
			get;
		}

		#endregion Properties
	}
}