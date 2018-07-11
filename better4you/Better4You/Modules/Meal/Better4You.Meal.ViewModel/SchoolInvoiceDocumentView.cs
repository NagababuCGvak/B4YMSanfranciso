using System;
using System.Runtime.Serialization;
using Tar.ViewModel;


namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class SchoolInvoiceDocumentView : IView
    {
        [DataMember]
        public long SchoolInvoiceDocumentId { get; set; }

        [DataMember]
        public long SchoolId { get; set; }
        
        [DataMember]
        public int InvoiceYear { get; set; }
        
        [DataMember]
        public int InvoiceMonth { get; set; }
        
        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public DateTime CreatedAt { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }

        [DataMember]
        public string CreatedByFullName { get; set; }

        [DataMember]
        public DateTime ModifiedAt { get; set; }

        [DataMember]
        public string ModifiedBy { get; set; }

        [DataMember]
        public string ModifiedByFullName { get; set; }

        [DataMember]
        public string ModifiedReason { get; set; }

        [DataMember]
        public GeneralItemView RecordStatus { get; set; } 

        
        [DataMember]
        public string DocumentName { get; set; }
        
        [DataMember]
        public byte[] DocumentStream { get; set; }
        [DataMember]
        public Guid DocumentGuid { get; set; }
    }
}
