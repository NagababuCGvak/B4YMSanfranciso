using System.Web.Mvc;
using Better4You.UI.Mvc.Core;
using Tar.Core.SessionManagement;
using Tar.Security;

namespace Better4You.UI.Mvc.Filters
{
    public class AutoLayoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.Layout = Layouts.LayoutFor(SessionManager.Get<CurrentUser>());
            base.OnActionExecuted(filterContext);
        }
    }
}