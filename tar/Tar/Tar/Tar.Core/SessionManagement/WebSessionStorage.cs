using System.Web;

namespace Tar.Core.SessionManagement
{
    public class WebSessionStorage : ISessionStorage
    {
        #region Implementation of ISessionStorage

        public T Get<T>(string key)
        {
            var val = HttpContext.Current.Session[key];

            if (val == null) return default(T);

            return (T) val;
        }

        public void Set<T>(string key, T value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public T Get<T>()
        {
            return Get<T>(typeof(T).FullName);
        }

        public void Set<T>(T value)
        {
            Set(typeof (T).FullName, value);
        }

        #endregion
    }
}