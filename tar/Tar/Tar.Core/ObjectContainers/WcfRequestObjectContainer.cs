using System;
using System.Diagnostics;
using System.ServiceModel;

namespace Tar.Core.ObjectContainers
{
    public class WcfRequestObjectContainer : ObjectContainerBase
    {
        private static bool _traceIsActive = false;
        public override void DoSet(string key, object value)
        {
            var wcfObjectContainerExtension = GetWcfObjectContainer();
            if (wcfObjectContainerExtension.ContainsKey(key))
                wcfObjectContainerExtension[key] = value;
            else
                wcfObjectContainerExtension.Add(key, value);
        }

        public override object DoGet(string key)
        {
            var wcfObjectContainerExtension = GetWcfObjectContainer();
            return wcfObjectContainerExtension.ContainsKey(key) ? wcfObjectContainerExtension[key] : null;
        }

        protected static WcfObjectContainerExtension GetWcfObjectContainer()
        {
            try
            {
                var extension = OperationContext.Current.Extensions.Find<WcfObjectContainerExtension>();
                if (extension == null)
                {
                    extension = new WcfObjectContainerExtension();
                    OperationContext.Current.Extensions.Add(extension);
                }
                if (_traceIsActive)
                    Trace.WriteLine("WcfObjectContainerExtension#" + extension.GetHashCode() +
                                    "#OperationContext.IsNull:" + (OperationContext.Current == null));
                return extension;
            }
            catch (Exception exception)
            {
                Trace.WriteLine("WcfObjectContainerExtension.Exception#" + exception);
                throw;
            }
        }
    }
}