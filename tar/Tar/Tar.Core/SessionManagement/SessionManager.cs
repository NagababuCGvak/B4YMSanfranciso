namespace Tar.Core.SessionManagement
{
    public static class SessionManager
    {
        #region
        public static T Get<T>(string key)
        {
            return Current.Get<T>(key);
        }

        public static void Set<T>(string key, T value)
        {
            Current.Set(key, value);
        }
        public static T Get<T>()
        {
            return Current.Get<T>();
        }

        public static void Set<T>(T value)
        {
            Current.Set(value);
        }
        #endregion

        private static volatile ISessionStorage _sessionManager;
        private static readonly object LockObject = new object();

        public static ISessionStorage Current
        {
            get
            {
                if (_sessionManager == null)
                {
                    lock (LockObject)
                    {
                        if (_sessionManager == null)
                        {
                            _sessionManager = new WebSessionStorage();
                        }
                    }
                }
                return _sessionManager;
            }
        }
    }
}