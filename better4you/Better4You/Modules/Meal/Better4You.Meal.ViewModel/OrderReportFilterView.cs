using System;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class OrderReportFilterView : IView
    {
        [DataMember]
        public long? OrderId { get; set; }

        [DataMember]
        public DateTime? OrderDate { get; set; }

        [DataMember]
        public DateTime? OrderStartDate { get; set; }
  
        [DataMember]
        public DateTime? OrderEndDate { get; set; }

        [DataMember]
        public string SchoolNameStartsWith { get; set; }

        [DataMember]
        public long MealTypeId { get; set; }

        [DataMember]
        public long SchoolTypeId { get; set; }             
  
    }
}
