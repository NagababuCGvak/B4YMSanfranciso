using System;
using System.ServiceModel;

namespace Tar.Service.Proxy
{
    public class ServiceProxyFactory<T> : IServiceProxyFactory<T>
    {
        public string EndPointConfigurationName { get; private set; }
        private readonly ChannelFactory<T> _factory;

        public virtual T CreateChannel()
        {
            var channel = _factory.CreateChannel();
            return channel;
        }
        public ServiceProxyFactory(string endPointConfigurationName)
        {
            if (endPointConfigurationName == null) throw new ArgumentNullException("endPointConfigurationName");

            EndPointConfigurationName = endPointConfigurationName;
            _factory = new ChannelFactory<T>(endPointConfigurationName);
        }
    }
}