using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class FoodIngredientView : IView
    {
        [DataMember]
        public  int Id { get; set;}

        [DataMember]
        public  string Description { get; set; }

        [DataMember]
        public  double? Value { get; set; }
        
        [DataMember]
        public GeneralItemView IngredientType { get; set; }        
    }
}