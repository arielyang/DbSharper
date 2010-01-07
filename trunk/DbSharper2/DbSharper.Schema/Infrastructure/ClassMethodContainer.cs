namespace DbSharper.Schema.Infrastructure
{
	/// <summary>
	/// Container of a class name and a method name.
	/// </summary>
	internal class ClassMethodContainer
	{
		#region Properties

		internal string ClassName
		{
			get;
			set;
		}

		internal string MethodName
		{
			get;
			set;
		}

		#endregion Properties

		#region Methods

		public override string ToString()
		{
			return this.ClassName + "." + this.MethodName;
		}

		#endregion Methods
	}
}