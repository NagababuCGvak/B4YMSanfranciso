using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Tar.Core;

namespace Better4You.UI.Mvc.Configuration
{
    public class DependencyResolver : IDependencyResolver
    {
        public void Dispose()
        {
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return ServiceLocator.Current.Get(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return ServiceLocator.Current.GetAll(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }
    }
}