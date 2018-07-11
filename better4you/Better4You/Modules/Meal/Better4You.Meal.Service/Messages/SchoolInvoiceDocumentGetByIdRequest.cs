using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class SchoolInvoiceDocumentGetByIdRequest : Request
    {
        [DataMember]
        public Guid SchoolInvoiceDocumentGuid { get; set; }
    }
}
