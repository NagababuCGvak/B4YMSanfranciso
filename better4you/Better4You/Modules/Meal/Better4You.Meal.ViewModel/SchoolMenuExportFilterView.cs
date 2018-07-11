using System;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class SchoolMenuExportFilterView : IView
    {
        [DataMember]
        public DateTime OrderDate { get; set; }
        [DataMember]
        public int MealTypeId { get; set; }
        [DataMember]
        public int SchoolId { get; set; }
        [DataMember]
        public int SchoolType { get; set; }
    }
}