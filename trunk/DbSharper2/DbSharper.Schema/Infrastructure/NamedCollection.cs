using System.Collections.ObjectModel;

namespace DbSharper.Schema.Infrastructure
{
	/// <summary>
	/// A collection contains object which implements IName interface.
	/// </summary>
	/// <typeparam name="T">Object implements IName interface.</typeparam>
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