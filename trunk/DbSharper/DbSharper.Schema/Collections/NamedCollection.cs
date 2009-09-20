using System.Collections.ObjectModel;

namespace DbSharper.Schema.Collections
{
	public class NamedCollection<T> : KeyedCollection<string, T>
		where T : IName
	{
		#region Methods

		protected override string GetKeyForItem(T item)
		{
			return item.Name;
		}

		#endregion Methods
	}
}