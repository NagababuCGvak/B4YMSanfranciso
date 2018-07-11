using System.Collections.Generic;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class DateRangeReportOrderItemView:IView
    {
        [DataMember]
        public List<DateRangeReportOrderItemMealView> MealList { get; set; }

        public DateRangeReportOrderItemView()
        {
            MealList= new List<DateRangeReportOrderItemMealView>();
        }

        [DataMember]
        public string SchoolName { get; set; }
    }
}
