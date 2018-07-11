namespace Tar.Core.Caching
{
    public static class CacheExtensions
    {
        public static ICacheManager GetGroup(this ICacheManager cache, string groupKey)
        {
            var key = GenerateGroupKey(groupKey);
            var value = cache.Get(key) as ICacheManager;
            if (value != null) return value;

            value = new CacheManager();
            cache.Set(key, value);
            return value;
        }

        public static void RemoveGroup(this ICacheManager cache, string groupKey)
        {
            var key = GenerateGroupKey(groupKey);
            cache.Remove(key);
        }

        private static string GenerateGroupKey(string groupKey)
        {
            return string.Format("tar_group_" + groupKey);
        }

        public static object Get(this ICacheManager cache, string groupKey, string key)
        {
            return GetGroup(cache, groupKey).Get(key);
        }

        public static void Remove(this ICacheManager cache, string groupKey, string key)
        {
            GetGroup(cache, groupKey).Remove(key);
        }

        public static void Set(this ICacheManager cache, string groupKey, string key, object value)
        {
            GetGroup(cache, groupKey).Set(key, value);
        }

        public static void Set(this ICacheManager cache, string groupKey, string key, object value, ICacheValidator validator)
        {
            GetGroup(cache, groupKey).Set(key, value, validator);
        }
    }
}
