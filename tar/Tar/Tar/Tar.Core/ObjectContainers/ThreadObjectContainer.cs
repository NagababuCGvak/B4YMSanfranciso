using System.Collections.Generic;
using System.Threading;

namespace Tar.Core.ObjectContainers
{
    public class ThreadObjectContainer : ObjectContainerBase
    {
        //[ThreadStatic]
        //public static IDictionary<string, object> Container;
        private static string ContainerKey
        {
            get { return "Tar.ThreadObjectContainer"; }
        }
        private static IDictionary<string, object> Container
        {
            get { return (IDictionary<string, object>)Thread.GetData(Thread.GetNamedDataSlot(ContainerKey)); }
            set { Thread.SetData(Thread.GetNamedDataSlot(ContainerKey), value); }
        }
        private static IDictionary<string, object> GetContainer()
        {
            return Container ?? (Container = new Dictionary<string, object>());
        }

        public override void DoSet(string key, object value)
        {
            GetContainer()[key]=value;
        }

        public override object DoGet(string key)
        {
            var container = GetContainer();
            return container.ContainsKey(key) ? GetContainer()[key] : null;
        }
    }
}