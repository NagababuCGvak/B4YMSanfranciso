using System;
using Tar.Core.Scheduling;

namespace Tar.Core.Caching
{
    public class CacheManagerBootStrapper : IBootStrapper
    {
        private readonly ICacheManager _cacheManager;
        public int PeriodAsMinutes { get; set; }

        public CacheManagerBootStrapper(ICacheManager cacheManager)
        {
            if (cacheManager == null) throw new ArgumentNullException("cacheManager");
            _cacheManager = cacheManager;
            PeriodAsMinutes = 10;
        }

        private static readonly object LockObject = new object();
        private static bool _initialized;

        public void BootStrap()
        {
            lock (LockObject)
            {
                if (_initialized) return;
                ScheduleManager.Start(TimeSpan.FromMinutes(PeriodAsMinutes),
                    () => _cacheManager.RemoveInvalidItems());
                _initialized = true;
            }
        }
    }
}