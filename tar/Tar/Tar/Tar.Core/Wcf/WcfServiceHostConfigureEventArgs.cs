using System;
using System.ServiceModel;

namespace Tar.Core.Wcf
{
    public class WcfServiceHostConfigureEventArgs : EventArgs
    {
        public ServiceHost ServiceHost { get; private set; }
        public bool Cancel { get; set; }
        public Type ServiceType { get; private set; }
        public Type ContractType { get; private set; }

        public WcfServiceHostConfigureEventArgs(ServiceHost serviceHost, Type contractType,Type serviceType)
        {
            if (serviceHost == null) throw new ArgumentNullException("serviceHost");
            if (contractType == null) throw new ArgumentNullException("contractType");
            if (serviceType == null) throw new ArgumentNullException("serviceType");
            
            ServiceHost = serviceHost;
            ContractType = contractType;
            ServiceType = serviceType;
        }
    }
}
