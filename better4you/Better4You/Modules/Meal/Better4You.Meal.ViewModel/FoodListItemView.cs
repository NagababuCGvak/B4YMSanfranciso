
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class FoodListItemView : IView
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string PortionSize { get; set; }

        [DataMember]
        public string DisplayName { get; set; }
        
        [DataMember]
        public GeneralItemView FoodType { get; set; }

        [DataMember]
        public GeneralItemView RecordStatus { get; set; }
    }
}
