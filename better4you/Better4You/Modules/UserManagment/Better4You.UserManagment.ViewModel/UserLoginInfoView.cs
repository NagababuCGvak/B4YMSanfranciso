using System;
using System.Runtime.Serialization;

namespace Better4You.UserManagment.ViewModel
{
    [DataContract]
    public class UserLoginInfoView
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public bool IsOnline { get; set; }

        [DataMember]
        public bool IsApprove { get; set; }

        [DataMember]
        public bool IsLocked { get; set; }

        [DataMember]
        public DateTime? LastLoginDate { get; set; }

        [DataMember]
        public int? PasswordAttemptCount { get; set; }

        [DataMember]
        public DateTime? LastPasswordAttemptDate { get; set; }

        [DataMember]
        public string ActivationCode { get; set; }

        [DataMember]
        public DateTime? ExpireActivationDate { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
