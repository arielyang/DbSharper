namespace DbSharper.Library.Model
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Base class of collection.
    /// </summary>
    /// <typeparam name="T">Type of item.</typeparam>
    public abstract class CollectionBase<T> : Collection<T>
        where T : ItemBase
    {
        #region Fields

        private bool innerListChanged = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// A flag indicates whether the collection is changed.
        /// </summary>
        protected bool InnerListChanged
        {
            get { return this.innerListChanged; }
            set { this.innerListChanged = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
		/// Removes all elements from the System.Collections.ObjectModel.Collection&lt;T&gt;.
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();

            this.innerListChanged = true;
        }

        /// <summary>
        /// Inserts an element into the System.Collections.ObjectModel.Collection&lt;T&gt; at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);

            this.innerListChanged = true;
        }

        /// <summary>
        /// Removes the element at the specified index of the System.Collections.ObjectModel.Collection&lt;T&gt;.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);

            this.innerListChanged = true;
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index. The value can be null for reference types.</param>
        protected override void SetItem(int index, T item)
        {
            base.SetItem(index, item);

            this.innerListChanged = true;
        }

        #endregion Methods
    }
}