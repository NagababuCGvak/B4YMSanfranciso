using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.UserManagment.ViewModel;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    [DataContract]
    public class AllAnnualAgreementsGetResponse : Response
    {
        [DataMember]
        public List<SchoolAnnualAgreementView> SchoolAnnualAgreements { get; set; }
    }
}