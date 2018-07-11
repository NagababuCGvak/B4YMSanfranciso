using System.Collections.Generic;
using Better4You.Meal.ViewModel;

namespace Better4You.Meal.Business
{
    public interface IMenuFacade
    {

        List<MenuListItemView> GetByFilter(MenuFilterView filter, int pageSize,
                                   int pageIndex, string orderByField,
                                   bool orderByAsc, out int totalCount);
        
        MenuView Get(long id);
        MenuView Create(MenuView view);
        MenuView Update(MenuView view);
        void Delete(MenuView view);
    }
}
