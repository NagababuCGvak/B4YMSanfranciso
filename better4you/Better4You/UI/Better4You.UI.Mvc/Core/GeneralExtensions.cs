using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Better4You.Core;
using Tar.Core.SessionManagement;
using Tar.Security;
using System.Web.Routing;
using System.Collections.Specialized;

namespace Better4You.UI.Mvc.Core
{
    public static class GeneralExtensions
    {
        public static SelectList ToSelectList(this IList<GeneralItemView> items, object selectedItem)
        {
            return new SelectList(items.Select(item =>
                                               new SelectListItem
                                                   {
                                                       Text = item.Text,
                                                       Value = item.Value
                                                   }),
                                                   "Value",
                                                   "Text",
                                  selectedItem);
        }

        public static SelectList ToSelectList(this List<Tar.ViewModel.GeneralItemView> items, object selectedItem)
        {
            return new SelectList(items.Select(item =>
                                               new SelectListItem
                                               {
                                                   Text = item.Text,
                                                   Value = item.Id.ToString()
                                               }),
                                                   "Value",
                                                   "Text",
                                  selectedItem);
        }
        public static CurrentUser CurrentUser(this HtmlHelper source)
        {
            return SessionManager.Get<CurrentUser>();
        }
        public static RouteValueDictionary ToRouteValues(this NameValueCollection col, object obj)
        {
            var values = new RouteValueDictionary(obj);
            if (col != null)
            {
                foreach (string key in col)
                {
                    if (key!=null && !values.ContainsKey(key)) values[key] = col[key];
                }
            }
            return values;
        }
    }
}