using System.Web;
using System.Web.Mvc;

namespace Better4You.UI.Mvc.Core
{
    public abstract class MvcControl<T> :IHtmlString where T:MvcControl<T>
    {
        public T InnerHtml(string html)
        {
            Builder.InnerHtml = html;
            return _self;
        }

        protected TagBuilder Builder;
        private readonly T _self;
        protected MvcControl(string tagName)
        {
            Builder = new TagBuilder(tagName);
            _self = (T) this;
        }

        public virtual string ToHtmlString()
        {
            return Builder.ToString();
        }

        public T AddAttribute(string key, string value)
        {
            Builder.Attributes.Add(key, value);
            return _self;
        }

        public T AddCssClass(string className)
        {
            Builder.AddCssClass(className);
            return _self;
        }
    }
}