using System;
using System.Runtime.Serialization;

namespace Better4You.UserManagment.ViewModel
{
    [DataContract]
    public class UserViewModelFilter
    {
        [DataMember]
        public long UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public long UserTypeId { get; set; }

        [DataMember]
        public bool IsLocked { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string ActivitationCode { get; set; }

        [DataMember]
        public DateTime ExpireActivationDate { get; set; }
    }
}