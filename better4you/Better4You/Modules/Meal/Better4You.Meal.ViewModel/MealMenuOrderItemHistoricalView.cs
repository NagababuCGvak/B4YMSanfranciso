using System;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class MealMenuOrderItemHistoricalView : IView
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public int? TotalCount { get; set; }

        [DataMember]
        public string ModifiedByFullName { get; set; }

        [DataMember]
        public DateTime ModifiedAt { get; set; }
        
        [DataMember]
        public string ModifiedReason { get; set; }

        [DataMember]
        public string RecordStatus { get; set; }
        
    }
}
