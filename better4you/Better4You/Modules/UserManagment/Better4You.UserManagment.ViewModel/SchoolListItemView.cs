using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.UserManagment.ViewModel
{
    [DataContract]
    public class SchoolListItemView:IView
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Code { get; set; }        

        [DataMember]
        public string Address1 { get; set; }

        [DataMember]
        public string Address2 { get; set; }

        [DataMember]
        public  string ZipCode { get; set; }

        [DataMember]
        public string City { get; set; }


        [DataMember]
        public string FirstAdminDivision { get; set; }


        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string BusinessPhone { get; set; }

        [DataMember]
        public string CellPhone { get; set; }

        [DataMember]
        public string HomePhone { get; set; }

        [DataMember]
        public string Fax { get; set; }

        [DataMember]
        public string SchoolType { get; set; }

        [DataMember]
        public string FoodServiceType { get; set; }

        [DataMember]
        public string OVSType { get; set; }
        
        [DataMember]
        public string RecordStatus { get; set; }

    }
}
