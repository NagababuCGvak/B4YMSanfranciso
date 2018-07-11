using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Better4You.UserManagment.ViewModel
{
    [DataContract]
    public class UserItemView
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }
    }
}
