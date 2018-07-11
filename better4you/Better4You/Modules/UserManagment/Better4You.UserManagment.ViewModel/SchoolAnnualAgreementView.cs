
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.UserManagment.ViewModel
{
    [DataContract]
    public class SchoolAnnualAgreementView
    {
        [DataMember]
        public long Id { get; set; }
        
        [DataMember]
        public int Year { get; set; }
        
        [DataMember]
        public double Price { get; set; }
        
        [DataMember]
        public GeneralItemView RecordStatus { get; set; }

        [DataMember]
        public long SchoolId { get; set; }

        /*
        [DataMember]
        public GeneralItemView MealType { get; set; }
        */
        [DataMember]
        public GeneralItemView ItemType { get; set; }

    }
}