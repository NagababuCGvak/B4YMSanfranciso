using System;

using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class MealMenuView : IView
    {
        [DataMember]
        public long Id { get; set; }
        
        [DataMember]
        public GeneralItemView MealType { get; set; }

        [DataMember]
        public DateTime ValidDate { get; set; }

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
        public MenuView Menu { get; set; }

        public MealMenuView()
        {
            Menu = new MenuView();
        }
    }
}
