using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Xml;

namespace Tar.Core.Wcf
{
    public class WcfServiceHostFactory : ServiceHostFactory
    {
        public Type ServiceType { get; private set; }
        public Type ContractType { get; private set; }
        private readonly List<EndPointParameters> _endPoints = new List<EndPointParameters>();
        private readonly List<IServiceBehavior> _behaviors = new List<IServiceBehavior>();

        private class EndPointParameters
        {
            public string Address { get; set; }
            public Binding Binding { get; set; }
            public List<IEndpointBehavior> Behaviors { get; private set; }

            public EndPointParameters()
            {
                Behaviors = new List<IEndpointBehavior>();
            }
        }

        protected event EventHandler<WcfServiceHostConfigureEventArgs> Configuring;

        protected void OnConfiguring(WcfServiceHostConfigureEventArgs e)
        {
            EventHandler<WcfServiceHostConfigureEventArgs> handler = Configuring;
            if (handler != null) handler(this, e);
        }

        public WcfServiceHostFactory AddEndPoint(string address, Binding binding)
        {
            _endPoints.Add(new EndPointParameters { Address = address, Binding = binding });
            return this;
        }

        public WcfServiceHostFactory AddRestfulEndPoint(
            string address,
            bool helpEnabled = false,
            bool automaticFormatSelectionEnabled = false,
            WebMessageBodyStyle defaultBodyStyle = WebMessageBodyStyle.Bare,
            WebMessageFormat defaultOutgoingRequestFormat = WebMessageFormat.Xml,
            WebMessageFormat defaultOutgoingResponseFormat = WebMessageFormat.Xml,
            bool crossDomainScriptAccessEnabled = false)
        {
            var endPointPrameters = new EndPointParameters
                                    {
                                        Address = address,
                                        Binding = new WebHttpBinding
                                                  {
                                                      CrossDomainScriptAccessEnabled = crossDomainScriptAccessEnabled,
                                                      CloseTimeout = new TimeSpan(0, 15, 0),
                                                      OpenTimeout = new TimeSpan(0, 15, 0),
                                                      ReceiveTimeout = new TimeSpan(0, 15, 0),
                                                      SendTimeout = new TimeSpan(0, 15, 0),
                                                      AllowCookies = false,
                                                      BypassProxyOnLocal = false,
                                                      HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                                                      MaxBufferSize = int.MaxValue,
                                                      MaxBufferPoolSize = int.MaxValue,
                                                      MaxReceivedMessageSize = int.MaxValue,
                                                      TransferMode = TransferMode.Buffered,
                                                      UseDefaultWebProxy = true,
                                                      ReaderQuotas = new XmlDictionaryReaderQuotas
                                                                     {
                                                                         MaxDepth = 2000000,
                                                                         MaxStringContentLength = int.MaxValue,
                                                                         MaxArrayLength = int.MaxValue,
                                                                         MaxBytesPerRead = int.MaxValue,
                                                                         MaxNameTableCharCount = int.MaxValue
                                                                     },
                                                      Security = new WebHttpSecurity {Mode = WebHttpSecurityMode.None}
                                                  }
                                    };
            endPointPrameters.Behaviors
                .Add(new WebHttpBehavior
                         {

                             AutomaticFormatSelectionEnabled = automaticFormatSelectionEnabled,
                             DefaultOutgoingRequestFormat = defaultOutgoingRequestFormat,
                             DefaultOutgoingResponseFormat = defaultOutgoingResponseFormat,
                             DefaultBodyStyle = defaultBodyStyle,
                             HelpEnabled = helpEnabled,
                         });
            _endPoints.Add(endPointPrameters);
            return this;
        }

