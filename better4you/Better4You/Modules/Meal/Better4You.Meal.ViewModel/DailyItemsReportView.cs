using System;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class DailyItemsReportView : IView
    {
        [DataMember]
        public string SchoolCode { get; set; }

        [DataMember]
        public string SchoolName { get; set; }

        [DataMember]
        public GeneralItemView MealType { get; set; }

        [DataMember]
        public int? FruitCount { get; set; }

        [DataMember]
        public int? VegetableCount { get; set; }

        [DataMember]
        public DateTime OrderDay { get; set; }
    }
}