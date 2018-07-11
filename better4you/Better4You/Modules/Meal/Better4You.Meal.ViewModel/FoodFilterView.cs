using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class FoodFilterView : IView
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public long FoodTypeId { get; set; }

        [DataMember]
        public string PortionSize { get; set; }

        [DataMember]
        public string DisplayName { get; set; }
    }
}
