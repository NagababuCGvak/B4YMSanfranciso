using System;
using Better4You.Meal.ViewModel;

namespace Better4You.UI.Mvc.ViewModels
{
    public class MonthlyExportViewData
    {
        public string SchoolName { get; set; }
        public long SchoolId { get; set; }

        private string _strSchoolRoute;
        public string strSchoolRoute
        {
            get { return _strSchoolRoute; }
            set
            {
                int route;
                if (Int32.TryParse(value, out route))
                {
                    SchoolRoute = route;
                }
                _strSchoolRoute = value;
            }
        }
        public int SchoolRoute { get; set; }
        public string SchoolType { get; set; }
        public string MealServiceType { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderReportMenuView Orders { get; set; }
        public int TotalCount { get; set; }
        public long? RefId { get; set; }
    }
}