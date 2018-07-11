using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class MenuView : IView
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public GeneralItemView MenuType { get; set; }

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
        public GeneralItemView SchoolType { get; set; }

        [DataMember]
        public bool AdditionalFruit { get; set; }

        [DataMember]
        public bool AdditionalVeg { get; set; }

        [DataMember]
        public GeneralItemView RecordStatus { get; set; } 

        [DataMember]
        public List<FoodListItemView> Foods { get; set; }

        [DataMember]
        public List<GeneralItemView> Schools { get; set; }

        public MenuView()
        {
            Foods = new List<FoodListItemView>();
            Schools= new List<GeneralItemView>();
        }
    }
}
