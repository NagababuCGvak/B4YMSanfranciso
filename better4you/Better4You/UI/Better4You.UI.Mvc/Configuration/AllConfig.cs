using System.Web.Mvc;
using System.Web.Routing;

namespace Better4You.UI.Mvc.Configuration
{
    public static class AllConfig
    {
        public static void Configure()
        {
            // Area Registration
            AreaRegistration.RegisterAllAreas();

            // Dependency
            DependencyConfig.Configure();

            // Bundle
            BundleConfig.Configure();

            // Filter
            FilterConfig.Configure();

            // Route
            ServiceApiConfig.Configure(RouteTable.Routes);
            RouteConfig.Configure(RouteTable.Routes);

            // Serialization
            SerializationConfig.Configure();

            // Metadata
            ModelMetadataConfig.Configure();
        }
    }
}