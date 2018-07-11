using System;
using System.Collections.Generic;

namespace Better4You.UI.Mvc.Models
{
    public abstract class PageModel
    {
        protected PageModel(string title)
        {
            if (title == null) throw new ArgumentNullException("title");
            Title = title;
        }

        public string Title { get; set; }
        public abstract List<BreadcrumbsModel> Breadcrumbs { get; }
       
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    