using System.Runtime.Serialization;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class MealMenuOrderGetRequest:Request
    {
        [DataMember]
        public long Id { get; set; }
    }
}