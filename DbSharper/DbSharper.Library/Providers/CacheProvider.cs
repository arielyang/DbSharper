namespace DbSharper.Library.Providers
{
    using System.Configuration.Provider;

    /// <summary>
    /// Abstract CacheProvider class.
    /// </summary>
    public abstract class CacheProvider : ProviderBase
    {
        #region Methods

        /// <summary>
        /// Retrieves the specified item from cache.
        /// </summary>
        /// <param name="key">The identifier for the cache item to retrieve.</param>
        /// <returns>The retrieved cache item, or null if the key is not found.</returns>
        public abstract object Get(string key);

        /// <summary>
        /// Inserts an item into cache with a cache key to reference its location.
        /// </summary>
        /// <param name="key">The cache key used to reference the item.</param>
        /// <param name="value">The object to be inserted into the cache.</param>
        /// <param name="duration">The interval between the time the inserted object is last accessed and the time at which that object expires. By seconds.</param>
        public abstract void Insert(string key, object value, int duration);

        /// <summary>
        /// Removes the specified item from the application's cache.
        /// </summary>
        /// <param name="key">A System.String identifier for the cache item to remove.</param>
        public abstract void Remove(string key);

        /// <summary>
        /// Removes a batch of items from the application's cache.
        /// </summary>
        /// <param name="tag">A System.String flag for the cache items to remove.</param>
        public abstract void RemoveBatch(string tag);

        #endregion Methods
    }
}