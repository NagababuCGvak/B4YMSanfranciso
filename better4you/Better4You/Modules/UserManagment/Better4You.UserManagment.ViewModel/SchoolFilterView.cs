using System.Runtime.Serialization;

namespace Better4You.UserManagment.ViewModel
{
    [DataContract]
    public class SchoolFilterView
    {

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Route { get; set; }
        [DataMember]
        public string District { get; set; }
        [DataMember]
        public long StateId { get; set; }
        [DataMember]
        public long CityId { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public long SchoolTypeId { get; set; }

        [DataMember]
        public long RecordStatusId { get; set; }

    }
}
