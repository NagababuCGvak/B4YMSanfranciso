using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Tar.Core.Wcf
{
    public class WcfServiceHostServiceBehavior : IServiceBehavior
    {
        private readonly Type _contractType;

        public WcfServiceHostServiceBehavior(Type contractType)
        {
            if (contractType == null) throw new ArgumentNullException("contractType");
            _contractType = contractType;
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                var cd = channelDispatcher as ChannelDispatcher;

                if (cd != null)
                {
                    foreach (var ed in cd.Endpoints)
                    {
                        ed.DispatchRuntime.InstanceProvider = new WcfServiceHostInstanceProvider(_contractType);
                    }
                }
            }
        }
    }
}