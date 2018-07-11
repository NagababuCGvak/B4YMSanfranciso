namespace Tar.Core.SessionManagement
{
    public interface ISessionStorage
    {
        T Get<T>(string key);
        void Set<T>(string key,T value);
        T Get<T>();
        void Set<T>(T value);
    }
}
