namespace Tar.Core.LoggingOld2
{
    interface IObjectContainer
    {
        object Get(object key);
        void Set(object key, object value);
    }
}