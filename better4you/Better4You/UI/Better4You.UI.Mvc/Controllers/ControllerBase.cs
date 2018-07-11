using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.IO;
using System.Web.Routing;
using System.Web.Security;
using Better4You.UI.Mvc.Core;
using Tar.Core;
using Tar.Core.SessionManagement;
using Tar.Security;
using Better4You.Core;

namespace Better4You.UI.Mvc.Controllers
{
    public abstract class ControllerBase : Controller
    {
        public static int[] Year = new[] { 2013, 2014, 2015,2016,2017,2018,2019};
        public static List<KeyValuePair<long, string>> Month = new List<KeyValuePair<long, string>>
                                                          {
                                                              new KeyValuePair<long, string>(1,"January"), 
                                                              new KeyValuePair<long, string>(2,"February"), 
                                                              new KeyValuePair<long, string>(3,"March"), 
                                                              new KeyValuePair<long, string>(4,"April"), 
                                                              new KeyValuePair<long, string>(5,"May"), 
                                                              new KeyValuePair<long, string>(6,"June"), 
                                                              new KeyValuePair<long, string>(7,"July"), 
                                                              new KeyValuePair<long, string>(8,"August"), 
                                                              new KeyValuePair<long, string>(9,"September"), 
                                                              new KeyValuePair<long, string>(10,"October"), 
                                                              new KeyValuePair<long, string>(11,"November"), 
                                                              new KeyValuePair<long, string>(12,"December")
                                                          };
        protected ControllerBase()
        {
            if (SessionManager.Get<CurrentUser>() == null) SessionManager.Set(new CurrentUser());
        }

        public IServiceLocator ServiceLocator { get; set; }
        public IAuthenticationProvider AuthenticationProvider { get; set; }

        protected CurrentUser CurrentUser
        {
            get
            {
                var cu = SessionManager.Get<CurrentUser>();
                if (cu == null) CurrentUser = new CurrentUser();

                return SessionManager.Get<CurrentUser>();
            }
            set
            {
                SessionManager.Set(value);
            }
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            base.Initialize(requestContext);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var id = User.Identity as FormsIdentity;
                    if (id != null)
                    {
                        if (!CurrentUser.IsAuthenticated)
                        {
                            CurrentUser = AuthenticationProvider.GetUser(User.Identity.Name);
                            CurrentUser.IsAuthenticated = true;
                            
                            //CurrentUser.Data.Add(new KeyValuePair<string, object>(User.));
                        }
                    }
                }
            }
            else
            {
                if (String.Compare(filterContext.RouteData.Values["controller"].ToString(), "Account", StringComparison.InvariantCultureIgnoreCase) != 0)
                {
                    filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Account", action = "Login" }));
                }
            }
            Thread.CurrentPrincipal = CurrentUser;
            HttpContext.User = CurrentUser;
        }

        /// <summary>
        /// Example
        /// string viewHtml = string.Empty;
        /// var viewModel = Execute(request, () => CrmService.GetCustomer(request));
        /// viewHtml = RenderRazorViewToString("CustomerDetail", viewModel);
        /// var hashtable = new Hashtable();
        /// hashtable["viewHtml"] = viewHtml;
        /// return Json(hashtable);       
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;

            using (var stringWriter = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, stringWriter);
                viewResult.View.Render(viewContext, stringWriter);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

                return stringWriter.GetStringBuilder().ToString();
            }
        }

        public bool IsCompanyUser()
        {
            return CurrentUser.UserTypeId() == (int)UserTypes.Company;
        }
        public bool IsSchoolUser()
        {
            return CurrentUser.UserTypeId() == (int)UserTypes.School;
        }

        public ActionResult RedirectToHomeIndex()
        {
            if(IsSchoolUser()) return RedirectToAction("Manage", "MealOrder");
            return RedirectToAction("Index", "School");
        }

        protected string ErrorMessage
        {
            set { TempData.ErrorMessage(value); }
        }

        protected string InfoMessage
        {
            set { TempData.InfoMessage(value); }
        }
    }
}