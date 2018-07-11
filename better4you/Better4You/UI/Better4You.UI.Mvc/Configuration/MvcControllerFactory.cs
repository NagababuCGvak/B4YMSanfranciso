using System;
using System.Web.Mvc;
using System.Web.Routing;
using Tar.Core;

namespace Better4You.UI.Mvc.Configuration
{
    public class MvcControllerFactory : DefaultControllerFactory
    {
        private readonly IServiceLocator _serviceLocator;

        public MvcControllerFactory(IServiceLocator serviceLocator)
        {
            if (serviceLocator == null) throw new ArgumentNullException("serviceLocator");
            _serviceLocator = serviceLocator;
        }

        public override void ReleaseController(IController controller)
        {
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return null;

            return (IController)_serviceLocator.Get(controllerType);
        }
    }
}