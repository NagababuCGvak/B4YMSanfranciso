using System.Collections.Generic;
using Better4You.Meal.Config;
using Tar.Core.Extensions;

namespace Better4You.Meal.Business
{
    public static class DeliveryTypeExtensions
    {
        private static readonly Dictionary<MealTypes, DeliveryTypes> DeliveryTypeOfMealTypes = new Dictionary
            <MealTypes, DeliveryTypes>
        {
            {MealTypes.None, DeliveryTypes.None},
            {MealTypes.Breakfast, DeliveryTypes.Breakfast},
            {MealTypes.Snack, DeliveryTypes.Breakfast},
            {MealTypes.Lunch, DeliveryTypes.Lunch},
            {MealTypes.SackLunch, DeliveryTypes.Lunch},
            {MealTypes.Supper, DeliveryTypes.Breakfast}
        };

        public static long GetDefaultDeliveryTypeOfMealType(this long mealType)
        {
            return GetDefaultDeliveryTypeOfMealType((MealTypes) mealType).ToInt64();
        }

        public static DeliveryTypes GetDefaultDeliveryTypeOfMealType(this MealTypes mealType)
        {
            return DeliveryTypeOfMealTypes[mealType];
        }
    }
}
