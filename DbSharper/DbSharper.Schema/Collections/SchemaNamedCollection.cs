using System.Collections.ObjectModel;

namespace DbSharper.Schema.Collections
{
	public class SchemaNamedCollection<T> : KeyedCollection<string, T>
		where T : ISchema
	{
		#region Methods

		public bool Contains(string schema, string name)
		{
			return this.Contains(schema + "." + name);
		}

		public T GetItem(string schema, string name)
		{
			return this[schema + "." + name];
		}

		protected override string GetKeyForItem(T item)
		{
			return item.Schema + "." + item.Name;
		}

		#endregion Methods
	}
}