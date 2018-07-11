using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class DailyChangesItemView : IView
    {
        [DataMember]
        public long SchoolId { get; set; }

        [DataMember]
        public string SchoolName { get; set; }

        [DataMember]
        public string SchoolCode { get; set; }

        [DataMember]
        public string SchoolRoute { get; set; }
        
        [DataMember]
        public string SchoolType { get; set; }

        [DataMember]
        public int NewCount { get; set; }

        [DataMember]
        public int PreviousCount { get; set; }

        [DataMember]
        public long MenuId { get; set; }
        
        [DataMember]
        public string MenuName { get; set; }

        public GeneralItemView MenuType { get; set; }

        //[DataMember]
        //public int MenuTypeId { get; set; }

        //[DataMember]
        //public string MenuTypeName { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public long? OrderItemRefId { get; set; }

        [DataMember]
        public string Notes { get; set; }


    }
}
