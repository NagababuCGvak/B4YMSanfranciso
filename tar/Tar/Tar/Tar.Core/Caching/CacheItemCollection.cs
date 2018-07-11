using System.Collections.ObjectModel;

namespace Tar.Core.Caching
{
    public class CacheItemCollection : KeyedCollection<string, ICacheItem>
    {
        #region Overrides of KeyedCollection<string,ICacheItem>

        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <returns>
        /// The key for the specified element.
        /// </returns>
        /// <param name="item">The element from which to extract the key.
        ///                 </param>
        protected override string GetKeyForItem(ICacheItem item)
        {
            return item.Key;
        }

        #endregion
    }
}
