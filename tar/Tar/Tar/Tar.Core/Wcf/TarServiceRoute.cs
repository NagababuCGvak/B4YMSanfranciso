using System;
using System.ServiceModel.Activation;

namespace Tar.Core.Wcf
{
    public class TarServiceRoute : ServiceRoute
    {
        public TarServiceRoute(string serviceName, Type serviceType)
            : base("Services." + serviceName + ".svc", new TarWebServiceHostFactory(), serviceType)
        {
        }

        public TarServiceRoute(Type serviceType)
            : this(serviceType.Name, serviceType)
        {
        }
    }
}
