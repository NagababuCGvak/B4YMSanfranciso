using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Tar.Core.Wcf
{
    public class WcfServiceHostInstanceProvider : IInstanceProvider
    {
        private readonly Type _serviceType;
        private static bool _traceIsActive = false;
        private IServiceLocator Locator
        {
            get
            {
                return ServiceLocator.Current;
            }
        }
        public WcfServiceHostInstanceProvider(Type serviceType)
        {
            if (serviceType == null) throw new ArgumentNullException("serviceType");
            _serviceType = serviceType;
            Trace.WriteLineIf(_traceIsActive, "WcfServiceHostInstanceProvider.ctor#" + serviceType);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            var result = Locator.Get(_serviceType);
            Trace.WriteLineIf(_traceIsActive, _serviceType + "#1#" + result.GetHashCode());
            return result;
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            var result = Locator.Get(_serviceType);
            Trace.WriteLineIf(_traceIsActive, _serviceType + "#2#" + result.GetHashCode());
            return result;
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }
    }
}