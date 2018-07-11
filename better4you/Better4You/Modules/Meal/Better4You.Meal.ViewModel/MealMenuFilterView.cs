using System;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class MealMenuFilterView : IView
    {
        [DataMember]
        public long MealTypeId { get; set; }

        [DataMember]
        public long? MenuId { get; set; }

        [DataMember]
        public DateTime? StartDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }
        
        [DataMember]
        public long RecordStatusId { get; set; }
    }
}
