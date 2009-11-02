using DbSharper.Schema.Database;
using DbSharper.Schema.Collections;
namespace DbSharper.Schema
{
	public interface IColumns : ISchema
	{
		#region Properties

		NamedCollection<Column> Columns
		{
			get;
			set;
		}

		#endregion Properties
	}
}
