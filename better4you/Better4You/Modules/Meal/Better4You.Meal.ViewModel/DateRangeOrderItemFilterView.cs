using System;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class DateRangeOrderItemFilterView : IView
    {
        
        [DataMember]
        public DateTime? StartDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }
        
        [DataMember]
        public string SchoolName { get; set; }

    }
}