        public WcfServiceHostFactory AddSecureRestfulEndPoint(
           string address,
           bool helpEnabled = false,
           bool automaticFormatSelectionEnabled = false,
           WebMessageBodyStyle defaultBodyStyle = WebMessageBodyStyle.Bare,
           WebMessageFormat defaultOutgoingRequestFormat = WebMessageFormat.Xml,
           WebMessageFormat defaultOutgoingResponseFormat = WebMessageFormat.Xml,
           bool crossDomainScriptAccessEnabled = false)
        {
            var endPointPrameters = new EndPointParameters
            {
                Address = address,
                Binding = new WebHttpBinding
                {
                    CrossDomainScriptAccessEnabled = crossDomainScriptAccessEnabled,
                    CloseTimeout = new TimeSpan(0, 15, 0),
                    OpenTimeout = new TimeSpan(0, 15, 0),
                    ReceiveTimeout = new TimeSpan(0, 15, 0),
                    SendTimeout = new TimeSpan(0, 15, 0),
                    AllowCookies = false,
                    BypassProxyOnLocal = false,
                    HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                    MaxBufferSize = int.MaxValue,
                    MaxBufferPoolSize = int.MaxValue,
                    MaxReceivedMessageSize = int.MaxValue,
                    TransferMode = TransferMode.Buffered,
                    UseDefaultWebProxy = true,
                    ReaderQuotas = new XmlDictionaryReaderQuotas
                    {
                        MaxDepth = 2000000,
                        MaxStringContentLength = int.MaxValue,
                        MaxArrayLength = int.MaxValue,
                        MaxBytesPerRead = int.MaxValue,
                        MaxNameTableCharCount = int.MaxValue
                    },
                    Security =
                        new WebHttpSecurity
                        {
                            Mode = WebHttpSecurityMode.Transport,
                            Transport = new HttpTransportSecurity { ClientCredentialType = HttpClientCredentialType.None }
                        }
                }
            };
            endPointPrameters.Behaviors
                .Add(new WebHttpBehavior
                {

                    AutomaticFormatSelectionEnabled = true,
                    DefaultOutgoingRequestFormat = WebMessageFormat.Json,
                    DefaultOutgoingResponseFormat = WebMessageFormat.Json,
                    DefaultBodyStyle = WebMessageBodyStyle.Bare,
                    HelpEnabled = true
                });
            _endPoints.Add(endPointPrameters);
            return this;
        }
        public WcfServiceHostFactory ClearEndPoints()
        {
            _endPoints.Clear();
            return this;
        }

        public T GetServiceBehavior<T>() where T : IServiceBehavior
        {
            var serviceBehavior = _behaviors.FirstOrDefault(sb => sb.GetType() == typeof(T));
            if (serviceBehavior == null)
                throw new Exception(string.Format("Not found service behavior. Type: {0}", typeof(T).AssemblyQualifiedName));
            return (T)serviceBehavior;
        }

        public WcfServiceHostFactory AddServiceBehavior(IServiceBehavior behavior)
        {
            _behaviors.Add(behavior);
            return this;
        }

        public WcfServiceHostFactory AddServiceMetaDataBehavior(bool httpGetEnabled)
        {
            return AddServiceBehavior(new ServiceMetadataBehavior { HttpGetEnabled = httpGetEnabled });
        }

        public WcfServiceHostFactory ClearBehaviors()
        {
            _behaviors.Clear();
            return this;
        }

        /// <summary>
        /// Create ServiceHost object for console, windows, windows service etc. standalone applications.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="baseAddresses"></param>
        /// <returns></returns>
        public ServiceHost CreateService(Type contractType, Uri[] baseAddresses)
        {
            return CreateServiceHost(contractType, baseAddresses);
        }

        private static volatile IServiceLocator _serviceLocator = new WindsorServiceLocator(AppSettings.ServiceLocatorConfigFile);

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            Trace.WriteLine(serviceType + "#Creating Service Host...");
            if (serviceType == null) throw new ArgumentNullException("serviceType");
            ContractType = serviceType;
            ServiceType = _serviceLocator.Get(ContractType).GetType();

            var serviceHost = base.CreateServiceHost(ServiceType, baseAddresses);
            serviceHost.Description.Behaviors.Add(new WcfServiceHostServiceBehavior(ContractType));

            serviceHost.Opening += (sender, e) => ServiceOpening((ServiceHost)sender);
            Trace.WriteLine(serviceType + "#Created Service Host.");
            return serviceHost;
        }

        private void ServiceOpening(ServiceHost serviceHost)
        {
            var e = new WcfServiceHostConfigureEventArgs(serviceHost, ContractType, ServiceType);

            OnConfiguring(e);

            if (e.Cancel) return;

            _behaviors.ForEach(b =>
                                   {
                                       if (!serviceHost.Description.Behaviors.Contains(b.GetType()))
                                           serviceHost.Description.Behaviors.Add(b);
                                   });
            _endPoints.ForEach(
                endPointParameter =>
                {
                    var serviceEndpoint = serviceHost.AddServiceEndpoint(ContractType, endPointParameter.Binding,
                                                                         endPointParameter.Address);
                    endPointParameter.Behaviors.ForEach(endPointBehavior => serviceEndpoint.Behaviors.Add(endPointBehavior));
                });
        }
    }
}