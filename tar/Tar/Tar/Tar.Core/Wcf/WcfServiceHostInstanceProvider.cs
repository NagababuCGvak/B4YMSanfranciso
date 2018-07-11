using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Tar.Core.Wcf
{
    public class WcfServiceHostInstanceProvider : IInstanceProvider
    {
        private readonly Type _serviceType;

        public WcfServiceHostInstanceProvider(Type serviceType)
        {
            if (serviceType == null) throw new ArgumentNullException("serviceType");
            _serviceType = serviceType;
        }

        public IServiceLocator Locator
        {
            get { return ServiceLocator.Current; }
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return Locator.Get(_serviceType);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return Locator.Get(_serviceType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }
    }
}