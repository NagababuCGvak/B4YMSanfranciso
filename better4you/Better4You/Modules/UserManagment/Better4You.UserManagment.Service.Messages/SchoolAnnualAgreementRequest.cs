using System.Runtime.Serialization;
using Better4You.UserManagment.ViewModel;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    public class SchoolAnnualAgreementRequest:Request
    {
        [DataMember]
        public long SchoolId { get; set; }

        [DataMember]
        public SchoolAnnualAgreementView SchoolAnnualAgreement { get; set; }
    }
}