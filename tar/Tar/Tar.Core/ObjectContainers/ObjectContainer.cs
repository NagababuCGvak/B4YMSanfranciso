using System.Collections.Generic;

namespace Tar.Core.ObjectContainers
{
    public class ObjectContainer : ObjectContainerBase
    {
        private readonly IDictionary<string, object> _objects = new Dictionary<string, object>();
        public override void DoSet(string key, object value)
        {
            _objects[key] = value;
        }

        public override object DoGet(string key)
        {
            return _objects.ContainsKey(key) ? _objects[key] : null;
        }
    }
}