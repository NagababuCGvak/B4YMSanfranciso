namespace Tar.Core.Caching
{
    public class NullCacheValidator : ICacheValidator
    {
        private static NullCacheValidator _instance;
        private static object _lockObject = new object();
        public static ICacheValidator Instance
        {
            get {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                            return _instance = new NullCacheValidator();
                    }
                }
                return _instance;
            }
        }

        private NullCacheValidator() { }

        #region Implementation of ICacheValidator
        public bool Valid
        {
            get { return true; }
        }
        #endregion
    }
}