using DbSharper.Schema.Collections;
using DbSharper.Schema.Database;

namespace DbSharper.Schema
{
	public interface IColumns : ISchema
	{
		#region Properties

		NamedCollection<Column> Columns
		{
			get;
		}

		#endregion Properties
	}
}