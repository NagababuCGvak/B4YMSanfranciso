namespace Tar.Core.Caching
{
    public class CacheItem : ICacheItem
    {
        #region Implementation of ICacheItem
        public string Key { get; set; }
        public object Value { get; set; }
        public ICacheValidator Validator { get; set; }
        #endregion

        public CacheItem()
        {
            Validator = NullCacheValidator.Instance;
        }
    }
}