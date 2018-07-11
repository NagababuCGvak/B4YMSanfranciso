using System.Collections.Generic;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class DateRangeReportOrderItemMealView:IView
    {
        [DataMember]
        public GeneralItemView MealType { get; set; }

        [DataMember]
        public double TotalPrice { get; set; }

        [DataMember]
        public long TotalCount { get; set; }

        [DataMember]
        public List<DateRangeReportOrderItemMealMenuView> MenuList { get; set; }

        public DateRangeReportOrderItemMealView()
        {
            MenuList = new List<DateRangeReportOrderItemMealMenuView>();
        }

    }
}