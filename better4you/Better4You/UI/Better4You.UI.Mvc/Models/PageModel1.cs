using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Better4You.UI.Mvc.Models
{
    public abstract class PageModel1
    {
        protected PageModel1(string title)
        {
            if (title == null) throw new ArgumentNullException("title");
            Title = title;
        }

        public string Title { get; set; }
        public abstract List<BreadcrumbsModel> Breadcrumbs { get; }
    }
}