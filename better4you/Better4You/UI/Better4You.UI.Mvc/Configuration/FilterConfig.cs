using System.Collections.Generic;
using System.Web.Mvc;
using Better4You.UI.Mvc.Filters;

namespace Better4You.UI.Mvc.Configuration
{
    public class FilterConfig
    {
        public static void Configure()
        {
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
            
            //GlobalFilters.Filters.Add(new AutoLayoutAttribute());
            GlobalFilters.Filters.Add(
                new SelectSchoolAttribute(new List<SelectSchoolAttribute.ActionDefinition>
                {
                    new SelectSchoolAttribute.ActionDefinition("account", "logout")
                }));
            GlobalFilters.Filters.Add(new CancelButtonAttribute());
        }
    }
}