using System;
using System.Runtime.Serialization;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class MealOrderManageDayItemView 
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long MealMenuId { get; set; }
        [DataMember]
        public long MenuId { get; set; }
        [DataMember]
        public string MenuName { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public string ModifiedReason { get; set; }
        [DataMember]
        public string ModifiedBy { get; set; }
        [DataMember]
        public string ModifiedByFullName { get; set; }
        [DataMember]
        public DateTime ModifiedAt { get; set; }
        [DataMember]
        public long MealServiceTypeId { get; set; }
        [DataMember]
        public long MealTypeId { get; set; }
        [DataMember]
        public bool IsSoyMilk { get; set; }
        [DataMember]
        public bool IsMilk { get; set; }

        [DataMember]
        public bool HasAdditionalFruit { get; set; }

        [DataMember]
        public bool HasAdditionalVegetable { get; set; }
    }
}