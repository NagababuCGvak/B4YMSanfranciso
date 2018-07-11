using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class MealMenuDeleteRequest:Request
    {
        [DataMember]
        public MealMenuView MealMenu { get; set; }
    }
}