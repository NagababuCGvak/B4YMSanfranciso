using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class MealMenuGetAllResponse: PageableResponse
    {
        [DataMember]
        public List<MealMenuListItemView> MealMenus { get; set; }

        public MealMenuGetAllResponse()
        {
            MealMenus = new List<MealMenuListItemView>();
        }
    }
}