using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class MealMenuSaveResponse:Response
    {
        [DataMember]
        public List<MealMenuView> MealMenus { get; set; }

        public MealMenuSaveResponse()
        {
            MealMenus = new List<MealMenuView>();
        }
    }
}