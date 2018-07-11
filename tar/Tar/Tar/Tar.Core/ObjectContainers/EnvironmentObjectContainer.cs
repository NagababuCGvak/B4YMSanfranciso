using System.ServiceModel;
using System.Web;

namespace Tar.Core.ObjectContainers
{
    public class EnvironmentObjectContainer : ObjectContainerBase
    {
        private readonly IObjectContainer _container;
        public EnvironmentObjectContainer()
        {
            if (HttpContext.Current != null)
                _container = new WebRequestObjectContainer();
            else if (OperationContext.Current != null && !string.IsNullOrEmpty(OperationContext.Current.SessionId))
                _container = new WcfRequestObjectContainer();
            else
                _container = new ThreadObjectContainer();
        }

        public override void DoSet(string key, object value)
        {
            _container.Set(key, value);
        }

        public override object DoGet(string key)
        {
            return _container.Get(key);
        }
    }
}