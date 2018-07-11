using System.Collections.Generic;
using Better4You.Meal.ViewModel;

namespace Better4You.Meal.Business
{
    public interface IFoodFacade
    {

        List<FoodListItemView> GetByFilter(FoodFilterView filter, int pageSize,
                                           int pageIndex, string orderByField,
                                           bool orderByAsc, out int totalCount);

        FoodView Get(long id);
        FoodView Create(FoodView view);
        FoodView Update(FoodView view);
        void Delete(FoodView view);

        List<FoodPercentageView> GetFoodPercentages(long schoolId);
        List<FoodPercentageView> SaveFoodPercentages(List<FoodPercentageView> percentageList);

    }
}
