using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class SchoolInvoiceDocumentGetByIdResponse : Response
    {
        [DataMember]
        public SchoolInvoiceDocumentView SchoolInvoiceDocument { get; set; }
    }
}