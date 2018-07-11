using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class InvoiceGetAllResponse: PageableResponse
    {
        [DataMember]
        public List<InvoiceListItemView> Invoices { get; set; }

        [DataMember]
        public List<SchoolInvoiceListItemView> SchoolInvoices { get; set; }

        public InvoiceGetAllResponse()
        {
            Invoices = new List<InvoiceListItemView>();
            SchoolInvoices = new List<SchoolInvoiceListItemView>();
        }
    }
}