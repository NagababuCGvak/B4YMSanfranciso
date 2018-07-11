using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class MealMenuOrderItemView : IView
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public int? TotalCount { get; set; }

        [DataMember]
        public double? Rate { get; set; }

        [DataMember]
        public int? AdjusmentCount { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public long? RefId { get; set; }

        [DataMember]
        public string ModifiedBy { get; set; }

        [DataMember]
        public string ModifiedByFullName { get; set; }

        [DataMember]
        public DateTime ModifiedAt { get; set; }
        
        [DataMember]
        public string ModifiedReason { get; set; }

        [DataMember]
        public GeneralItemView RecordStatus { get; set; }

        [DataMember]
        public long MealMenuId { get; set; }

        [DataMember]
        public DateTime MealMenuValidDate { get; set; }

        [DataMember]
        public GeneralItemView MealType { get; set; }

        [DataMember]
        public long MenuId { get; set; }

        [DataMember]
        public GeneralItemView MenuType { get; set; }

        [DataMember]
        public string MenuName { get; set; }
        
        [DataMember]
        public IList<FoodListItemView> Foods { get; set; }

        [DataMember]
        public GeneralItemView MealServiceType { get; set; }
        
        [DataMember]
        public GeneralItemView DeliveryType { get; set; }

        [DataMember]
        public bool HasAdditionalFruit { get; set; }

        [DataMember]
        public bool HasAdditionalVegetable { get; set; }

        [DataMember]
        public int FruitCount { get; set; }
        
        [DataMember]
        public int VegetableCount { get; set; }

        [DataMember]
        public long MealMenuOrderId { get; set; }



        public MealMenuOrderItemView()
        {
            Foods = new List<FoodListItemView>();
        }
    }
}
