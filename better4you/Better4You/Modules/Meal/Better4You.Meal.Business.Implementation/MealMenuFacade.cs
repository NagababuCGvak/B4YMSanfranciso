using System;
using System.Collections.Generic;
using System.Linq;
using Better4You.Core.Repositories;
using Better4You.Meal.Config;
using Better4You.Meal.EntityModel;
using Better4You.Meal.ViewModel;

namespace Better4You.Meal.Business.Implementation
{
    public class MealMenuFacade:IMealMenuFacade
    {
        public IConfigRepository Repository { get; set; }

        public List<MealMenuListItemView> GetByFilter(MealMenuFilterView filter, int pageSize, int pageIndex, string orderByField, bool orderByAsc, out int totalCount)
        {

            var query = Repository.Query<MealMenu>();
            if (filter.MealTypeId > 0)
                query = query.Where(d => d.MealType == filter.MealTypeId);
            if (filter.RecordStatusId > 0)
                query = query.Where(d => d.RecordStatus == filter.RecordStatusId);
            if (filter.StartDate.HasValue)
                query = query.Where(d => d.ValidDate >= filter.StartDate);
            if (filter.EndDate.HasValue)
                query = query.Where(d => d.ValidDate <= filter.EndDate);
            if (filter.MenuId.HasValue)
                query = query.Where(d => new[]{filter.MenuId.Value}.Contains(d.Menu.Id));


            totalCount = query.Count();

            switch (orderByField)
            {
                case "Id":
                    query = orderByAsc ? query.OrderBy(c => c.Id) : query.OrderByDescending(c => c.Id);
                    break;
                case "MealType":
                    query = orderByAsc ? query.OrderBy(c => c.MealType) : query.OrderByDescending(c => c.MealType);
                    break;
                case "ValidDate":
                    query = orderByAsc ? query.OrderBy(c => c.ValidDate) : query.OrderByDescending(c => c.ValidDate);
                    break;
                case "ModifiedAt":
                    query = orderByAsc ? query.OrderBy(c => c.ModifiedAt) : query.OrderByDescending(c => c.ModifiedAt);
                    break;
                default:
                    query = orderByAsc ? query.OrderBy(c => new { c.Id }) : query.OrderByDescending(c => new { c.Id });
                    break;
            }

            if (pageSize > 0) query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var mealMenuList = query.AsEnumerable().Select(d => d.ToView<MealMenuListItemView>()).ToList();

            return mealMenuList;
        }

        public MealMenuView Get(long id)
        {
            return Repository.GetById<MealMenu>(id).ToView<MealMenuView>();
        }

        public MealMenuView Create(MealMenuView view)
        {
            return Update(view);
        }

        public MealMenuView Update(MealMenuView view)
        {
            var dtNow = DateTime.Now;
            MealMenu model;
            MealMenu duplicatedMealMenu = null;

            if(view.Id>0)
            {
                model = Repository.GetById<MealMenu>(view.Id);
            }
            else
            {
                duplicatedMealMenu = (from mealMenu in Repository.Query<MealMenu>()
                                        where
                                        mealMenu.MealType == view.MealType.Id
                                        && mealMenu.ValidDate == view.ValidDate.Date
                                        && mealMenu.Menu.Id == view.Menu.Id
                                        && mealMenu.RecordStatus == (long)RecordStatuses.Active
                                      select mealMenu).FirstOrDefault();
                if (duplicatedMealMenu == null)
                {
                    model = new MealMenu
                    {
                        Menu = Repository.GetById<Menu>(view.Menu.Id),
                            ModifiedAt = dtNow,
                            ModifiedReason = view.ModifiedReason,
                            ModifiedBy = view.ModifiedBy,
                            ModifiedByFullName = view.ModifiedByFullName,
                            RecordStatus = (long)RecordStatuses.Active 
                    };
                }
                else
                {
                    model = duplicatedMealMenu;
                }
            }
            if (duplicatedMealMenu == null)
            {
                model.MealType = (int)view.MealType.Id;
                model.ValidDate = view.ValidDate.Date;
                if (model.Id == 0)
                {
                    model.CreatedAt = dtNow;
                    model.CreatedBy = view.ModifiedBy;
                    model.CreatedByFullName = view.ModifiedByFullName;
                }

                Repository.Update(model);
            }
            return model.ToView<MealMenuView>();
            
        }

        public void Delete(MealMenuView view)
        {
            
            var model = Repository.GetById<MealMenu>(view.Id);
            model.ModifiedAt = DateTime.Now;
            model.ModifiedBy = view.ModifiedBy;
            model.ModifiedByFullName = view.ModifiedByFullName;
            model.ModifiedReason = view.ModifiedReason;
            model.RecordStatus = (long)RecordStatuses.Deleted;
            Repository.Update(model);
            
        }
    }
}
