using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace Tar.Core.ObjectContainers
{
    public class EnvironmentObjectContainer : ObjectContainerBase
    {
        private volatile static IObjectContainer _wcf = new WcfRequestObjectContainer();
        private volatile static IObjectContainer _web = new WebRequestObjectContainer();
        private static volatile IObjectContainer _thread = new ThreadObjectContainer();
        private static bool traceIsActive = false;
        public Func<IObjectContainer> GetContainer = () =>
        {
            if (OperationContext.Current == null)
                return HttpContext.Current != null ? _web : _thread;
            return _wcf;
        };

        private IObjectContainer Container
        {
            get
            {
                var result = GetContainer();
                Trace.WriteLineIf(traceIsActive, result.GetType().Name);
                if (!traceIsActive) return result;

                var stackTrace = new StackTrace();
                // ReSharper disable once AssignNullToNotNullAttribute
                stackTrace.GetFrames().Select(x => "Type:" + x.GetMethod().DeclaringType + ", Method:" + x.GetMethod().Name).ToList()
                    .ForEach(x => Trace.WriteLine(x));
                return result;
            }
        }
        public override void DoSet(string key, object value)
        {
            Trace.WriteLineIf(traceIsActive, "DoSet:" + key + "," + value + "#" + value.GetHashCode());
            Container.Set(key, value);
        }

        public override object DoGet(string key)
        {
            var result = Container.Get(key);
            Trace.WriteLineIf(traceIsActive, "DoGet:" + key + "|" +
                            (result == null
                                ? "NULL#NULL"
                                : result.GetType().Name + result.GetHashCode()));
            return result;
        }
    }
}