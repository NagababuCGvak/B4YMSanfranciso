using System.Collections.Generic;
using Better4You.Meal.ViewModel;

namespace Better4You.Meal.Business
{
    public interface IMealMenuFacade
    {
        List<MealMenuListItemView> GetByFilter(MealMenuFilterView filter, int pageSize,
                                   int pageIndex, string orderByField,
                                   bool orderByAsc, out int totalCount);
        MealMenuView Get(long id);
        MealMenuView Create(MealMenuView view);
        MealMenuView Update(MealMenuView view);
        void Delete(MealMenuView view);
    }
}
