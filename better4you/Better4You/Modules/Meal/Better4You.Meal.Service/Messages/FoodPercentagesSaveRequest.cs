using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class FoodPercentagesSaveRequest : Request
    {
        [DataMember]
        public List<FoodPercentageView> PercentageList { get; set; }

        public FoodPercentagesSaveRequest()
        {
            PercentageList = new List<FoodPercentageView>();
        }
    }
}