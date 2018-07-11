using System;

namespace Tar.Core.Caching
{
    public static class CacheValidator
    {
        public static ICacheValidator FromTimeSpan(TimeSpan timeSpan)
        {
            return new TimeSpanCacheValidator(timeSpan);
        }
    }
}