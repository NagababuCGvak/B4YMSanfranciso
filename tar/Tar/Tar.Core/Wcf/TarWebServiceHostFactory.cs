using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Tar.Core.Wcf
{
    public class TarWebServiceHostFactory : WebServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return base.CreateServiceHost(ServiceLocator.Current.Get(serviceType).GetType(), baseAddresses);
        }
    }
}
