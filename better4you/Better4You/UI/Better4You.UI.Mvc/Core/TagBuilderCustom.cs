using System.Web;
using System.Web.Mvc;
using Better4You.UI.Mvc.Models;

namespace Better4You.UI.Mvc.Core
{
    public class TagBuilderCustom:IHtmlString
    {
        private readonly TagBuilder _builder;
        public TagBuilderCustom(string tagName)
        {
            _builder = new TagBuilder(tagName);
        }

        public TagBuilderCustom MergeAttribute(string key, string value)
        {
            _builder.MergeAttribute(key, value, true);
            return this;
        }
        public TagBuilderCustom AddCssClass(string value)
        {
            _builder.AddCssClass(value);
            return this;
        }

        public static TagBuilderCustom Create(string tagName)
        {
            return new TagBuilderCustom(tagName);
        }

        public override string ToString()
        {
            return _builder.ToString();
        }

        public string ToHtmlString()
        {
            return _builder.ToString();
            //return new MvcHtmlString(_builder.ToString()).ToString();
        }

        public TagBuilderCustom SetInnerText(string innerText)
        {
            _builder.SetInnerText(innerText);
            return this;
        }
        public TagBuilderCustom SetInnerHtml(string innerHtml)
        {
            _builder.InnerHtml = innerHtml;
            return this;
        }
    }
}