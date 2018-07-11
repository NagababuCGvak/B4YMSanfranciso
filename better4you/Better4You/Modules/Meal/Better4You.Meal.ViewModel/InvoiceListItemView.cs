using System;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class InvoiceListItemView : IView
    {
        [DataMember]
        public long Id { get; set; }
        
        [DataMember]
        public GeneralItemView OrderStatus { get; set; }

        [DataMember]
        public GeneralItemView RecordStatus { get; set; }
        
        [DataMember]
        public double? TotalAmount { get; set; }
        
        [DataMember]
        public double? TotalCredit { get; set; }
        
        [DataMember]
        public double? TotalAdjusmentCredit { get; set; }
        
        [DataMember]
        public double? Rate { get; set; }
        
        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public int SoyMilkCount { get; set; }

        [DataMember]
        public double? SoyMilkRate { get; set; }

        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        public long SchoolId { get; set; }

        [DataMember]
        public string SchoolName { get; set; }

        [DataMember]
        public string SchoolType { get; set; }

        [DataMember]
        //public string MealType { get; set; }
        public GeneralItemView MealType { get; set; }

        [DataMember]
        public double DebitAmount { get; set; }
        
        [DataMember]
        public string Note { get; set; }
    }
}
