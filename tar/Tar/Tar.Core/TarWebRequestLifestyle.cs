using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle;
using Tar.Core.ObjectContainers;

namespace Tar.Core
{
    public class TarWebRequestLifestyle : AbstractLifestyleManager
    {
        private readonly string _perWebRequestObjectId = "twrl_" + Guid.NewGuid();
        private readonly IObjectContainer _container = new EnvironmentObjectContainer();
        public override object Resolve(CreationContext context, IReleasePolicy releasePolicy)
        {
            var result = _container.Get(_perWebRequestObjectId);
            if (result == null)
            {
                result = base.Resolve(context, releasePolicy);
                _container.Set(_perWebRequestObjectId, result);
            }
            return result;
        }
        public override bool Release(object instance)
        {
            var disposable = instance as IDisposable;
            if (disposable != null)
                disposable.Dispose();
            return true;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    //public class TarWebRequestLifestyleOld : AbstractLifestyleManager
    //{
    //    private readonly string _perWebRequestObjectId = "twrl_" + Guid.NewGuid();
    //    private object _component;
    //    private static readonly Dictionary<string, object> StaticObjects = new Dictionary<string, object>();

    //    public override object Resolve(CreationContext context, IReleasePolicy releasePolicy)
    //    {
    //        if (HttpContext.Current != null)
    //            _component = HttpContext.Current.Items[_perWebRequestObjectId] ??
    //                         (HttpContext.Current.Items[_perWebRequestObjectId] = base.Resolve(context, releasePolicy));
    //        else if (OperationContext.Current != null && !string.IsNullOrEmpty(OperationContext.Current.SessionId))
    //        {
    //            var objectContainer = GetWcfObjectContainer();
    //            _component = objectContainer.ContainsKey(_perWebRequestObjectId)
    //                             ? objectContainer[_perWebRequestObjectId]
    //                             : (objectContainer[_perWebRequestObjectId] = base.Resolve(context, releasePolicy));
    //        }
    //        else
    //        {
    //            if (!StaticObjects.ContainsKey(_perWebRequestObjectId))
    //                StaticObjects[_perWebRequestObjectId] = base.Resolve(context, releasePolicy);
    //            _component = StaticObjects[_perWebRequestObjectId];
    //        }
    //        return _component;
    //    }

    //    private WcfObjectContainerExtension GetWcfObjectContainer()
    //    {
    //        var extension = OperationContext.Current.Channel.Extensions.Find<WcfObjectContainerExtension>();
    //        if (extension == null)
    //        {
    //            extension = new WcfObjectContainerExtension();
    //            OperationContext.Current.Channel.Extensions.Add(extension);
    //        }
    //        return extension;
    //    }

    //    public override bool Release(object instance)
    //    {
    //        if (_component is IDisposable)
    //            ((IDisposable)_component).Dispose();
    //        _component = null;

    //        if (StaticObjects.ContainsKey(_perWebRequestObjectId))
    //            StaticObjects.Remove(_perWebRequestObjectId);

    //        _component = null;
    //        return base.Release(instance);
    //    }

    //    public override void Dispose()
    //    {
    //        GC.SuppressFinalize(this);
    //    }
    //}
}