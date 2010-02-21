using System;

namespace DbSharper2.Library.Schema
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class ViewAttribute : Attribute
	{
		#region Fields

		private string name;

		#endregion Fields

		#region Constructors

		public ViewAttribute(string name)
		{
			this.name = name;
		}

		#endregion Constructors

		#region Properties

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		#endregion Properties
	}
}