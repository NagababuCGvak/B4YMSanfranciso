using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class DateRangeReportOrderItemMealMenuView:IView
    {
        [DataMember]
        public GeneralItemView MenuType { get; set; }
        [DataMember]
        public double TotalPrice { get; set; }
        [DataMember]
        public long TotalCount { get; set; }
    }
}