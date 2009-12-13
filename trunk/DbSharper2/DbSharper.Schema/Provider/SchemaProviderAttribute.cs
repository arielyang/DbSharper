using System;

namespace DbSharper.Schema
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SchemaProviderAttribute : Attribute
	{
		#region Fields

		private string providerName;

		#endregion Fields

		#region Constructors

		public SchemaProviderAttribute(string providerName)
		{
			this.providerName = providerName;
		}

		#endregion Constructors

		#region Properties

		public string ProviderName
		{
			get { return providerName; }
		}

		#endregion Properties
	}
}