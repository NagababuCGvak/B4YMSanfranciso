using AutoMapper;
using Better4You.Meal.EntityModel;
using Better4You.Meal.ViewModel;

namespace Better4You.Meal.Business
{
    public static class MappingExtension
    {

        #region Food List Item View
        public static FoodListItemView ToView(this Food model)
        {
            return Mapper.Map<FoodListItemView>(model);
        }
        public static Food ToModel(this FoodListItemView view)
        {
            return Mapper.Map<Food>(view);
        }
        public static void SetTo(this FoodListItemView view, Food model)
        {
            Mapper.Map(view, model);
        }
        #endregion Food List Item View

        #region Food View
        public static T ToView<T>(this Food model)
        {
            return Mapper.Map<T>(model);
        }
        public static Food ToModel(this FoodView view)
        {
            return Mapper.Map<Food>(view);
        }
        public static void SetTo(this FoodView view, Food model)
        {
            Mapper.Map(view, model);
        }
        #endregion Food View

        #region Food Ingredient View
        public static FoodIngredientView ToView(this FoodIngredient model)
        {
            return Mapper.Map<FoodIngredientView>(model);
        }
        public static FoodIngredient ToModel(this FoodIngredientView view)
        {
            return Mapper.Map<FoodIngredient>(view);
        }
        public static void SetTo(this FoodIngredientView view, FoodIngredient model)
        {
            Mapper.Map(view, model);
        }
        #endregion Food Ingredient View


        #region Food Percentage View
        public static FoodPercentageView ToView(this FoodPercentage model)
        {
            return Mapper.Map<FoodPercentageView>(model);
        }
        public static FoodPercentage ToModel(this FoodPercentageView view)
        {
            return Mapper.Map<FoodPercentage>(view);
        }
        public static void SetTo(this FoodPercentageView view, FoodPercentage model)
        {
            Mapper.Map(view, model);
        }
        #endregion Food Percentage View

        #region Menu View
        public static T ToView<T>(this Menu model)
        {
            return Mapper.Map<T>(model);
        }
        public static Menu ToModel(this MenuView view)
        {
            return Mapper.Map<Menu>(view);
        }
        public static void SetTo(this MenuView view, Menu model)
        {
            Mapper.Map(view, model);
        }
        public static Menu ToModel(this MenuListItemView view)
        {
            return Mapper.Map<Menu>(view);
        }
        public static void SetTo(this MenuListItemView view, Menu model)
        {
            Mapper.Map(view, model);
        }
        #endregion Menu View

        #region Meal Menu View
        public static T ToView<T>(this MealMenu model)
        {
            return Mapper.Map<T>(model);
        }
        public static MealMenu ToModel(this MealMenuView view)
        {
            return Mapper.Map<MealMenu>(view);
        }
        public static void SetTo(this MealMenuView view, MealMenu model)
        {
            Mapper.Map(view, model);
        }
        public static MealMenu ToModel(this MealMenuListItemView view)
        {
            return Mapper.Map<MealMenu>(view);
        }
        public static void SetTo(this MealMenuListItemView view, MealMenu model)
        {
            Mapper.Map(view, model);
        }
        #endregion Meal Menu View

        #region Meal Menu Order View
        public static T ToView<T>(this MealMenuOrder model)
        {
            return Mapper.Map<T>(model);
        }
        public static MealMenuOrder ToModel(this MealMenuOrderView view)
        {
            return Mapper.Map<MealMenuOrder>(view);
        }
        public static void SetTo(this MealMenuOrderView view, MealMenuOrder model)
        {
            Mapper.Map(view, model);
        }
        /*
        public static MealMenuOrder ToModel(this MealMenuOrderListItemView view)
        {
            return Mapper.Map<MealMenuOrder>(view);
        }
        public static void SetTo(this MealMenuOrderListItemView view, MealMenuOrder model)
        {
            Mapper.Map(view, model);
        }
       */
        #endregion Meal Menu Order View

        #region Meal Menu Order Item View
        public static T ToView<T>(this MealMenuOrderItem model)
        {
            return Mapper.Map<T>(model);
        }
        public static MealMenuOrderItem ToModel(this MealMenuOrderItemView view)
        {
            return Mapper.Map<MealMenuOrderItem>(view);
        }
        public static void SetTo(this MealMenuOrderItemView view, MealMenuOrderItem model)
        {
            Mapper.Map(view, model);
        }
        /*
        public static MealMenuOrderItem ToModel(this MealMenuOrderItemListItemView view)
        {
            return Mapper.Map<MealMenuOrderItem>(view);
        }
        public static void SetTo(this MealMenuOrderItemListItemView view, MealMenuOrderItem model)
        {
            Mapper.Map(view, model);
        }
        */
        #endregion Meal Menu Order Item View


        public static T ToView<T>(this SchoolInvoiceDocument model)
        {
            return Mapper.Map<T>(model);

        }
        public static SchoolInvoiceDocument ToModel(this SchoolInvoiceDocumentView view)
        {
            return Mapper.Map<SchoolInvoiceDocument>(view);
        }
    }
}
