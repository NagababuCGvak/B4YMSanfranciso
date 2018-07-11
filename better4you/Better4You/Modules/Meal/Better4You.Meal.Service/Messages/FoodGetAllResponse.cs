using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class FoodGetAllResponse: PageableResponse
    {
        [DataMember]
        public List<FoodListItemView> Foods { get; set; }

        public FoodGetAllResponse()
        {
            Foods = new List<FoodListItemView>();
        }
    }
}