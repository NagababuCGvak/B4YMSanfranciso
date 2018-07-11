namespace Tar.Core.Caching
{
    public interface ICacheManager
    {
        object Get(string key);
        void Remove(string key);
        void Set(string key, object value);
        void Set(string key, object value, ICacheValidator validator);
        void RemoveInvalidItems();
    }
}
