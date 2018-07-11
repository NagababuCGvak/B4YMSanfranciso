using System.Runtime.Serialization;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class MealMenuOrderSubmitOrderResponse:Response
    {
        [DataMember]
        public bool IsOrderSubmitted{ get; set; }

    }
}