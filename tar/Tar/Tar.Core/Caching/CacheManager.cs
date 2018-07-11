using System;
using System.Diagnostics;
using System.Linq;

namespace Tar.Core.Caching
{
    public class CacheManager : ICacheManager
    {
        #region Implementation of ICacheManager
        private CacheItemCollection CachedItems { get; set; }
        private static object _lockObject = new object();
        public object Get(string key)
        {
            lock (_lockObject)
            {
                if (!CachedItems.Contains(key)) return null;
                return CachedItems[key].Validator.Valid ? CachedItems[key].Value : null;
            }
        }

        public void Remove(string key)
        {
            lock (_lockObject)
            {
                if (CachedItems.Any(d => d.Key == key))
                    CachedItems.Remove(key);
            }
        }

        public void Set(string key, object value)
        {
            Set(key, value, NullCacheValidator.Instance);
        }

        public void Set(string key, object value, ICacheValidator validator)
        {
            lock (_lockObject)
            {
                Remove(key);
                CachedItems.Add(new CacheItem { Key = key, Value = value, Validator = validator });
            }
        }

        public void RemoveInvalidItems()
        {
            lock (_lockObject)
            {
                Trace.WriteLine(DateTime.Now + " - Tar.Core.Caching.ICacheManager --> Removing invalid cached items....");
                var existsItemCount = CachedItems.Count;
                CachedItems.Where(x => !x.Validator.Valid).ToList().ForEach(x => Remove(x.Key));
                var currentItemCount = CachedItems.Count;
                var removedItemCount = existsItemCount - currentItemCount;
                Trace.WriteLine(
                    string.Format(DateTime.Now +
                                  " - Tar.Core.Caching.ICacheManager --> Removed invalid cached items. Total existing item count:{0}, removed item count:{1}, current item count:{2}",
                        existsItemCount,
                        removedItemCount,
                        currentItemCount));
            }
        }
        #endregion

        public CacheManager()
        {
            CachedItems = new CacheItemCollection();
        }
    }
}