using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class MealMenuOrderItemSaveRequest:Request
    {
        [DataMember]
        public long SchoolId { get; set; }
        [DataMember]
        public long MealTypeId { get; set; }

        [DataMember]
        public MealMenuOrderItemView OrderItem { get; set; }
    }
}