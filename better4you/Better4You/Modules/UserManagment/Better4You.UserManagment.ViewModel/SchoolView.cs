using System.Collections.Generic;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.UserManagment.ViewModel
{
    [DataContract]
    public class SchoolView:IView
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public IList<UserItemView> Users { get; set; }
        
        [DataMember]
        public IList<SchoolAnnualAgreementView> SchoolAnnualAgreements { get; set; }
        
        [DataMember]
        public IList<SchoolRouteView> SchoolRoutes { get; set; }

        [DataMember]
        public string Address1 { get; set; }

        [DataMember]
        public string Address2 { get; set; }


        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public string BusinessPhone { get; set; }

        [DataMember]
        public string CellPhone { get; set; }
        
        [DataMember]
        public string Fax { get; set; }
        
        [DataMember]
        public string Email { get; set; }
        
        [DataMember]
        public string Notes { get; set; }


        [DataMember]
        public long RecordStatus { get; set; }
        
        [DataMember]
        public long SchoolType { get; set; }
        
        [DataMember]
        public long CityId { get; set; }
        
        [DataMember]
        public string City { get; set; }
        
        [DataMember]
        public long FirstAdminDivisionId { get; set; }
        
        [DataMember]
        public string FirstAdminDivision { get; set; }

        [DataMember]
        public long FoodServiceType { get; set; }

        [DataMember]
        public long OVSType { get;set;}

        public SchoolView()
        {
            Users = new List<UserItemView>();
            SchoolAnnualAgreements = new List<SchoolAnnualAgreementView>();
            SchoolRoutes = new List<SchoolRouteView>();
        }
    }
}
