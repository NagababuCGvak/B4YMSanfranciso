using System.Runtime.Serialization;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class MealMenuOrderItemGetHistoryRequest : Request
    {
        [DataMember]
        public long OrderItemId { get; set; }

        [DataMember]
        public long OrderItemRefId { get; set; }
    }
}