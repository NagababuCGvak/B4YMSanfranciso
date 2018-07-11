using System;
using System.Web;
using System.Web.Mvc;

namespace Better4You.UI.Mvc.Core
{
    public class MvcHtmlHelper<TModel>
    {
        private readonly HtmlHelper<TModel> _html;

        public MvcHtmlHelper(HtmlHelper<TModel> html)
        {
            if (html == null) throw new ArgumentNullException("html");
            _html = html;
        }

        public MvcHtmlString TextBoxFor<TProperty>(System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression, bool label)
        {
            var tb = new TagBuilder("input");
            var tmp = expression.Body.ToString(); 
            
            var id = tmp;

            if (tmp.StartsWith("model."))
                id = expression.Body.ToString().Remove(0, 6);
            else if (tmp.Contains("cshtml).Model"))
                id = tmp.Substring(tmp.IndexOf("cshtml).Model.", StringComparison.InvariantCulture) + 14);

            tb.GenerateId(id);
            tb.MergeAttribute("name", id);
            tb.MergeAttribute("type", "text");
            tb.MergeAttribute("data-toogle", "tooltip");
            tb.MergeAttribute("title", id);
            tb.MergeAttribute("placeholder", id);
            return new MvcHtmlString(tb.ToString());
        }

        public MvcHtmlString PasswordFor<TProperty>(System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression, bool label)
        {
            var tb = new TagBuilder("input");
            var id = expression.Body.ToString().Remove(0, 6);
            tb.GenerateId(id);
            tb.MergeAttribute("name", id);
            tb.MergeAttribute("type", "password");
            tb.MergeAttribute("data-toogle", "tooltip");
            tb.MergeAttribute("title", id);
            tb.MergeAttribute("placeholder", id);
            return new MvcHtmlString(tb.ToString());
        }

        public MvcHtmlString Error()
        {
            const string formattedErrorMessage =
                @"<div class='alert alert-danger'>
                    <button type='button' class='close' data-dismiss='alert'>&times;</button>
                    <h4>Error!</h4>
                    {0}
                </div>";

            if (_html.ViewContext.TempData.ErrorMessage() != null)
            {
                return
                    new MvcHtmlString(string.Format(formattedErrorMessage, _html.ViewContext.TempData.ErrorMessage()));
            }

            if (_html.ViewBag.ErrorMessage == null) return new MvcHtmlString("");
            
            return new MvcHtmlString(string.Format(formattedErrorMessage, _html.ViewBag.ErrorMessage));
        }

        public MvcHtmlString Info()
        {
            const string formattedErrorMessage =
                @"<div class='alert alert-info'>
                    <button type='button' class='close' data-dismiss='alert'>&times;</button>
                    <h4>Info!</h4>
                    {0}
                </div>";

            if (_html.ViewContext.TempData.InfoMessage() != null)
            {
                return new MvcHtmlString(string.Format(formattedErrorMessage, _html.ViewContext.TempData.InfoMessage()));
            }

            if (_html.ViewBag.InfoMessage == null) return new MvcHtmlString("");

            return new MvcHtmlString(string.Format(formattedErrorMessage, _html.ViewBag.InfoMessage));
        }

        public SubmitButton SubmitButton(string id, string text)
        {
            return new SubmitButton(id, text).Primary();
        }

        public Button Button(string id, string text)
        {
            return new Button(id, text);
        }

        public SubmitButton CancelButton()
        {
            return new SubmitButton("action.cancel", "Cancel");
        }

        public ReturnButton ReturnButton()
        {
            return new ReturnButton();
        }

        public SubmitButton DeleteButton2()
        {
            return new SubmitButton("action.delete", "Delete").Danger();
        }
        public DeleteButton DeleteButton(string action, string controller, string id)
        {
            return new DeleteButton(action, controller, id);
        }
        public ActionButton ActionButton(string text, string action, string controller, string id)
        {
            return new ActionButton(text, action, controller, id);
        }
        public ActionButton ActionButton(string text, string action, string controller)
        {
            return new ActionButton(text, action, controller, null);
        }
    }
}