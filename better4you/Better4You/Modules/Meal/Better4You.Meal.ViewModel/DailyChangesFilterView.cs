using System;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class DailyChangesFilterView:IView
    {
        [DataMember]
        public DateTime? OrderItemDate { get; set; }
        [DataMember]
        public long MealTypeId { get; set; }
  
    }
}
