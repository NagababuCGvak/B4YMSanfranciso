using System;
using System.Collections.Generic;
using System.Linq;
using Better4You.Core.Repositories;
using Better4You.Meal.Config;
using Better4You.Meal.EntityModel;
using Better4You.Meal.ViewModel;

namespace Better4You.Meal.Business.Implementation
{
    public class FoodFacade:IFoodFacade
    {
        public IConfigRepository Repository { get; set; }

        public List<FoodListItemView> GetByFilter(FoodFilterView filter, int pageSize, int pageIndex, string orderByField, bool orderByAsc, out int totalCount)
        {
            var query = Repository.Query<Food>().Where(d => d.RecordStatus == (long)RecordStatuses.Active);
            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(d => d.Name.Contains(filter.Name));
            if (filter.FoodTypeId > 0)
                query = query.Where(d => d.FoodType == filter.FoodTypeId);
            if (!string.IsNullOrWhiteSpace(filter.PortionSize))
                query = query.Where(d => d.PortionSize.Contains(filter.PortionSize));
            if (!string.IsNullOrWhiteSpace(filter.DisplayName))
                query = query.Where(d => d.DisplayName.Contains(filter.DisplayName));

            totalCount = query.Count();

            switch (orderByField)
            {
                case "Id":
                    query = orderByAsc ? query.OrderBy(c => c.Id) : query.OrderByDescending(c => c.Id);
                    break;
                case "Name":
                    query = orderByAsc ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name);
                    break;
                case "PortionSize":
                    query = orderByAsc ? query.OrderBy(c => c.PortionSize) : query.OrderByDescending(c => c.PortionSize);
                    break;
                case "DisplayName":
                    query = orderByAsc ? query.OrderBy(c => c.DisplayName) : query.OrderByDescending(c => c.DisplayName);
                    break;
                case "FoodType":
                    query = orderByAsc ? query.OrderBy(c => c.FoodType) : query.OrderByDescending(c => c.FoodType);
                    break;
                default:
                    query = orderByAsc ? query.OrderBy(c => new { c.Name}) : query.OrderByDescending(c => new { c.Name});
                    break;
            
            }

            if (pageSize > 0) query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var foodList = query.AsEnumerable().Select( d => d.ToView()).ToList();

            return foodList;
        }

        public FoodView Get(long id)
        {
            return Repository.GetById<Food>(id).ToView<FoodView>();
        }
        
        public FoodView Create(FoodView view)
        {
            return Update(view);
        }
        
        public FoodView Update(FoodView view)
        {
            var dtNow = DateTime.Now;
            view.FoodIngredients.Where(d=>d.Id==0 && !d.Value.HasValue)
                .ToList().ForEach(d => view.FoodIngredients.Remove(d));

            var model = view.Id > 0
                            ? Repository.GetById<Food>(view.Id)
                            : new Food
                            {
                                    ModifiedAt = dtNow,
                                    ModifiedReason = view.ModifiedReason,
                                    ModifiedBy = view.ModifiedBy,
                                    ModifiedByFullName = view.ModifiedByFullName,
                                    RecordStatus = (long)RecordStatuses.Active 
                            };

            model.Name = view.Name;
            model.FoodType = view.FoodType.Id;
            model.DisplayName = view.DisplayName;
            model.PortionSize = view.PortionSize;
            model.ModifiedBy = view.ModifiedBy;
            model.ModifiedByFullName = view.ModifiedBy;
            model.ModifiedAt = dtNow;
            model.ModifiedReason = view.ModifiedReason;
            if (model.Id == 0)
            {
                model.CreatedAt = dtNow;
                model.CreatedBy = view.ModifiedBy;
                model.CreatedByFullName = view.ModifiedByFullName;
            }

            var modelFoodIngredients = model.FoodIngredients.AsEnumerable();
            view.FoodIngredients
                .ToList()
                .ForEach(d =>
                             {
                                 if (d.Id > 0)
                                 {
                                     d.SetTo(modelFoodIngredients.First(m => m.Id == d.Id));
                                 }
                                 else if (d.Id == 0 && d.Value.HasValue)
                                 {
                                     var foodIngredient = d.ToModel();
                                     foodIngredient.Food = model;
                                     model.FoodIngredients.Add(foodIngredient);
                                 }
                             });
            Repository.Update(model);
            return model.ToView<FoodView>();
        }

        public void Delete(FoodView view)
        {
            var isItemOrdered = Repository.Query<MealMenuOrderItem>().Any(
                d => d.MealMenuOrder.OrderStatus == (long)OrderStatuses.InitialState &&
                     d.MealMenu.Menu.Foods.Any(f=>f.Id==view.Id) &&
                     d.RecordStatus == (long)RecordStatuses.Active);

            if (isItemOrdered)
                throw new Exception("Couldn't remove active ordered item");
            var model = Repository.GetById<Food>(view.Id);
            model.ModifiedAt = DateTime.Now;
            model.ModifiedBy = view.ModifiedBy;
            model.ModifiedByFullName = view.ModifiedByFullName;
            model.ModifiedReason = view.ModifiedReason;
            model.RecordStatus = (long)RecordStatuses.Deleted;
          
        }

        public List<FoodPercentageView> GetFoodPercentages(long schoolId)
        {
            var percentageList =
                Repository.Query<FoodPercentage>()
                    .Where(d => d.SchoolId == schoolId)
                    .AsEnumerable()
                    .Select(d => d.ToView())
                    .ToList();
            Lookups.GetItems<MealTypes>().Where(d=>d.Id!=0).ToList().ForEach(d =>
            {
                if(percentageList.All(p => p.MealType != d))
                    percentageList.Add(new FoodPercentageView{SchoolId = schoolId,MealType = d,Fruit = 100,Vegetable = 100});
            });
            return percentageList;
        }

        public List<FoodPercentageView> SaveFoodPercentages(List<FoodPercentageView> percentageList)
        {
            percentageList.ForEach(pl =>
            {
                var plModel = new FoodPercentage();

                if (pl.Id > 0)
                {
                    plModel = Repository.GetById<FoodPercentage>(pl.Id);
                    pl.SetTo(plModel);
                    Repository.Update(plModel);
                }
                else
                {
                    pl.SetTo(plModel);
                    Repository.Create(plModel);
                    pl.Id = plModel.Id;
                }                    
                //pl=plModel.ToView();
            });
            return percentageList;
        }
    }
}
