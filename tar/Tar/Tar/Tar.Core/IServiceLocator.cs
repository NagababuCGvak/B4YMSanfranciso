using System;

namespace Tar.Core
{
    public interface IServiceLocator:IServiceProvider
    {
        object Get(string key, Type service);
        object[] GetAll(Type service);
        object Get(Type service);
        object BuildUp(Type serviceType, object component);
        
        T Get<T>();
        T Get<T>(string key);
        T[] GetAll<T>();

        void AddConfig(string configFile);
    }
}