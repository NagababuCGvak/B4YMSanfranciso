using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Better4You.UserManagment.ViewModel
{
    [DataContract]
    public class AddressView
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string District { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public KeyValuePair<long, string> AddressType { get; set; }
        [DataMember]
        public KeyValuePair<long, string> City { get; set; }
        [DataMember]
        public KeyValuePair<long, string> FirstAdminDivision { get; set; }
        [DataMember]
        public KeyValuePair<long, string> Country { get; set; }
        [DataMember]
        public KeyValuePair<long, string> RecordStatus { get; set; }
    }
}
