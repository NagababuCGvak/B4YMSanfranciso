using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class MealMenuGetResponse:Response
    {
        [DataMember]
        public MealMenuView MealMenu { get; set; }
    }
}