using System.Web.Mvc;

namespace Better4You.UI.Mvc.Core
{
    public static class HtmlExtensions
    {
        public static MvcHtmlHelper<T> Mvc<T>(this HtmlHelper<T> html)
        {
            return new MvcHtmlHelper<T>(html);
        }
    }
}