using System.Web.Mvc;
using System.Web.Routing;

namespace Better4You.UI.Mvc.Configuration
{
    public class RouteConfig
    {
        public static void Configure(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Meal/Daily",
                url: "Meal/Daily/{year}/{month}/{day}/{mealType}",
                defaults: new {controller = "Meal", action = "Daily"}
                );
            routes.MapRoute(
                name: "MealOrder/Manage",
                url: "MealOrder/Manage/{year}/{month}/{mealTypeId}",
                defaults: new { controller = "MealOrder", action = "Manage" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}