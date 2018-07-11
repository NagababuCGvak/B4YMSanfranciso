using System.Web.Routing;

namespace Better4You.UI.Mvc.Configuration
{
    public static class ClipOneConfig
    {
        public static void Configure()
        {
            // Dependency
            DependencyConfig.Configure();

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