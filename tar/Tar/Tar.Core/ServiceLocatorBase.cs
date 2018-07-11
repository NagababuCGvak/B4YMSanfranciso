using System;
using System.Linq;

namespace Tar.Core
{
    public abstract class ServiceLocatorBase : IServiceLocator
    {
        public abstract object Get(string key, Type service);
        public abstract object[] GetAll(Type service);

        public virtual object Get(Type service)
        {
            return Get(null, service);
        }

        public abstract object BuildUp(Type serviceType, object component);

        public virtual object GetService(Type serviceType)
        {
            return Get(serviceType);
        }
        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }
        public T Get<T>(string key)
        {
            return (T)Get(key, typeof(T));
        }
        public T[] GetAll<T>()
        {
            return GetAll(typeof(T)).Select(x => (T)x).ToArray();
        }

        public abstract void AddConfig(string configFile);
    }
}