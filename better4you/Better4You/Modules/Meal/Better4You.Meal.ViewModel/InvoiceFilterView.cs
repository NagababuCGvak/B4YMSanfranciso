using System;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class InvoiceFilterView : IView
    {
        
        [DataMember]
        public long OrderStatusId { get; set; }

        [DataMember]
        public long RecordStatusId { get; set; }
        
        [DataMember]
        public DateTime? OrderDate { get; set; }

        [DataMember]
        public long SchoolId { get; set; }

        [DataMember]
        public long RecordCount { get; set; }

        [DataMember]
        public long MealTypeId { get; set; }

        [DataMember]
        public long OrderId { get; set; }

        [DataMember]
        public string SchoolNameStartsWith { get; set; }

    }
}
