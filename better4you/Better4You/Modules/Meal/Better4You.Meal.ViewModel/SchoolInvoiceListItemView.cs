using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class SchoolInvoiceListItemView : IView
    {
        [DataMember]
        public List<InvoiceListItemView> InvoiceList { get; set; }

        [DataMember]
        public string SchoolName { get; set; }

        [DataMember]
        public long SchoolId { get; set; }

        [DataMember]
        public string SchoolType { get; set; }

        [DataMember]
        public double? TotalAmount { get; set; }

        [DataMember]
        public Guid DocumentGuid { get; set; }

        public SchoolInvoiceListItemView()
        {
            InvoiceList = new List<InvoiceListItemView>();
        }
    }
}
