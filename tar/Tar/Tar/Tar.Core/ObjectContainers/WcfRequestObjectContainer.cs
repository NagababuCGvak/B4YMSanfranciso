using System.ServiceModel;

namespace Tar.Core.ObjectContainers
{
    public class WcfRequestObjectContainer : ObjectContainerBase
    {
        public override void DoSet(string key, object value)
        {
            GetWcfObjectContainer()[key] = value;
        }

        public override object DoGet(string key)
        {
            return GetWcfObjectContainer()[key];
        }

        protected static WcfObjectContainerExtension GetWcfObjectContainer()
        {
            var extension = OperationContext.Current.Channel.Extensions.Find<WcfObjectContainerExtension>();
            if (extension == null)
            {
                extension = new WcfObjectContainerExtension();
                OperationContext.Current.Channel.Extensions.Add(extension);
            }
            return extension;
        }
    }
}