using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Better4You.Core;
using Tar.Core.SessionManagement;
using Tar.Security;

namespace Better4You.UI.Mvc.Filters
{
    public class SelectSchoolAttribute : ActionFilterAttribute
    {
        public class ActionDefinition
        {
            public string ControllerName { get; set; }
            public string ActionName { get; set; }

            public ActionDefinition(string controllerName, string actionName)
            {
                if (controllerName == null) throw new ArgumentNullException("controllerName");
                if (actionName == null) throw new ArgumentNullException("actionName");

                ControllerName = controllerName;
                ActionName = actionName;
            }
        }
        private readonly IList<ActionDefinition> _ignoreActions;

        public SelectSchoolAttribute(IEnumerable<ActionDefinition> ignoreActions)
        {
            if (ignoreActions == null) throw new ArgumentNullException("ignoreActions");
            _ignoreActions = new List<ActionDefinition>(ignoreActions)
                                 {
                                     new ActionDefinition("Account", "SelectSchool")
                                 };
        }

        public SelectSchoolAttribute()
            :this(new List<ActionDefinition>())
        {
        }

        #region OnActionExecuting
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CanExecute(filterContext))
            {
                if (CurrentUser.CurrentApplicationId() == 0 && CurrentUser.Applications().Count > 1)
                {
                    filterContext.Result = new RedirectResult("~/Account/SelectSchool");
                }
                else if (CurrentUser.UserTypeId() == Convert.ToInt32(UserTypes.School)
                    && CurrentUser.CurrentSchoolId() == 0 && CurrentUser.Schools().Count > 1)
                {
                    filterContext.Result = new RedirectResult("~/Account/SelectSchool");
                }
            }
        }
        #endregion

        #region Helper Members
        private CurrentUser CurrentUser
        {
            get { return SessionManager.Get<CurrentUser>(); }
        }

        private bool IsSelf(ActionDescriptor actionDescriptor)
        {
            var controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = actionDescriptor.ControllerDescriptor.ControllerName;
            return AreEqual(controllerName, "Account") && AreEqual(actionName, "SelectSchool");
        }

        private bool IsIgnore(ActionDescriptor actionDescriptor)
        {
            var controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = actionDescriptor.ActionName;
            return _ignoreActions.Any(action => AreEqual(action.ControllerName, controllerName)
                                                && AreEqual(action.ActionName, actionName));
        }
        private bool CanExecute(ActionExecutingContext filterContext)
        {
            return IsAuthenticated() 
                && !IsSelf(filterContext.ActionDescriptor)
                && !IsIgnore(filterContext.ActionDescriptor);
        }

        private bool IsAuthenticated()
        {
            return CurrentUser != null && CurrentUser.IsAuthenticated;
        }

        private bool AreEqual(string val1, string val2)
        {
            if (val1 == null && val2 == null) return true;
            if (val1 != null) return val1.Equals(val2, StringComparison.InvariantCultureIgnoreCase);
            return false;
        }

        #endregion
    }
}