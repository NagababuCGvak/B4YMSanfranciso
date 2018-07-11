using System;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class MealMenuOrderFilterView : IView
    {
        [DataMember]
        public long MealMenuOrderId { get; set; }
        
        [DataMember]
        public long ApplicationId { get; set; }

        [DataMember]
        public long SchoolId { get; set; }

        [DataMember]
        public long SchoolType { get; set; }

        [DataMember]
        public DateTime? OrderDate { get; set; }

        [DataMember]
        public long OrderStatusId { get; set; }

        [DataMember]
        public long RecordStatusId { get; set; }

        [DataMember]
        public long MealTypeId { get; set; }

    }
}
