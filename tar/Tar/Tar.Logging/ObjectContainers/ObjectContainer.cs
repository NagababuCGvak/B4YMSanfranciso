using System.Collections.Generic;

namespace Tar.Logging.ObjectContainers
{
    public class ObjectContainer:IObjectContainer
    {
        private readonly IDictionary<object, object> _objects = new Dictionary<object, object>();

        #region Implementation of IObjectContainer
        public object Get(object key)
        {
            return _objects.ContainsKey(key) ? _objects[key] : null;
        }

        public void Set(object key, object value)
        {
            _objects.Add(key, value);
        }
        #endregion
    }
}