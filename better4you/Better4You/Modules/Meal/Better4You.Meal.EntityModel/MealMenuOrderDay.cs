using System;
using Better4You.Core.Repositories;

namespace Better4You.Meal.EntityModel
{
    public class MealMenuOrderDay : IConfigEntity
    {
        public virtual long Id { get; set; }
        public virtual MealMenuOrder MealMenuOrder { get; set; }
        public virtual DateTime ValidDate { get; set; }
        public virtual long DeliveryType { get; set; }
        public virtual int? FruitCount { get; set; }
        public virtual int? VegetableCount { get; set; }
    }
}