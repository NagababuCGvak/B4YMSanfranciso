using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class MealMenuOrderItemGetHistoryResponse : Response
    {
        [DataMember]
        public List<MealMenuOrderItemHistoricalView> OrderItems { get; set; }

        public MealMenuOrderItemGetHistoryResponse()
        {
            OrderItems = new List<MealMenuOrderItemHistoricalView>();
        }
    }
}