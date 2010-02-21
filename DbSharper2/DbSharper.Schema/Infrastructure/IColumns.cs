using DbSharper2.Schema.Database;
using DbSharper2.Schema.Infrastructure;

namespace DbSharper2.Schema.Infrastructure
{
	public interface IColumns : ISchema
	{
		#region Properties

		NamedCollection<Column> Columns
		{
			get;
		}

		NamedCollection<Index> Indexes
		{
			get;
		}

		#endregion Properties
	}
}