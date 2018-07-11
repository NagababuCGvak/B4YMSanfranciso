using System;
using System.ServiceModel;

namespace Tar.Service.Proxy
{
    [Obsolete("Bunu kullanmayın, yakında kaldırılacaktır...",false)]
    public abstract class ServiceProxy<T> : ClientBase<T> where T:class
    {
        protected ServiceProxy(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }
    }
}
