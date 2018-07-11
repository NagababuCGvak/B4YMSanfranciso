using System.Linq;
using System.Web.Mvc;
using Tar.Core;

namespace Better4You.UI.Mvc.Configuration
{
    public static class DependencyConfig
    {
        public static void Configure()
        {
            ControllerBuilder.Current.SetControllerFactory(new MvcControllerFactory(ServiceLocator.Current));
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new DependencyResolver();

            ServiceLocator.Current.GetAll<IBootStrapper>().ToList().ForEach(item => item.BootStrap());
        }
    }
}