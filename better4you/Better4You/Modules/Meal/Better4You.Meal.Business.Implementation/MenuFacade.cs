using System;
using System.Collections.Generic;
using System.Linq;
using Better4You.Core.Repositories;
using Better4You.Meal.Config;
using Better4You.Meal.EntityModel;
using Better4You.Meal.ViewModel;
using Better4You.UserManagment.EntityModel;
using Tar.Core.Exceptions;

namespace Better4You.Meal.Business.Implementation
{
    public class MenuFacade:IMenuFacade
    {
        public IConfigRepository Repository { get; set; }
        
        public List<MenuListItemView> GetByFilter(MenuFilterView filter, int pageSize, int pageIndex, string orderByField, bool orderByAsc, out int totalCount)
        {
            var query = Repository.Query<Menu>();
            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(d => d.Name.Contains(filter.Name));

            if (filter.MealTypeId > 0 && filter.MenuTypeId == 0)
            {
                var mealType = (MealTypes)Enum.Parse(typeof (MealTypes), filter.MealTypeId.ToString("N0"));
                var menuTypeIdList = Lookups.MealMenuTypeList[mealType].Select(d => long.Parse(d.ToString("D"))).ToList();
                query = query.Where(d => menuTypeIdList.Contains(d.MenuType));
            }
            else if (filter.MenuTypeId > 0)
                query = query.Where(d => d.MenuType == filter.MenuTypeId);
            if (filter.RecordStatusId > 0)
                query = query.Where(d => d.RecordStatus == filter.RecordStatusId);

            totalCount = query.Count();

            switch (orderByField)
            {
                case "Id":
                    query = orderByAsc ? query.OrderBy(c => c.Id) : query.OrderByDescending(c => c.Id);
                    break;
                case "Name":
                    query = orderByAsc ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name);
                    break;
                case "MenuType":
                    query = orderByAsc ? query.OrderBy(c => c.MenuType) : query.OrderByDescending(c => c.MenuType);
                    break;
                case "ModifiedAt":
                    query = orderByAsc ? query.OrderBy(c => c.ModifiedAt) : query.OrderByDescending(c => c.ModifiedAt);
                    break;
                default:
                    query = orderByAsc ? query.OrderBy(c => new { c.Name }) : query.OrderByDescending(c => new { c.Name });
                    break;
            }

            if (pageSize > 0) query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var menuList = query.AsEnumerable().Select(d => d.ToView<MenuListItemView>()).ToList();

            return menuList;
        }

        public MenuView Get(long id)
        {

            var view = Repository.GetById<Menu>(id).ToView<MenuView>();

            return view;
        }

        public MenuView Create(MenuView view)
        {
            return Update(view);
        }

        public MenuView Update(MenuView view)
        {
            var dtNow = DateTime.Now;
            var model = view.Id > 0
                ? Repository.GetById<Menu>(view.Id)
                : new Menu
                  {
                      ModifiedAt = dtNow,
                      ModifiedReason = view.ModifiedReason,
                      ModifiedBy = view.ModifiedBy,
                      ModifiedByFullName = view.ModifiedByFullName,
                      RecordStatus = (long)RecordStatuses.Active

                  };

            model.Name = view.Name;
            model.MenuType = view.MenuType.Id;
            model.SchoolType = view.SchoolType.Id;
            model.AdditionalFruit = view.AdditionalFruit;
            model.AdditionalVeg = view.AdditionalVeg;

            if(model.Id==0)
            {
                model.CreatedAt = dtNow;
                model.CreatedBy = view.ModifiedBy;
                model.CreatedByFullName = view.ModifiedByFullName;
            }
            
            var menuFoods = model.Foods.ToList();
            var menuFoodIds = menuFoods.Select(d => d.Id).ToList();
            var viewFoodIds = view.Foods.Select(d => d.Id).Distinct().ToList();
            menuFoods.Where(d => !viewFoodIds.Contains(d.Id)).ToList().ForEach(d => model.Foods.Remove(d));
            view.Foods.Where(d => !menuFoodIds.Contains(d.Id))
                .ToList()
                .ForEach(d => model.AddFood(Repository.GetById<Food>(d.Id)));

            var menuSchools = model.Schools.ToList();
            if (model.SchoolType == (int) MenuSchoolTypes.None)
            {
                var menuSchoolIds = menuSchools.Select(d => d.Id).ToList();
                var viewSchoolIds = view.Schools.Select(d => Convert.ToInt64(d.Id)).Distinct().ToList();
                menuSchools.Where(d => !viewSchoolIds.Contains(d.Id)).ToList().ForEach(d => model.Schools.Remove(d));
                view.Schools.Where(d => !menuSchoolIds.Contains(Convert.ToInt64(d.Id)))
                    .ToList()
                    .ForEach(d => model.AddSchool(Repository.GetById<School>(Convert.ToInt64(d.Id))));
            }
            else
            {
                menuSchools.ForEach(d => model.Schools.Remove(d));
            }
            Repository.Update(model);
            return model.ToView<MenuView>();
        }

        public void Delete(MenuView view)
        {

            var isMenuOrdered = Repository.Query<MealMenuOrderItem>().Any(
                d => //d.MealMenuOrder.OrderStatus == (int) OrderStatuses.InitialState &&
                     d.MealMenu.Menu.Id == view.Id &&
                     d.RecordStatus == (int)RecordStatuses.Active);
            if(isMenuOrdered)
                throw new WarningException("Couldn't remove active ordered menu");
            var model = Repository.GetById<Menu>(view.Id);
            model.ModifiedAt = DateTime.Now;
            model.ModifiedBy = view.ModifiedBy;
            model.ModifiedByFullName = view.ModifiedByFullName;
            model.ModifiedReason = view.ModifiedReason;
            model.RecordStatus = (int)RecordStatuses.Deleted ;

        }
        
    }
}
