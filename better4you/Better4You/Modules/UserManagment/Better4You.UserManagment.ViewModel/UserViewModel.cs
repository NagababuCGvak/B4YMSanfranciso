using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Better4You.UserManagment.ViewModel
{
    [DataContract]
    public class UserViewModel
    {
        [DataMember]
        public long UserId { get; set; }

        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Please check the username")]
        [DataMember]
        public string UserName { get; set; }

        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Please check the firstname")]
        [DataMember]
        public string FirstName { get; set; }

        [RegularExpression(@"^[^\s]+$", ErrorMessage = "Please check the lastname")]
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
