using System.Runtime.Serialization;

namespace Better4You.UserManagment.ViewModel
{
    [DataContract]
    public class ApplicationView
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Name { get; set; }      
    }
}
