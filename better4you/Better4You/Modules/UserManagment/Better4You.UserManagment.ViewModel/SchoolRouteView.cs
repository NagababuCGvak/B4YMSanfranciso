
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.UserManagment.ViewModel
{
    [DataContract]
    public class SchoolRouteView
    {
        [DataMember]
        public long Id { get; set; }
        
        [DataMember]
        public string Route { get; set; }

        [DataMember]
        public GeneralItemView MealType { get; set; }

        [DataMember]
        public GeneralItemView RecordStatus { get; set; }

    }
}
