using System;
using System.Linq;
using System.Web.Mvc;

namespace Better4You.UI.Mvc.Filters
{
    public class ButtonAttribute : ActionFilterAttribute
    {
        public string ButtonKey { get; set; }
        public string ParameterKey { get; set; }

        public ButtonAttribute(string commonKey)
            :this(commonKey,commonKey)
        {
            if (commonKey == null) throw new ArgumentNullException("commonKey");
        }

        public ButtonAttribute(string buttonKey, string parameterKey)
        {
            if (buttonKey == null) throw new ArgumentNullException("buttonKey");
            if (parameterKey == null) throw new ArgumentNullException("parameterKey");
            ButtonKey = buttonKey;
            ParameterKey = parameterKey;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var actionParameters = filterContext.ActionParameters;

            var actionParameterKey = actionParameters.Keys.FirstOrDefault(
                i => ParameterKey.ToLowerInvariant().Equals(i.ToLowerInvariant()));

            if (actionParameterKey != null)
            {
                actionParameters[actionParameterKey] = request.Params.AllKeys.Any(
                    k => k.ToLowerInvariant() == ButtonKey.ToLowerInvariant());
            }
        }
    }
}