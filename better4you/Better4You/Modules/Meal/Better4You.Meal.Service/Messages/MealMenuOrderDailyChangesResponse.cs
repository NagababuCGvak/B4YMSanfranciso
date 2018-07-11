using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class MealMenuOrderDailyChangesResponse : PageableResponse
    {
        [DataMember]
        public List<DailyChangesItemView> OrderItems { get; set; }

        public MealMenuOrderDailyChangesResponse()
        {
            OrderItems = new List<DailyChangesItemView>();
        }
        
    }
}