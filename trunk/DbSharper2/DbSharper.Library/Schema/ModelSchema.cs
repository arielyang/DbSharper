using System.Collections.Generic;

namespace DbSharper.Library.Schema
{
	public class ModelSchema
	{
		#region Properties

		public string Name
		{
			get;
			set;
		}

		public Dictionary<string, PropertySchema> Properties
		{
			get;
			set;
		}

		public string Schema
		{
			get;
			set;
		}

		#endregion Properties
	}
}