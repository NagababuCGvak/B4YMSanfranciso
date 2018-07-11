namespace Tar.Core.Caching
{
    public class NullCacheValidator : ICacheValidator
    {
        private static NullCacheValidator _instance;

        public static ICacheValidator Instance
        {
            get { return _instance ?? (_instance = new NullCacheValidator()); }
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