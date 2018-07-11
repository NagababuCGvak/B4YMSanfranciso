using System;
using System.Collections.Generic;
using System.Web;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle;

namespace Tar.Core
{
    public class TarPerSessionLifestyle : AbstractLifestyleManager
    {
        private readonly string _perSessionObjectId = "tpsl_" + Guid.NewGuid();
        private object _component;
        private static readonly Dictionary<string, object> StaticObjects = new Dictionary<string, object>();

        public override object Resolve(CreationContext context, IReleasePolicy releasePolicy)
        {
            if (HttpContext.Current != null)
                _component = HttpContext.Current.Session[_perSessionObjectId] ??
                             (HttpContext.Current.Session[_perSessionObjectId] = base.Resolve(context, releasePolicy));
            else
            {
                if (!StaticObjects.ContainsKey(_perSessionObjectId))
                    StaticObjects[_perSessionObjectId] = base.Resolve(context, releasePolicy);
                _component = StaticObjects[_perSessionObjectId];
            }
            return _component;
        }

        public override void Dispose()
        {
            if (_component is IDisposable)
                ((IDisposable)_component).Dispose();

            if (StaticObjects.ContainsKey(_perSessionObjectId))
                StaticObjects.Remove(_perSessionObjectId);

            _component = null;
        }
    }
}