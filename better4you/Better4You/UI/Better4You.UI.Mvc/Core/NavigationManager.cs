using System.Collections.Generic;

namespace Better4You.UI.Mvc.Core
{
    public static class NavigationManager
    {
        public static Dictionary<long, List<NavigationItem>> NavItems = new Dictionary<long, List<NavigationItem>>
        {
            {0,new List<NavigationItem>()},
            {111001,new List<NavigationItem>
            {
                new NavigationItem{ ControllerName="School", ActionName="Index",        Actions=new string[]{}, IconClass="clip-arrow-right",Title="Manage Schools"},
                new NavigationItem{ ControllerName="User", ActionName="Index",        Actions=new string[]{}, IconClass="fa fa-group",Title="Manage Users"},
                new NavigationItem{ ControllerName="Food", ActionName="Index",        Actions=new string[]{}, IconClass="clip-arrow-right",Title="Manage Items"},
                new NavigationItem{ ControllerName="Menu", ActionName="Index",        Actions=new string[]{}, IconClass="clip-arrow-right",Title="Manage Meals"},
                new NavigationItem{ ControllerName="Meal", ActionName="Monthly",        Actions=new string[]{}, IconClass="fa fa-calendar",Title="Build Monthly Menu"},
                new NavigationItem{ ControllerName="MealOrder", ActionName="OrderList",        Actions=new []{"OrderList"}, IconClass="fa fa-shopping-cart",Title="View School Orders"},
                new NavigationItem{ ControllerName="Invoice", ActionName="Index",        Actions=new []{"Index"}, IconClass="clip-arrow-right",Title="Manage Invoices"},
                new NavigationItem{ ControllerName="Invoice", ActionName="InvoiceSummary",        Actions=new []{"InvoiceSummary"}, IconClass="clip-arrow-right",Title="Accounting Summary"},
                new NavigationItem{ ControllerName="Invoice", ActionName="DateRangeBilling",        Actions=new []{"DateRangeBilling"}, IconClass="clip-arrow-right",Title="Weekly Billing Report"},
                new NavigationItem{ ControllerName="Reports", ActionName="Index",        Actions=new string[]{}, IconClass="clip-arrow-right",Title="Reports"}
            }
            },
            {111002,new List<NavigationItem>
            {
                new NavigationItem{ControllerName ="Account",ActionName = "SelectSchool",Actions = new string[]{"SelectSchool"},Title = "Select School", IconClass = "fa fa-check"},
                new NavigationItem{ControllerName ="MealOrder",ActionName = "Manage",Actions = new string[]{},Title = "Manage Orders", IconClass = "fa fa-shopping-cart"},
                new NavigationItem{ControllerName ="Account",ActionName = "ContactInfo",Actions = new []{ "ContactInfo"},Title = "Contact B4YM", IconClass = "clip-arrow-right"},
                new NavigationItem{ControllerName ="Invoice",ActionName = "SchoolInvoice",Actions = new string[]{},Title = "Invoices", IconClass = "clip-arrow-right"},
                new NavigationItem{ControllerName ="Milk",ActionName = "Percentage",Actions = new string[]{},Title = "Milk Percentage", IconClass = "clip-arrow-right"},
                new NavigationItem{ControllerName ="Food",ActionName = "Percentage",Actions = new string[]{},Title = "Fruit/Veg Percentage", IconClass = "clip-arrow-right"}
            }}
        };

        public static List<NavigationItem> GetNavItems(long userTypeId)
        {
            return NavItems.ContainsKey(userTypeId) ? NavItems[userTypeId] : null;
        }
    }
}