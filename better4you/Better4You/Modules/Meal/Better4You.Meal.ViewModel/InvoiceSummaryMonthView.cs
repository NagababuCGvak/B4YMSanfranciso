using System;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class InvoiceSummaryMonthView : IView
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public int Month { get; set; }

        [DataMember]
        public GeneralItemView OrderStatus { get; set; }
        
        [DataMember]
        public double? TotalAmount { get; set; }
        
        [DataMember]
        public double? TotalCredit { get; set; }
        
        [DataMember]
        public double? Rate { get; set; }
        
        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public double? SoyMilkRate { get; set; }

        [DataMember]
        public int SoyMilkCount { get; set; }

        [DataMember]
        public DateTime OrderDate { get; set; }
        [DataMember]
        public Guid SchoolInvoiceDocumentGuid { get; set; }

    }
}
