using System;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class MealMenuListItemView : IView
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public DateTime ValidDate { get; set; }

        [DataMember]
        public MenuView Menu { get; set; }

        [DataMember]
        public GeneralItemView MealType { get; set; }

        [DataMember]
        public GeneralItemView RecordStatus { get; set; }

        public MealMenuListItemView()
        {
            Menu = new MenuView();
        }
    }
}
