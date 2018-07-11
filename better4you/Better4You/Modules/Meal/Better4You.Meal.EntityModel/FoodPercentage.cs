using Better4You.Core.Repositories;

namespace Better4You.Meal.EntityModel
{
    public class FoodPercentage : IConfigEntity
    {
        public virtual long Id { get; set; }

        public virtual long SchoolId { get; set; }

        public virtual long MealType { get; set; }
        
        public virtual int Fruit { get; set; }
        
        public virtual int Vegetable { get; set; }
    }
}