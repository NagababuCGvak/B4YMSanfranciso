using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    public class FoodPercentageView : IView
    {
        public long Id { get; set; }

        public long SchoolId { get; set; }

        public GeneralItemView MealType { get; set; }
        
        public int Fruit { get; set; }
        
        public int Vegetable { get; set; }
    }
}