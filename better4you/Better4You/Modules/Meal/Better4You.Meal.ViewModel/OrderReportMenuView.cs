using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class OrderReportMenuView : IView
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public long MenuTypeId { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
        [DataMember]
        public double? Rate { get; set; }
        [DataMember]
        public int? AdjusmentCount { get; set; }
        [DataMember]
        public long? RefId { get; set; }
        [DataMember]
        public string ModifiedReason { get; set; }
        [DataMember]
        public GeneralItemView MealServiceType { get; set; }
        [DataMember]
        public int? fruitCount { get; set; }
        [DataMember]
        public int? vegetableCount { get; set; }
    }
}