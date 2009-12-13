using DbSharper.Schema.Database;
using DbSharper.Schema.Infrastructure;

namespace DbSharper.Schema.Infrastructure
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