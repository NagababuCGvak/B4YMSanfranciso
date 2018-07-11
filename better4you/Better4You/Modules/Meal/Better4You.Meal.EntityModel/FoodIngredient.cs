using Better4You.Core.Repositories;
using Tar.Model;

namespace Better4You.Meal.EntityModel
{
    public class FoodIngredient : IConfigEntity
    {
        public virtual long Id { get; set; }
        public virtual string Description { get; set; } 
        public virtual double? Value { get; set; }
        public virtual Food Food { get; set; }
        public virtual int IngredientType { get; set; }
        
    }
}