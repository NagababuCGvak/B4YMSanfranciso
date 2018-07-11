using System.Collections.Generic;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class MealMenuOrderMenuView : IView
    {
        [DataMember]
        public long MenuId { get; set; }

        [DataMember]
        public GeneralItemView MenuType { get; set; }

        [DataMember]
        public string MenuName { get; set; }
        
        [DataMember]
        public IList<FoodListItemView> Foods { get; set; }

        public MealMenuOrderMenuView()
        {
            Foods = new List<FoodListItemView>();
        }
    }
}
