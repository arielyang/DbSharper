using DbSharper.Schema.Collections;

namespace DbSharper.Schema.Database
{
	public class Enumeration : IName
	{
		#region Constructors

		public Enumeration()
		{
			this.Members = new NamedCollection<EnumerationMember>();
		}

		#endregion Constructors

		#region Properties

		public string BaseType
		{
			get; set;
		}

		public string Description
		{
			get; set;
		}

		public bool HasFlagsAttribute
		{
			get; set;
		}

		public NamedCollection<EnumerationMember> Members
		{
			get; set;
		}

		public string Name
		{
			get; set;
		}

		#endregion Properties
	}
}