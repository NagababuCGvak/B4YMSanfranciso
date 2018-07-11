using System.Collections.Generic;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class InvoiceSummaryView : IView
    {

        [DataMember]
        public long SchoolId { get; set; }

        [DataMember]
        public string SchoolName { get; set; }

        [DataMember]
        public string SchoolType { get; set; }

        [DataMember]
        public double? TotalAmount { get; set; }

        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public int SoyMilkCount { get; set; }

        [DataMember]
        public List<InvoiceSummaryMonthView> InvoiceItems { get; set; }

        public InvoiceSummaryView()
        {
            InvoiceItems=new List<InvoiceSummaryMonthView>();
        }
    }
}
