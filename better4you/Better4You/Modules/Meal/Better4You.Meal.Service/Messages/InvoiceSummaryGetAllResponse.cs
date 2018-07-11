using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class InvoiceSummaryGetAllResponse: PageableResponse
    {
        [DataMember]
        public List<InvoiceSummaryView> Invoices { get; set; }

        public InvoiceSummaryGetAllResponse()
        {
            Invoices = new List<InvoiceSummaryView>();
        }
    }
}