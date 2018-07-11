using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Better4You.UserManagment.ViewModel
{
    [DataContract]
    public class CommunicationView
    {
        [DataMember]

        public long Id { get; set; }

        [DataMember]

        public string Phone { get; set; }

        [DataMember]

        public string Extension { get; set; }

        [DataMember]

        public string Email { get; set; }

        [DataMember]

        public bool IsPrimary { get; set; }

        [DataMember]

        public string Description { get; set; }

        [DataMember]
        public KeyValuePair<long, string> CommunicationType { get; set; }

        [DataMember]
        public KeyValuePair<long, string> RecordStatus { get; set; }
    }
}