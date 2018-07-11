using System.Runtime.Serialization;

namespace Better4You.UserManagment.ViewModel
{
    [DataContract]
    public class SchoolLookupView
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Code { get; set; }
        //[DataMember]
        //public string Route { get; set; }
      
    }
}
