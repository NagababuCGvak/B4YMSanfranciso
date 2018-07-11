using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class MenuListItemView : IView
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public GeneralItemView MenuType { get; set; }

        [DataMember]
        public GeneralItemView SchoolType { get; set; }

        [DataMember]
        public bool AdditionalFruit { get; set; }

        [DataMember]
        public bool AdditionalVeg { get; set; }

        [DataMember]
        public GeneralItemView RecordStatus { get; set; }

    }
}
