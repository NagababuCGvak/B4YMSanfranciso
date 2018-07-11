using System.Collections.Generic;

namespace Tar.Core.ObjectContainers
{
    public abstract class ObjectContainerBase : IObjectContainer
    {
        public abstract void DoSet(string key, object value);
        public abstract object DoGet(string key);
        public List<string> Keys { get; private set; }
        public void Set(string key, object value)
        {
            DoSet(key, value);
        }

        public object Get(string key)
        {
            return DoGet(key);
        }

        public object this[string key]
        {
            get { return Get(key); }
            set { Set(key, value); }
        }

        protected ObjectContainerBase()
        {
            Keys = new List<string>();
        }
    }
}