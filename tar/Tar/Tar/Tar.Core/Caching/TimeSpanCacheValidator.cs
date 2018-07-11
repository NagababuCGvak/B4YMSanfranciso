using System;

namespace Tar.Core.Caching
{
    public class TimeSpanCacheValidator : ICacheValidator
    {
        private readonly TimeSpan _timeSpan;
        private readonly DateTime _init;
        public TimeSpanCacheValidator(TimeSpan timeSpan)
        {
            _timeSpan = timeSpan;
            _init = DateTime.Now;
        }

        #region Implementation of ICacheValidator
        public bool Valid
        {
            get
            {
                var now = DateTime.Now;
                return !(now - _init > _timeSpan);
            }
        }
        #endregion
    }
}