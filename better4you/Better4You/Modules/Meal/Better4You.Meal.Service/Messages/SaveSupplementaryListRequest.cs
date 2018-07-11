using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class SaveSupplementaryListRequest : Request
    {
        [DataMember]
        public List<MealMenuSupplementaryView> List { get; set; }
    }
}