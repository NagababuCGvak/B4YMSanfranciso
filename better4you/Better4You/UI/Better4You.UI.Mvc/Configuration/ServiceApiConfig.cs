using System.Web.Http;
using System.Web.Routing;

namespace Better4You.UI.Mvc.Configuration
{
    public class ServiceApiConfig
    {
        public static void Configure(RouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "service.api",
                routeTemplate: "service.api/{serviceKey}/{methodName}",
                defaults: new { controller = "ServiceApi", action = "Execute" });

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            ////Url: http://localhost:33911/Flex.Crm.svc
            //routes.Add(new TarServiceRoute("Flex.Crm", typeof(ICrmService)));
        }
    }
}