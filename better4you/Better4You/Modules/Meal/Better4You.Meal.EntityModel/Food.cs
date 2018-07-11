using System;
using System.Collections.Generic;
using Better4You.Core.Repositories;

namespace Better4You.Meal.EntityModel
{
    public class Food : IConfigEntity
    {
        public virtual long Id { get; set; }
        
        public virtual string Name { get; set; }
        
        public virtual string PortionSize { get; set; }
        
        public virtual string DisplayName { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual string CreatedByFullName { get; set; }

        public virtual DateTime ModifiedAt { get; set; }

        public virtual string ModifiedBy { get; set; }

        public virtual string ModifiedByFullName { get; set; }

        public virtual string ModifiedReason { get; set; }

        public virtual long RecordStatus { get; set; }

        public virtual long FoodType { get; set; }

        public virtual IList<Menu> Menus { get; set; }

        public virtual IList<FoodIngredient> FoodIngredients { get; set; }
        public Food()
        {
            FoodIngredients = new List<FoodIngredient>();
            Menus = new List<Menu>();
        }

        public virtual void AddFoodIngredient(FoodIngredient newFoodIngredient)
        {
            newFoodIngredient.Food =this;
            FoodIngredients.Add(newFoodIngredient);
        }
        public virtual void AddMenu(Menu newMenu)
        {
            Menus.Add(newMenu);
        }
    }
}