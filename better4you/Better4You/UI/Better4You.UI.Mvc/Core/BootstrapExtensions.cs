using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Better4You.UI.Mvc.Core
{
    public static class BootstrapExtensions
    {
        public enum ButtonType
        {
            Excel,
            Word,
            Add,
            AddUser,
            AddBuilding,
            Remove,
            Search,
            Edit,
            Save,
            Cancel
        }
        public static MvcHtmlString BootstrapEditButton(this HtmlHelper helper, string text, string type)
        {
            /*
            return new MvcHtmlString(String.Format("<button class='btn btn-primary' {0}><i class='icon-edit icon-white'></i> {1}</button>",
                string.IsNullOrWhiteSpace(type)?"":string.Format("type='{0}'",type), text));  
            */
            return new MvcHtmlString(String.Format("<button class='btn' {0}><i class='cus-page-white-edit'></i> {1}</button>",
                string.IsNullOrWhiteSpace(type)?"":string.Format("type='{0}'",type), text));  

            
        }
        public static MvcHtmlString BootstrapAddButton(this HtmlHelper helper, string text, string type)
        {
            /*
            return new MvcHtmlString(String.Format("<button class='btn btn-primary' {0}><i class='icon-plus icon-white'></i> {1}</button>",
                                 string.IsNullOrWhiteSpace(type) ? "" : string.Format("type='{0}'", type), text));
            */
            return new MvcHtmlString(String.Format("<button class='btn' {0}><i class='cus-add'></i> {1}</button>",
                     string.IsNullOrWhiteSpace(type) ? "" : string.Format("type='{0}'", type), text));

        }
        public static MvcHtmlString BootstrapRemoveButton(this HtmlHelper helper, string text, string type)
        {
            return helper.BootstrapRemoveButton(text, type,null);
            //return new MvcHtmlString(String.Format("<button class='btn btn-danger' {0}><i class='icon-remove'></i> {1}</button>",
            //                     string.IsNullOrWhiteSpace(type) ? "" : string.Format("type='{0}'", type), text));

        }
        public static MvcHtmlString BootstrapRemoveButton(this HtmlHelper helper, string text, string type, object htmlAttributes)
        {
            var attributes = (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes);
            var attributesString = "";
            //var defClass = "btn btn-danger";
            var defClass = "btn";
            foreach (var attribute in attributes)
            {
                if (attribute.Key == "class")
                {
                    defClass = defClass + " " + attribute.Value;
                }
                attributesString = string.Format("{0}='{1}'", attribute.Key, attribute.Value);
            }
            /*
            return new MvcHtmlString(String.Format("<button class='{3}' {0} {1}><i class='icon-remove icon-white'></i> {2}</button>",
                                 string.IsNullOrWhiteSpace(type) ? "" : string.Format("type='{0}'", type), attributesString, text, defClass));
            */
            return new MvcHtmlString(String.Format("<button class='{3}' {0} {1}><i class='cus-delete'></i> {2}</button>",
                                 string.IsNullOrWhiteSpace(type) ? "" : string.Format("type='{0}'", type), attributesString, text, defClass));

        }
        public static MvcHtmlString BootstrapSearchButton(this HtmlHelper helper, string text, string type)
        {
            return helper.BootstrapSearchButton(text, type, null);
        }
        public static MvcHtmlString BootstrapSearchButton(this HtmlHelper helper, string text, string type, object htmlAttributes)
        {
            var attributes = (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes);
            var attributesString = "";
            //var defClass = "btn btn-primary";
            var defClass = "btn";
            foreach (var attribute in attributes)
            {
                if(attribute.Key=="class")
                {
                    defClass = defClass + " " + attribute.Value;
                }
                attributesString = string.Format("{0}='{1}'", attribute.Key, attribute.Value);
            }
            /*
            return new MvcHtmlString(String.Format("<button class='{3}' {0} {1}><i class='icon-search  icon-white'></i> {2}</button>",
                                 string.IsNullOrWhiteSpace(type) ? "" : string.Format("type='{0}'", type), attributesString,text,defClass));
            */
            return new MvcHtmlString(String.Format("<button class='{3}' {0} {1}><i class='cus-find'></i> {2}</button>",
                     string.IsNullOrWhiteSpace(type) ? "" : string.Format("type='{0}'", type), attributesString, text, defClass));

        }
        public static MvcHtmlString BootstrapCustomButton(this HtmlHelper helper, ButtonType buttonType,string text, string type)
        {
            return helper.BootstrapCustomButton(buttonType,text, type, null);
        }
        public static MvcHtmlString BootstrapCustomButton(this HtmlHelper helper, ButtonType buttonType,string text, string type, object htmlAttributes)
        {
            var attributes = (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes);
            var attributesString = "";
            var defClass = "btn";
            foreach (var attribute in attributes)
            {
                if (attribute.Key == "class")
                {
                    defClass = defClass + " " + attribute.Value;
                }
                attributesString += string.Format(" {0}='{1}' ", attribute.Key, attribute.Value);
            }
            var iconClass = "";
            switch (buttonType)
            {
                case ButtonType.Excel:
                    iconClass = "cus-page-white-excel";
                    break;
                case ButtonType.Word:
                    iconClass = "cus-page-white-excel";
                    break;
                case ButtonType.Cancel:
                    iconClass = "cus-cancel";
                    break;
                case ButtonType.Remove:
                    iconClass = "cus-delete";
                    break;
                case ButtonType.Add:
                    iconClass = "cus-add";
                    break;
                case ButtonType.AddUser:
                    iconClass = "cus-user-add";
                    break;
                case ButtonType.AddBuilding:
                    iconClass = "cus-building-add";
                    break;
                case ButtonType.Edit:
                    iconClass = "cus-page-white-edit";
                    break;
                case ButtonType.Save:
                    iconClass = "cus-disk";
                    break;
                case ButtonType.Search:
                    iconClass = "cus-find";
                    break;

            }
            if (iconClass != "")
                iconClass = string.Format("<i class='{0}'></i>", iconClass);
            
            return new MvcHtmlString(String.Format("<button class='{3}' {0} {1}>{4} {2}</button>",
                     string.IsNullOrWhiteSpace(type) ? "" : string.Format("type='{0}'", type), attributesString, text, defClass,iconClass));

        }
        public static MvcHtmlString BootstrapTextBoxFor<TModel,TProperty>(this HtmlHelper<TModel> htmlHelper,
                Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var member = expression.Body as MemberExpression;
            var attributes = (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes);


            if (member != null)
            {
                #region Zahir
                // added by Zahir
                var displayName = htmlHelper.DisplayNameFor(expression);
                attributes.Add("title", displayName);
                attributes.Add("placeholder", displayName);

                object display;
                if (attributes.TryGetValue("display", out display))
                {
                    attributes.Remove("display");
                    attributes["title"] = display;
                    attributes["placeholder"] = display;
                }
                #endregion
            }
            attributes.Add("autocomplete", "off");
            attributes.Add("data-toggle", "tooltip");
            return htmlHelper.TextBoxFor(expression, attributes);
        }
        public static MvcHtmlString BootstrapPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var member = expression.Body as MemberExpression;
            var attributes = (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes);


            if (member != null)
            {
                #region Zahir
                // added by Zahir
                var displayName = htmlHelper.DisplayNameFor(expression);
                attributes.Add("title", displayName);
                attributes.Add("placeholder", displayName);

                // commented by Zahir
                //attributes.Add("title", member.Member.Name);
                //attributes.Add("placeholder", member.Member.Name);
                object display;
                if (attributes.TryGetValue("display", out display))
                {
                    attributes.Remove("display");
                    attributes["title"] = display;
                    attributes["placeholder"] = display;
                }
                #endregion
            }            
            attributes.Add("data-toggle","tooltip");
            return htmlHelper.PasswordFor(expression, attributes);
        }
    }
}