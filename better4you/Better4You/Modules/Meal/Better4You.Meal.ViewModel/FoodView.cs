using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class FoodView : IView
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
        public DateTime CreatedAt { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }

        [DataMember]
        public string CreatedByFullName { get; set; }

        [DataMember]
        public DateTime ModifiedAt { get; set; }

        [DataMember]
        public string ModifiedBy { get; set; }

        [DataMember]
        public string ModifiedByFullName { get; set; }

        [DataMember]
        public string ModifiedReason { get; set; }

        [DataMember]
        public GeneralItemView RecordStatus { get; set; } 

        
        [DataMember]
        public GeneralItemView FoodType { get; set; }

        [DataMember]
        public IList<FoodIngredientView> FoodIngredients { get; set; }

        public FoodView()
        {
            FoodIngredients= new List<FoodIngredientView>();
        }
    }
}
