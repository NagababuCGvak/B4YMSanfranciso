using System;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class MealMenuOrderItemFilterView : IView
    {
        [DataMember]
        public long? OrderId { get; set; }

        [DataMember]
        public long? OrderItemId { get; set; }

        [DataMember]
        public long MealMenuId { get; set; }

        [DataMember]
        public long SchoolId { get; set; }

        [DataMember]
        public DateTime? OrderDate { get; set; }

    }
}
