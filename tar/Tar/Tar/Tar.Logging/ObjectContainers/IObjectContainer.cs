namespace Tar.Logging.ObjectContainers
{
    public interface IObjectContainer
    {
        object Get(object key);
        void Set(object key, object value);
    }
}