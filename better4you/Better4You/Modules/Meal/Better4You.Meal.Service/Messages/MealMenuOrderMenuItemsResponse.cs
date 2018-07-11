using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class MealMenuOrderMenuItemsResponse : Response
    {
        [DataMember]
        public List<MealMenuOrderMenuView> Menus { get; set; }

        public MealMenuOrderMenuItemsResponse()
        {
            Menus=new List<MealMenuOrderMenuView>();
        }


    }
}