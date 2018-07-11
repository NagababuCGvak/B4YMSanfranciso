using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Better4You.UserManagment.ViewModel
{
    [DataContract]
    public class ContactInfoView
    {
        [DataMember]
        public KeyValuePair<long, string> School { get; set; }

        [DataMember]
        public KeyValuePair<long, string> ContactRegard { get; set; }

        [DataMember]
        public long UserId { get; set; }
        
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Message { get; set; }

    }
}
