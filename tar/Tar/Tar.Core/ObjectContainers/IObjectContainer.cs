namespace Tar.Core.ObjectContainers
{
    public interface IObjectContainer
    {
        void Set(string key, object value);
        object Get(string key);
        object this[string key] { get; set; }
    }
}