using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class FoodCreateRequest:Request
    {
        [DataMember]
        public FoodView Food { get; set; }

        //[DataMember]
        //public IList<KeyValuePair<long, string>> Ingredients { get; set; }

        //public FoodCreateRequest()
        //{
        //    Ingredients= new List<KeyValuePair<long, string>>();
        //}
    }
}