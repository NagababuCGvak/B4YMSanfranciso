using System.Web;

namespace Tar.Core.ObjectContainers
{
    public class WebRequestObjectContainer : ObjectContainerBase
    {
        public override void DoSet(string key, object value)
        {
            HttpContext.Current.Items[key] = value;
        }

        public override object DoGet(string key)
        {
            return HttpContext.Current.Items[key];
        }
    }
}