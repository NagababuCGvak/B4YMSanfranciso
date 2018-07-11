using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Better4You.Core;
using Better4You.Core.Repositories;

using Better4You.Meal.Config;
using Better4You.Meal.EntityModel;
using Better4You.Meal.ViewModel;
using Better4You.UserManagment.EntityModel;
using Tar.Core.Exceptions;
using Tar.Core.Extensions;
using Tar.Security;
using GeneralItemView = Tar.ViewModel.GeneralItemView;
using RecordStatuses = Better4You.Meal.Config.RecordStatuses;
using SysMngConfig = Better4You.UserManagement.Config;

namespace Better4You.Meal.Business.Implementation
{
    public class MealMenuOrderFacade : IMealMenuOrderFacade
    {
        public IConfigRepository Repository { get; set; }

        public class MealMenuOrderItemComparer : IEqualityComparer<MealMenuOrderItemView>
        {
            public bool Equals(MealMenuOrderItemView x, MealMenuOrderItemView y)
            {

                return (x.MealType == y.MealType && x.MealMenuId == y.MealMenuId &&
                        x.MealMenuValidDate == y.MealMenuValidDate && x.MenuId == y.MenuId && x.MenuName == y.MenuName &&
                        x.MenuType == y.MenuType);
            }

            public int GetHashCode(MealMenuOrderItemView obj)
            {
                if (obj == null) { return 0; }
                return obj.GetHashCode();
            }
        }

        private DateTime AddBusinessDays(DateTime date, int days)
        {
            if (days == 0)
                return date;

            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(2);
                days -= 1;
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
                days -= 1;
            }

            date = date.AddDays(days / 5 * 7);
            int extraDays = days % 5;

            if ((int)date.DayOfWeek + extraDays > 5)
            {
                extraDays += 2;
            }

            return date.AddDays(extraDays);

        }

        public MealOrderManageView GetSchoolOrder(MealMenuOrderFilterView filter)
        {

            var currentUser = Thread.CurrentPrincipal.AsCurrentUser();

            var isCompanyUser = (currentUser.UserTypeId() == (int)UserTypes.Company);

            var orderDate = filter.OrderDate ?? DateTime.Now;
            var startedAt = new DateTime(orderDate.Year, orderDate.Month, 1);
            var endedAt = new DateTime(orderDate.Year, orderDate.Month,
                DateTime.DaysInMonth(orderDate.Year, orderDate.Month));

            var mealMenuOrder = filter.MealMenuOrderId > 0
                ? Repository.GetById<MealMenuOrder>(filter.MealMenuOrderId)
                : Repository.Query<MealMenuOrder>()
                    .FirstOrDefault(d =>
                        d.School.Id == filter.SchoolId &&
                        d.RecordStatus == filter.RecordStatusId &&
                        d.OrderDate >= startedAt && d.OrderDate <= endedAt &&
                        d.MealType == filter.MealTypeId);

            var manageOrder = new MealOrderManageView
            {
                OrderId = 0,
                MealTypeId = filter.MealTypeId,
                Days = new List<MealOrderManageDayView>(),
                OrderIsSubmitted = false,
                Year = orderDate.Year,
                Month = orderDate.Month,
                SchoolId = filter.SchoolId,
                SchoolType = filter.SchoolType
            };

            var query = Repository.Query<MealMenu>()
                // ReSharper disable ImplicitlyCapturedClosure
                .Where(d => d.RecordStatus == (int)RecordStatuses.Active && d.MealType == filter.MealTypeId &&
                            // ReSharper restore ImplicitlyCapturedClosure
                            d.ValidDate >= startedAt && d.ValidDate <= endedAt);


            var schoolTypeMealMenu = query.Where(d => d.Menu.SchoolType != (int)MenuSchoolTypes.None && d.Menu.SchoolType == filter.SchoolType)
                .Select(d => new MealMenuOrderItemView
                {
                    MealType = new GeneralItemView { Id = d.MealType },
                    MealMenuId = d.Id,
                    MealMenuValidDate = d.ValidDate,
                    MenuId = d.Menu.Id,
                    MenuName = d.Menu.Name,
                    MenuType = new GeneralItemView { Id = d.Menu.MenuType },
                    HasAdditionalFruit = d.Menu.AdditionalFruit,
                    HasAdditionalVegetable = d.Menu.AdditionalVeg,
                }).AsEnumerable();

            var schoolMealMenu = query.Where(d => d.Menu.Schools.Any(k => k.Id == filter.SchoolId) && d.Menu.SchoolType == (int)MenuSchoolTypes.None)
                .Select(d => new MealMenuOrderItemView
                {
                    MealType = new GeneralItemView { Id = d.MealType },
                    MealMenuId = d.Id,
                    MealMenuValidDate = d.ValidDate,
                    MenuId = d.Menu.Id,
                    MenuName = d.Menu.Name,
                    MenuType = new GeneralItemView { Id = d.Menu.MenuType },
                    HasAdditionalFruit = d.Menu.AdditionalFruit,
                    HasAdditionalVegetable = d.Menu.AdditionalVeg
                }).AsEnumerable();
            var genericMealMenu = query.Where(d => !d.Menu.Schools.Any() && d.Menu.SchoolType == (int)MenuSchoolTypes.None)
                .Select(d => new MealMenuOrderItemView
                {
                    MealType = new GeneralItemView { Id = d.MealType },
                    MealMenuId = d.Id,
                    MealMenuValidDate = d.ValidDate,
                    MenuId = d.Menu.Id,
                    MenuName = d.Menu.Name,
                    MenuType = new GeneralItemView { Id = d.Menu.MenuType },
                    HasAdditionalFruit = d.Menu.AdditionalFruit,
                    HasAdditionalVegetable = d.Menu.AdditionalVeg
                }).AsEnumerable();

            var mealMenuList = schoolMealMenu.ToList();
            mealMenuList.AddRange(genericMealMenu);
            mealMenuList.AddRange(schoolTypeMealMenu);

            var lastChangeDate = AddBusinessDays(DateTime.Today, 3);

            var manageOrderDays = mealMenuList.GroupBy(ml => ml.MealMenuValidDate).Select(ml =>
                new MealOrderManageDayView
                {
                    Items = ml.Select(x => new MealOrderManageDayItemView
                    {
                        Id = 0,
                        Count = 0,
                        MealMenuId = x.MealMenuId,
                        MenuName = x.MenuName,
                        MenuId = x.MenuId,
                        ModifiedByFullName = x.ModifiedByFullName,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedReason = x.ModifiedReason,
                        ModifiedAt = x.ModifiedAt,
                        MealServiceTypeId = 0,
                        MealTypeId = x.MealType.Id,
                        IsSoyMilk = (x.MenuType.Id == MenuTypes.SoyMilk.ToInt64()),
                        IsMilk = (x.MenuType.Id == MenuTypes.Milk.ToInt64()),
                        HasAdditionalFruit = x.HasAdditionalFruit,
                        HasAdditionalVegetable = x.HasAdditionalVegetable,
                    }).ToList(),
                    Date = ml.Key,
                    DeliveryTypeId = filter.MealTypeId.GetDefaultDeliveryTypeOfMealType(),
                    SchoolId = filter.SchoolId,
                    MealTypeId = filter.MealTypeId,
                    IsEditable = isCompanyUser
                        ? ml.Key >= DateTime.Today
                         : ml.Key > lastChangeDate,
                    FruitCount = 0,
                    VegetableCount = 0
                }).ToList();

            if (mealMenuOrder != null)
            {
                manageOrder.OrderId = mealMenuOrder.Id;
                manageOrder.OrderIsSubmitted = (mealMenuOrder.OrderStatus == OrderStatuses.Submitted.ToInt64());

                var orderDayProps =
                    Repository.Query<MealMenuOrderDay>()
                        .Where(d => d.MealMenuOrder.Id == mealMenuOrder.Id)
                        .Select(d => new { d.FruitCount, d.VegetableCount, d.ValidDate, d.DeliveryType })
                        .ToList();

                var mealMenuOrderItems =
                    mealMenuOrder.MealMenuOrderItems.Where(d => d.RecordStatus == (int)RecordStatuses.Active)
                        .Select(
                            d =>
                                new
                                {
                                    d.Id,
                                    MealMenuId = d.MealMenu.Id,
                                    d.MealMenu.ValidDate,
                                    d.TotalCount,
                                    d.MealServiceType,
                                    /*,
                                    d.ModifiedBy,
                                    d.ModifiedByFullName,
                                    d.ModifiedReason,
                                    d.ModifiedAt
                                    */
                                }).ToList();

                manageOrderDays.ForEach(mo =>
                {
                    var dayPropItem = orderDayProps.FirstOrDefault(od => od.ValidDate == mo.Date);
                    if (dayPropItem != null)
                    {
                        mo.FruitCount = dayPropItem.FruitCount ?? 0;
                        mo.VegetableCount = dayPropItem.VegetableCount ?? 0;
                        mo.DeliveryTypeId = dayPropItem.DeliveryType;
                    }
                    mo.Items.ForEach(item =>
                    {
                        var orderedItem =
                            mealMenuOrderItems.FirstOrDefault(
                                k => k.ValidDate == mo.Date && k.MealMenuId == item.MealMenuId);
                        if (orderedItem == null) return;

                        item.Id = orderedItem.Id;
                        item.Count = (orderedItem.TotalCount ?? 0);
                        item.MealServiceTypeId = orderedItem.MealServiceType;
                        /*
                            item.ModifiedByFullName = orderedItem.ModifiedByFullName;
                            item.ModifiedBy = orderedItem.ModifiedBy;
                            item.ModifiedReason = orderedItem.ModifiedReason;
                            item.ModifiedAt = orderedItem.ModifiedAt;
                            */
                    });
                });
                var count = orderDayProps.ToList();
            }

            manageOrder.Days = manageOrderDays.OrderBy(x => x.Date).ToList();
            manageOrder.Days.ForEach(x => x.Items = x.Items.OrderBy(y => y.IsMilk).ThenBy(y => y.IsSoyMilk).ToList());
            return manageOrder;


        }

        public MealMenuOrderView GetByFilter(MealMenuOrderFilterView filter)
        {
            var orderDate = filter.OrderDate ?? DateTime.Now;
            var startedAt = new DateTime(orderDate.Year, orderDate.Month, 1);
            var endedAt = new DateTime(orderDate.Year, orderDate.Month,
                DateTime.DaysInMonth(orderDate.Year, orderDate.Month));
            var query = Repository.Query<MealMenu>()
                // ReSharper disable ImplicitlyCapturedClosure
                .Where(d => d.RecordStatus == (int)RecordStatuses.Active && d.MealType == filter.MealTypeId &&
                            // ReSharper restore ImplicitlyCapturedClosure
                            d.ValidDate >= startedAt && d.ValidDate <= endedAt);


            var schoolTypeMealMenu = query.Where(d => d.Menu.SchoolType != (int)MenuSchoolTypes.None && d.Menu.SchoolType == filter.SchoolType)
                .Select(d => new MealMenuOrderItemView
                {
                    MealType = new GeneralItemView { Id = d.MealType },
                    MealMenuId = d.Id,
                    MealMenuValidDate = d.ValidDate,
                    MenuId = d.Menu.Id,
                    MenuName = d.Menu.Name,
                    MenuType = new GeneralItemView { Id = d.Menu.MenuType },
                    HasAdditionalFruit = d.Menu.AdditionalFruit,
                    HasAdditionalVegetable = d.Menu.AdditionalVeg
                }).AsEnumerable();

            var schoolMealMenu = query.Where(d => d.Menu.Schools.Any(k => k.Id == filter.SchoolId) && d.Menu.SchoolType == (int)MenuSchoolTypes.None)
                .Select(d => new MealMenuOrderItemView
                {
                    MealType = new GeneralItemView { Id = d.MealType },
                    MealMenuId = d.Id,
                    MealMenuValidDate = d.ValidDate,
                    MenuId = d.Menu.Id,
                    MenuName = d.Menu.Name,
                    MenuType = new GeneralItemView { Id = d.Menu.MenuType },
                    HasAdditionalFruit = d.Menu.AdditionalFruit,
                    HasAdditionalVegetable = d.Menu.AdditionalVeg
                }).AsEnumerable();
            var genericMealMenu = query.Where(d => !d.Menu.Schools.Any() && d.Menu.SchoolType == (int)MenuSchoolTypes.None)
                .Select(d => new MealMenuOrderItemView
                {
                    MealType = new GeneralItemView { Id = d.MealType },
                    MealMenuId = d.Id,
                    MealMenuValidDate = d.ValidDate,
                    MenuId = d.Menu.Id,
                    MenuName = d.Menu.Name,
                    MenuType = new GeneralItemView { Id = d.Menu.MenuType },
                    HasAdditionalFruit = d.Menu.AdditionalFruit,
                    HasAdditionalVegetable = d.Menu.AdditionalVeg
                }).AsEnumerable();

            var mealMenuList = schoolMealMenu.ToList();
            mealMenuList.AddRange(genericMealMenu);
            mealMenuList.AddRange(schoolTypeMealMenu);

            mealMenuList.ForEach(x =>
            {
                x.MenuType = Lookups.GetItem<MenuTypes>(x.MenuType.Id);
                x.MealType = Lookups.GetItem<MealTypes>(x.MealType.Id);
                x.DeliveryType = Lookups.GetItem<DeliveryTypes>(x.MealType.Id.GetDefaultDeliveryTypeOfMealType());
            });

            var mealMenuOrder = filter.MealMenuOrderId > 0
                ? Repository.GetById<MealMenuOrder>(filter.MealMenuOrderId)
                : Repository.Query<MealMenuOrder>()
                    .FirstOrDefault(d =>
                        d.School.Id == filter.SchoolId &&
                        d.RecordStatus == filter.RecordStatusId &&
                        d.OrderDate >= startedAt && d.OrderDate <= endedAt &&
                        d.MealType == filter.MealTypeId);

            var mealMenuOrderView = mealMenuOrder.ToView<MealMenuOrderView>() ??
                                    new MealMenuOrderView
                                    {
                                        SchoolId = filter.SchoolId,
                                        OrderStatus = Lookups.OrderStatusList.FirstOrDefault(d => d.Id == filter.OrderStatusId),
                                        RecordStatus = Lookups.RecordStatusList.FirstOrDefault(d => d.Id == filter.RecordStatusId),
                                        OrderDate = startedAt
                                    };


            if (mealMenuOrder != null)
            {
                var mealMenuOrderItems =
                    mealMenuOrder.MealMenuOrderItems.Where(d => d.RecordStatus == (int)RecordStatuses.Active).Select(
                        d =>
                            new MealMenuOrderItemView
                            {
                                Id = d.Id,
                                MealMenuOrderId = mealMenuOrder.Id,
                                ModifiedBy = d.ModifiedBy,
                                ModifiedByFullName = d.ModifiedByFullName,
                                ModifiedReason = d.ModifiedReason,
                                ModifiedAt = d.ModifiedAt,
                                Version = d.Version,
                                RefId = d.RefId,
                                TotalCount = d.TotalCount,
                                RecordStatus = Lookups.RecordStatusList.FirstOrDefault(k => k.Id == d.RecordStatus),
                                MealType = Lookups.MealTypeList.FirstOrDefault(k => k.Id == d.MealMenu.MealType),
                                MealMenuId = d.MealMenu.Id,
                                MealMenuValidDate = d.MealMenu.ValidDate,
                                MenuId = d.MealMenu.Menu.Id,
                                MenuName = d.MealMenu.Menu.Name,
                                MenuType = Lookups.MenuTypeList.FirstOrDefault(k => k.Id == d.MealMenu.Menu.MenuType),
                                MealServiceType = Lookups.GetItem<MealServiceTypes>(d.MealServiceType),
                            }).AsEnumerable().ToList();

                mealMenuOrderView.OrderItems = (from mealmenu in mealMenuList

                                                join mealMenuOrderItem in mealMenuOrderItems on mealmenu.MealMenuId equals mealMenuOrderItem.MealMenuId into mealMenu1

                                                from mm1 in mealMenu1.DefaultIfEmpty()
                                                let orderItem = mealMenu1.FirstOrDefault()
                                                //join dayProps in orderDayProps on orderItem.MealMenuValidDate equals dayProps.ValidDate                      
                                                select new MealMenuOrderItemView
                                                {
                                                    MealMenuOrderId = orderItem != null ? orderItem.MealMenuOrderId : 0,
                                                    Id = orderItem != null ? orderItem.Id : 0,
                                                    RefId = orderItem != null ? orderItem.RefId : null,
                                                    TotalCount = orderItem != null ? orderItem.TotalCount : null,
                                                    Version = orderItem != null ? orderItem.Version : 0,
                                                    ModifiedBy = orderItem != null ? orderItem.ModifiedBy : "",
                                                    ModifiedByFullName = orderItem != null ? orderItem.ModifiedByFullName : "",
                                                    ModifiedReason = orderItem != null ? orderItem.ModifiedReason : "",
                                                    ModifiedAt = orderItem != null ? orderItem.ModifiedAt : DateTime.Now,
                                                    MealMenuId = mealmenu.MealMenuId,
                                                    MealType = mealmenu.MealType,
                                                    MealMenuValidDate = mealmenu.MealMenuValidDate,
                                                    MenuId = mealmenu.MenuId,
                                                    MenuName = mealmenu.MenuName,
                                                    MenuType = mealmenu.MenuType,
                                                    MealServiceType = orderItem != null ? orderItem.MealServiceType : null,
                                                    HasAdditionalVegetable = mealmenu.HasAdditionalVegetable,
                                                    HasAdditionalFruit = mealmenu.HasAdditionalFruit,
                                                }).ToList();

            }
            else
            {
                mealMenuOrderView.OrderItems = mealMenuList;
            }

            //mealMenuOrderView.OrderItems.ForEach(GetDataOrderItem);


            return mealMenuOrderView;
        }


        private readonly Dictionary<long, List<Tuple<DateTime, long>>> _orderDeliveryTypes = new Dictionary<long, List<Tuple<DateTime, long>>>();
        private GeneralItemView GetDeliveryType(long orderId, DateTime date, long mealTypeId)
        {
            //var key = string.Format("{0}-{1}", date.Ticks, mealTypeId);
            if (orderId == 0)
                return Lookups.GetItem<DeliveryTypes>(mealTypeId.GetDefaultDeliveryTypeOfMealType());

            if (!_orderDeliveryTypes.ContainsKey(orderId))
                _orderDeliveryTypes[orderId] =
                    Repository.Query<MealMenuOrderDay>()
                        .Where(d => d.MealMenuOrder.Id == orderId)
                        .Select(d => new Tuple<DateTime, long>(d.ValidDate, d.DeliveryType))
                        .ToList();

            var dailyDeliveryType = _orderDeliveryTypes[orderId].FirstOrDefault(d => d.Item1 == date);

            return Lookups.GetItem<DeliveryTypes>(dailyDeliveryType == null ? mealTypeId.GetDefaultDeliveryTypeOfMealType() : dailyDeliveryType.Item2);

        }


        private void GetDataOrderItem(MealMenuOrderItemView item)
        {
            if (item == null) return;
            //var date = new DateTime(item.MealMenuValidDate.Year, item.MealMenuValidDate.Month, 1);

            if (item.RecordStatus != null)
                item.RecordStatus = Lookups.GetItem<RecordStatuses>(item.RecordStatus.Id);
            if (item.MealType != null)
                item.MealType = Lookups.GetItem<MealTypes>(item.MealType.Id);
            if (item.MenuType != null)
                item.MenuType = Lookups.GetItem<MenuTypes>(item.MenuType.Id);
            if (item.MealServiceType != null)
                item.MealServiceType = Lookups.GetItem<MealServiceTypes>(item.MealServiceType.Id);
            item.Foods = Repository.Query<Food>().Where(d => d.Menus
                .Any(k => k.Id == item.MenuId)).AsEnumerable().
                Select(d => d.ToView<FoodListItemView>()).ToList();

            if (item.MealType != null)
                item.DeliveryType = GetDeliveryType(item.MealMenuOrderId, item.MealMenuValidDate, item.MealType.Id);
        }

        public MealMenuOrderView Get(long id)
        {
            return GetByFilter(new MealMenuOrderFilterView { MealMenuOrderId = id });
        }

        public MealMenuOrderView GetMealMenuOrderById(long id)
        {
            var mealMenuOrder = Repository.GetById<MealMenuOrder>(id);
            var mealMenuOrderView = mealMenuOrder.ToView<MealMenuOrderView>();
            return mealMenuOrderView;
        }

        public List<MealMenuOrderMenuView> GetOrderMenuItems(MealMenuOrderItemFilterView filter)
        {
            var query = Repository.Query<MealMenuOrderItem>().Where(d => d.RecordStatus == (long)RecordStatuses.Active);
            if (filter.OrderId.HasValue)
                query = query.Where(d => d.MealMenuOrder.Id == filter.OrderId.Value);
            if (filter.OrderDate.HasValue)
                query = query.Where(d => d.MealMenuOrder.OrderDate == filter.OrderDate.Value);
            if (filter.SchoolId > 0)
                query = query.Where(d => d.MealMenuOrder.School.Id == filter.SchoolId);
            return query.Select(d => d.MealMenu.Menu)
                .Distinct()
                .Select(d => new MealMenuOrderMenuView
                {
                    MenuId = d.Id,
                    //MenuType = new KeyValuePair<long, string>(d.MenuType.Id, d.MenuType.FieldText),
                    MenuType = Lookups.MenuTypeList.FirstOrDefault(k => k.Id == d.MenuType),
                    MenuName = d.Name,
                    Foods = d.Foods.AsEnumerable().Select(k => k.ToView()).ToList()
                }).ToList();

        }

        public MealMenuOrderItemView GetOrderItemByFilter(MealMenuOrderItemFilterView filter)
        {
            MealMenuOrderItemView orderItem;

            if (filter.OrderItemId.HasValue && filter.OrderItemId.Value > 0)
            {
                orderItem = Repository.Query<MealMenuOrderItem>().Where(d => d.Id == filter.OrderItemId).
                    Select(d => new MealMenuOrderItemView
                    {
                        Id = d.Id,
                        MealMenuOrderId = d.MealMenuOrder.Id,
                        RefId = d.RefId,
                        Version = d.Version,
                        TotalCount = d.TotalCount,
                        ModifiedBy = d.ModifiedBy,
                        ModifiedByFullName = d.ModifiedByFullName,
                        ModifiedAt = d.ModifiedAt,
                        ModifiedReason = d.ModifiedReason,
                        AdjusmentCount = d.AdjusmentCount,
                        Rate = d.Rate,
                        MealMenuId = d.MealMenu.Id,
                        MealMenuValidDate = d.MealMenu.ValidDate,
                        MenuName = d.MealMenu.Menu.Name,
                        MenuId = d.MealMenu.Menu.Id,
                        //MenuType = Lookups.MenuTypeList.FirstOrDefault(k=>k.Id==d.MealMenu.Menu.MenuType),
                        //MealServiceType = Lookups.MealServiceTypeList.FirstOrDefault(k => k.Id == d.MealServiceType)
                        //MealType = Lookups.MealTypeList.FirstOrDefault(k => k.Id == d.MealMenu.MealType),
                        //RecordStatus = Lookups.RecordStatusList.FirstOrDefault(k => k.Id == d.RecordStatus),

                        RecordStatus = new GeneralItemView { Id = d.RecordStatus },
                        MealType = new GeneralItemView { Id = d.MealMenu.MealType },
                        MenuType = new GeneralItemView { Id = d.MealMenu.Menu.MenuType },
                        MealServiceType = new GeneralItemView { Id = d.MealServiceType }
                    }).FirstOrDefault();
            }
            else
            {
                orderItem = Repository.Query<MealMenu>().Where(d => d.Id == filter.MealMenuId).
                    Select(d => new MealMenuOrderItemView
                    {
                        MealMenuId = d.Id,
                        MealMenuValidDate = d.ValidDate,
                        MenuName = d.Menu.Name,
                        MenuId = d.Menu.Id,
                        //MealType = Lookups.MenuTypeList.FirstOrDefault(k => k.Id == d.MealType),
                        //MenuType = Lookups.MenuTypeList.FirstOrDefault(k => k.Id == d.Menu.MenuType)
                        RecordStatus = new GeneralItemView { Id = (long)RecordStatuses.Active },
                        MealType = new GeneralItemView { Id = d.MealType },
                        MenuType = new GeneralItemView { Id = d.Menu.MenuType },
                        MealServiceType = new GeneralItemView { Id = (long)MealServiceTypes.None }
                    }).FirstOrDefault();
            }
            if (orderItem != null)
                GetDataOrderItem(orderItem);
            return orderItem;
        }

        public List<MealMenuOrderItemHistoricalView> GetOrderItemHistory(long id)
        {
            return
                Repository.Query<MealMenuOrderItem>()
                    .Where(d => d.RefId == id)
                    .OrderByDescending(d => d.CreatedAt)
                    .AsEnumerable()
                    .Select(d => d.ToView<MealMenuOrderItemHistoricalView>())
                    .ToList();
        }

        public bool SubmitOrder(MealMenuOrderFilterView filter, long userId)
        {

            var schoolOrder = GetSchoolOrder(filter);
            var isOrderSubmitable = schoolOrder.Days.Any(d => d.Items.Any(k => k.Id > 0));
            if (isOrderSubmitable)
            {
                if (schoolOrder.OrderId > 0)
                {
                    var mealMenuOrder = Repository.GetById<MealMenuOrder>(schoolOrder.OrderId);
                    mealMenuOrder.OrderStatus = (long)OrderStatuses.Submitted;
                    mealMenuOrder.ModifiedReason = "Order Submitted";
                    mealMenuOrder.ModifiedAt = DateTime.Now;
                    Repository.Update(mealMenuOrder);
                }
            }
            return isOrderSubmitable;
        }

        public MealMenuOrderItemView SaveOrderItem(MealMenuOrderItemView orderItem, long schoolId, double? orderRate)
        {
            var dtNow = DateTime.Now;
            MealMenuOrderItemView newOrderItem;

            if (orderItem.Id > 0)
            {
                var prevOrderItem = Repository.GetById<MealMenuOrderItem>(orderItem.Id);
                prevOrderItem.RecordStatus = (long)RecordStatuses.InActive;
                prevOrderItem.ModifiedBy = orderItem.ModifiedBy;
                prevOrderItem.ModifiedByFullName = orderItem.ModifiedByFullName;
                prevOrderItem.ModifiedAt = dtNow;
                prevOrderItem.ModifiedReason = orderItem.ModifiedReason;
                if (!prevOrderItem.RefId.HasValue)
                    prevOrderItem.RefId = orderItem.Id;


                Repository.Update(prevOrderItem);

                if (orderItem.MealType.Id == 0)
                    orderItem.MealType = Lookups.GetItem<MealTypes>((long)MealTypes.Breakfast);
                //orderItem.MealType = new KeyValuePair<long, string>((int)MealTypes.Breakfast, MealTypes.Breakfast.ToString());
                var orderItemModel = new MealMenuOrderItem
                {
                    MealMenuOrder = prevOrderItem.MealMenuOrder,
                    MealMenu = prevOrderItem.MealMenu,
                    RefId = prevOrderItem.RefId,
                    TotalCount = orderItem.TotalCount,
                    Rate = orderItem.Rate,
                    AdjusmentCount = orderItem.AdjusmentCount,
                    Version = prevOrderItem.Version + 1,
                    RecordStatus = (long)RecordStatuses.Active,
                    ModifiedBy = orderItem.ModifiedBy,
                    ModifiedByFullName = orderItem.ModifiedByFullName,
                    ModifiedAt = dtNow,
                    CreatedBy = orderItem.ModifiedBy,
                    CreatedByFullName = orderItem.ModifiedByFullName,
                    CreatedAt = dtNow,
                    ModifiedReason = orderItem.ModifiedReason,
                    MealServiceType = orderItem.MealServiceType.Id
                };

                var duplicatedOrderItem = (from order in Repository.Query<MealMenuOrderItem>()
                                           where
                                               order.MealMenuOrder.Id == prevOrderItem.MealMenuOrder.Id
                                               && order.MealMenu.Id == prevOrderItem.MealMenu.Id
                                               && order.Version > prevOrderItem.Version
                                           select order).FirstOrDefault();

                if (duplicatedOrderItem != null)
                    throw new ApplicationException(
                        "Occurred duplicate order items problem. \n Please refresh monthly order.");

                Repository.Create(orderItemModel);
                newOrderItem = new MealMenuOrderItemView
                {
                    Id = orderItemModel.Id,
                    MealMenuOrderId = orderItemModel.MealMenuOrder.Id,
                    RefId = orderItemModel.RefId,
                    Version = orderItemModel.Version,
                    TotalCount = orderItemModel.TotalCount,
                    ModifiedBy = orderItemModel.ModifiedBy,
                    ModifiedByFullName = orderItemModel.ModifiedByFullName,
                    ModifiedAt = orderItemModel.ModifiedAt,
                    RecordStatus = Lookups.GetItem<RecordStatuses>(orderItemModel.RecordStatus),
                    MealMenuId = orderItemModel.MealMenu.Id,
                    MealMenuValidDate = orderItemModel.MealMenu.ValidDate,
                    MealType = Lookups.GetItem<MealTypes>(orderItemModel.MealMenu.MealType),
                    MenuId = orderItemModel.MealMenu.Menu.Id,
                    MenuName = orderItemModel.MealMenu.Menu.Name,
                    MenuType = Lookups.GetItem<MenuTypes>(orderItemModel.MealMenu.Menu.MenuType),
                    Rate = orderItemModel.Rate,
                    AdjusmentCount = orderItemModel.AdjusmentCount,
                    MealServiceType = Lookups.GetItem<MealServiceTypes>(orderItemModel.MealServiceType)
                };
            }
            else
            {
                var mealMenu = Repository.GetById<MealMenu>(orderItem.MealMenuId);
                var orderDate = mealMenu.ValidDate;

                var startedAt = new DateTime(orderDate.Year, orderDate.Month, 1);
                var endedAt = new DateTime(orderDate.Year, orderDate.Month,
                    DateTime.DaysInMonth(orderDate.Year, orderDate.Month));
                var mealMenuOrder = Repository.Query<MealMenuOrder>()
                    .FirstOrDefault(d =>
                        d.School.Id == schoolId &&
                        d.RecordStatus == (int)RecordStatuses.Active &&
                        d.OrderDate >= startedAt && d.OrderDate <= endedAt &&
                        d.MealType == mealMenu.MealType)

                                    ??
                                    new MealMenuOrder
                                    {
                                        School = new School { Id = schoolId },
                                        OrderDate = startedAt,
                                        Rate = orderRate,
                                        OrderStatus = (long)OrderStatuses.InitialState,
                                        RecordStatus = (long)RecordStatuses.Active,
                                        ModifiedBy = orderItem.ModifiedBy,
                                        ModifiedByFullName = orderItem.ModifiedByFullName,
                                        ModifiedAt = dtNow,
                                        CreatedBy = orderItem.ModifiedBy,
                                        CreatedByFullName = orderItem.ModifiedByFullName,
                                        CreatedAt = dtNow,
                                        ModifiedReason = orderItem.ModifiedReason,
                                        MealType = mealMenu.MealType
                                    };

                if (mealMenuOrder.MealMenuOrderItems.Any(
                    d =>
                        d.MealMenu.Id == orderItem.MealMenuId &&
                        d.RecordStatus == (long)RecordStatuses.Active))
                    throw new Exception("Meal Menu Already Exist!\n Please refresh monthly order.");

                var orderItemModel = new MealMenuOrderItem
                {
                    MealMenu = mealMenu,
                    TotalCount = orderItem.TotalCount,
                    Version = 0,
                    RecordStatus = (long)RecordStatuses.Active,
                    ModifiedBy = orderItem.ModifiedBy,
                    ModifiedByFullName = orderItem.ModifiedByFullName,
                    ModifiedAt = dtNow,
                    CreatedBy = orderItem.ModifiedBy,
                    CreatedByFullName = orderItem.ModifiedByFullName,
                    CreatedAt = dtNow,
                    MealServiceType = orderItem.MealServiceType.Id
                    //MealServiceType = Repository.GetById<MealServiceType>(orderItem.MealServiceTypeId)
                };
                mealMenuOrder.AddMealMenuOrderItem(orderItemModel);
                Repository.Create(mealMenuOrder);
                newOrderItem = new MealMenuOrderItemView
                {
                    Id = orderItemModel.Id,
                    MealMenuOrderId = mealMenuOrder.Id,
                    RefId = orderItemModel.RefId,
                    Version = orderItemModel.Version,
                    TotalCount = orderItemModel.TotalCount,
                    ModifiedBy = orderItemModel.ModifiedBy,
                    ModifiedByFullName = orderItemModel.ModifiedByFullName,
                    ModifiedAt = orderItemModel.ModifiedAt,
                    RecordStatus = Lookups.GetItem<RecordStatuses>(orderItemModel.RecordStatus),
                    MealMenuId = orderItemModel.MealMenu.Id,
                    MealMenuValidDate = orderItemModel.MealMenu.ValidDate,
                    MealType = Lookups.GetItem<MealTypes>(orderItemModel.MealMenu.MealType),

                    MenuId = orderItemModel.MealMenu.Menu.Id,
                    MenuName = orderItemModel.MealMenu.Menu.Name,
                    MenuType = Lookups.GetItem<MenuTypes>(orderItemModel.MealMenu.Menu.MenuType),
                    MealServiceType = Lookups.GetItem<MealServiceTypes>(orderItemModel.MealServiceType)
                };

            }
            return newOrderItem;
        }

        public void DeleteOrderItem(MealMenuOrderItemView orderItem)
        {
            var modelOrderItem = Repository.GetById<MealMenuOrderItem>(orderItem.Id);
            modelOrderItem.ModifiedBy = orderItem.ModifiedBy;
            modelOrderItem.ModifiedByFullName = orderItem.ModifiedByFullName;
            modelOrderItem.ModifiedAt = DateTime.Now;
            modelOrderItem.ModifiedReason = orderItem.ModifiedReason;
            modelOrderItem.RecordStatus = (long)RecordStatuses.Deleted;
            Repository.Update(modelOrderItem);

        }

        public List<DailyItemsReportView> GetDailyItemsReport(MealMenuOrderFilterView filter)
        {

            var query = from order in Repository.Query<MealMenuOrder>()
                        join school in Repository.Query<School>() on order.School.Id equals school.Id
                        join dayProp in Repository.Query<MealMenuOrderDay>() on order.Id equals dayProp.MealMenuOrder.Id
                        where order.OrderStatus != (long)OrderStatuses.InitialState && order.OrderDate == filter.OrderDate
                        select
                            new
                            {
                                order.MealType,
                                SchoolCode = school.Code,
                                SchoolName = school.Name,
                                dayProp.FruitCount,
                                dayProp.VegetableCount,
                                OrderDay = dayProp.ValidDate
                            };

            var queryResuls = query.AsEnumerable().ToList();
            return queryResuls.Select(d => new DailyItemsReportView
            {

                SchoolName = d.SchoolName,
                SchoolCode = d.SchoolCode,
                FruitCount = d.FruitCount,
                VegetableCount = d.VegetableCount,
                OrderDay = d.OrderDay,
                MealType = Lookups.GetItem<MealTypes>(d.MealType)
            }).ToList();
        }





        public List<SchoolInvoiceListItemView> GetSchoolInvoiceListByFilter(InvoiceFilterView filter, int pageSize,
            int pageIndex, string orderByField, bool orderByAsc, out int totalCount)
        {
            var annualYear = DateTime.Now.Year;
            var menuTypes =
                Lookups.MenuTypeList.Where(d => d.Id != (long)MenuTypes.Milk).Select(d => d.Id).ToList();

            var query = Repository.Query<MealMenuOrder>().Where(d => d.OrderStatus != (long)OrderStatuses.InitialState);

            if (filter.OrderStatusId > 0)
                query = query.Where(d => d.OrderStatus == filter.OrderStatusId);

            if (filter.OrderDate.HasValue)
            {
                query = query.Where(d => d.OrderDate == filter.OrderDate.Value);
                annualYear = filter.OrderDate.Value.Year;
            }
            if (filter.SchoolId > 0)
                query = query.Where(d => d.School.Id == filter.SchoolId);

            if (filter.RecordStatusId > 0)
                query = query.Where(d => d.RecordStatus == filter.RecordStatusId);

            if (!String.IsNullOrWhiteSpace(filter.SchoolNameStartsWith))
                query = query.Where(d => d.School.Name.StartsWith(filter.SchoolNameStartsWith));

            var querySelect = query.Select(d => d.School).OrderBy(o => o.Name).Distinct();

            totalCount = query.Count();

            if (pageSize > 0) querySelect = querySelect.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var schoolList = querySelect.ToList();

            var schoolIdList = schoolList.Select(s => s.Id).ToList();

            var schoolSoyMilkRates = Repository.Query<SchoolAnnualAgreement>()
                .Where(d => schoolIdList.Contains(d.School.Id) && d.ItemType == (long)AnnualItemTypes.SoyMilk && d.Year == annualYear && d.RecordStatus == (long)SysMngConfig.RecordStatuses.Active)
                .Select(d => new { d.Price, SchoolId = d.School.Id })
                .ToList();

            var queryInvoices = query.Where(q => schoolIdList.Contains(q.School.Id)).Select(d => new InvoiceListItemView
            {
                Id = d.Id,
                OrderDate = d.OrderDate,
                OrderStatus = new GeneralItemView { Id = d.OrderStatus },
                SchoolId = d.School.Id,
                SchoolName = d.School.Name,
                SchoolType = SysMngConfig.Lookups.GetItem<SysMngConfig.SchoolTypes>(d.School.SchoolType).Text,
                RecordStatus = new GeneralItemView { Id = d.RecordStatus },
                Rate = d.Rate,
                TotalCredit = d.TotalCredit,
                MealType = new GeneralItemView { Id = d.MealType },
                DebitAmount = d.DebitAmount ?? 0,
                Note = d.Note
            });

            var invoices = queryInvoices.AsEnumerable().ToList();

            var invoicesId = invoices.Select(d => d.Id).ToArray();
            var invoiceTotalCounts = Repository.Query<MealMenuOrderItem>()
                .Where(d => invoicesId.Contains(d.MealMenuOrder.Id) &&
                            d.RecordStatus == (long)RecordStatuses.Active &&
                            menuTypes.Contains(d.MealMenu.Menu.MenuType) &&
                            d.MealMenu.RecordStatus == (long)RecordStatuses.Active)
                .Select(d => new {
                    MealMenuOrderId = d.MealMenuOrder.Id,

                    //d.TotalCount, 
                    TotalCount = (d.MealMenu.Menu.MenuType != (long)MenuTypes.SoyMilk ? d.TotalCount : 0),
                    SoyMilkCount = (d.MealMenu.Menu.MenuType == (long)MenuTypes.SoyMilk ? d.TotalCount : 0),
                    d.AdjusmentCount,
                    d.Rate
                })
                .AsEnumerable()
                .GroupBy(d => d.MealMenuOrderId)
                .Select(d => new
                {
                    OrderId = d.Key,
                    TotalCount = d.Sum(l => l.TotalCount),
                    SoyMilkCount = d.Sum(l => l.SoyMilkCount),
                    TotalAdjusmentCredit = d.Sum(l => (l.AdjusmentCount ?? 0) * (l.Rate ?? 0))
                })
                .ToList();

            var invoiceResult = (from invoice in invoices
                                 join invoiceTotalCount in invoiceTotalCounts on invoice.Id equals invoiceTotalCount.OrderId
                                 join schoolSoyMilkRate in schoolSoyMilkRates on invoice.SchoolId equals schoolSoyMilkRate.SchoolId
                                 into subSoyMilkRate
                                 from soyMilkRate in subSoyMilkRate.DefaultIfEmpty()
                                 select new InvoiceListItemView
                                 {
                                     Id = invoice.Id,
                                     OrderDate = invoice.OrderDate,
                                     //OrderStatus = invoice.OrderStatus,
                                     OrderStatus = Lookups.OrderStatusList.FirstOrDefault(k => k.Id == invoice.OrderStatus.Id),
                                     SchoolId = invoice.SchoolId,
                                     SchoolName = invoice.SchoolName,
                                     SchoolType = invoice.SchoolType,
                                     RecordStatus = Lookups.RecordStatusList.FirstOrDefault(k => k.Id == invoice.RecordStatus.Id),
                                     //invoice.RecordStatus,
                                     Rate = invoice.Rate,
                                     TotalCredit = invoice.TotalCredit,
                                     TotalAdjusmentCredit = invoiceTotalCount.TotalAdjusmentCredit,
                                     TotalCount = invoiceTotalCount.TotalCount ?? 0,
                                     SoyMilkCount = invoiceTotalCount.SoyMilkCount ?? 0,
                                     SoyMilkRate = soyMilkRate != null ? soyMilkRate.Price : 0,
                                     TotalAmount = ((invoiceTotalCount.TotalCount ?? 0) * (invoice.Rate ?? 0))
                                                   + ((invoiceTotalCount.SoyMilkCount ?? 0) * (soyMilkRate != null ? soyMilkRate.Price : 0))
                                                   - (invoice.TotalCredit ?? 0)
                                                   + invoiceTotalCount.TotalAdjusmentCredit + invoice.DebitAmount,
                                     //MealType = invoice.MealType,
                                     MealType = Lookups.MealTypeList.FirstOrDefault(k => k.Id == invoice.MealType.Id),
                                     DebitAmount = invoice.DebitAmount,
                                     Note = invoice.Note
                                 }).ToList();

            var schoolInvoiceDocumentList = (from schoolInvoiceDocument in Repository.Query<SchoolInvoiceDocument>()
                                             where
                                                 schoolIdList.Contains(schoolInvoiceDocument.SchoolId)
                                                                              && schoolInvoiceDocument.RecordStatus == (long)RecordStatuses.Active
                                             select
                                                 new
                                                 {
                                                     schoolInvoiceDocument.DocumentGuid,
                                                     schoolInvoiceDocument.SchoolId,
                                                     schoolInvoiceDocument.InvoiceYear,
                                                     schoolInvoiceDocument.InvoiceMonth
                                                 }).ToList();

            var schoolInvoiceList = new List<SchoolInvoiceListItemView>();
            schoolList.ForEach(s =>
            {
                var schoolInvoiceListItem = new SchoolInvoiceListItemView
                {
                    InvoiceList = invoiceResult.Where(i => i.SchoolId == s.Id).ToList()
                };
                schoolInvoiceListItem.SchoolId = schoolInvoiceListItem.InvoiceList[0].SchoolId;
                schoolInvoiceListItem.SchoolName = schoolInvoiceListItem.InvoiceList[0].SchoolName;
                schoolInvoiceListItem.SchoolType = schoolInvoiceListItem.InvoiceList[0].SchoolType;
                schoolInvoiceListItem.TotalAmount = schoolInvoiceListItem.InvoiceList.Sum(i => i.TotalAmount);
                schoolInvoiceListItem.DocumentGuid = schoolInvoiceDocumentList.Where(d =>
                    d.SchoolId == schoolInvoiceListItem.InvoiceList[0].SchoolId
                    && d.InvoiceYear == schoolInvoiceListItem.InvoiceList[0].OrderDate.Year
                    && d.InvoiceMonth == schoolInvoiceListItem.InvoiceList[0].OrderDate.Month)
                    .Select(d => d.DocumentGuid)
                    .FirstOrDefault();

                schoolInvoiceList.Add(schoolInvoiceListItem);
            });

            return schoolInvoiceList;
        }

        public List<InvoiceListItemView> GetInvoicesByFilter(InvoiceFilterView filter, int pageSize, int pageIndex,
            string orderByField, bool orderByAsc, out int totalCount)
        {
            var menuTypes =
                Lookups.MenuTypeList.Where(d => d.Id != (long)MenuTypes.Milk).Select(d => d.Id).ToList();

            var query = Repository.Query<MealMenuOrder>().Where(d => d.OrderStatus != (long)OrderStatuses.InitialState);

            if (filter.OrderStatusId > 0)
                query = query.Where(d => d.OrderStatus == filter.OrderStatusId);

            if (filter.OrderDate.HasValue)
                query = query.Where(d => d.OrderDate == filter.OrderDate.Value);

            if (filter.SchoolId > 0)
                query = query.Where(d => d.School.Id == filter.SchoolId);

            if (filter.RecordStatusId > 0)
                query = query.Where(d => d.RecordStatus == filter.RecordStatusId);

            if (filter.OrderId > 0)
                query = query.Where(d => d.Id == filter.OrderId);

            var querySelect = query.Select(d => new InvoiceListItemView
            {
                Id = d.Id,
                OrderDate = d.OrderDate,
                OrderStatus = new GeneralItemView { Id = d.OrderStatus },
                SchoolId = d.School.Id,
                SchoolName = d.School.Name,
                RecordStatus = new GeneralItemView { Id = d.RecordStatus },
                Rate = d.Rate,
                TotalCredit = d.TotalCredit,
                MealType = new GeneralItemView { Id = d.MealType },
                Note = d.Note
            });

            totalCount = querySelect.Count();

            switch (orderByField)
            {
                case "Id":
                    querySelect = orderByAsc ? querySelect.OrderBy(c => c.Id) : querySelect.OrderByDescending(c => c.Id);
                    break;
                case "SchoolName":
                    querySelect = orderByAsc
                        ? querySelect.OrderBy(c => c.SchoolName)
                        : querySelect.OrderByDescending(c => c.SchoolName);
                    break;
                case "OrderStatus":
                    querySelect = orderByAsc
                        ? querySelect.OrderBy(c => c.OrderStatus.Value)
                        : querySelect.OrderByDescending(c => c.OrderStatus.Value);
                    break;
                default:
                    querySelect = orderByAsc ? querySelect.OrderBy(c => c.Id) : querySelect.OrderByDescending(c => c.Id);
                    break;
            }

            if (pageSize > 0) querySelect = querySelect.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var invoices = querySelect.AsEnumerable().ToList();
            invoices.ForEach(x =>
            {
                x.OrderStatus = Lookups.GetItem<OrderStatuses>(x.OrderStatus.Id);
                x.RecordStatus = Lookups.GetItem<RecordStatuses>(x.RecordStatus.Id);
                x.MealType = Lookups.GetItem<MealTypes>(x.MealType.Id);
            });

            var invoicesId = invoices.Select(d => d.Id).ToArray();
            var invoiceTotalCounts = Repository.Query<MealMenuOrderItem>()
                .Where(d => invoicesId.Contains(d.MealMenuOrder.Id) &&
                            d.RecordStatus == (long)RecordStatuses.Active &&
                            menuTypes.Contains(d.MealMenu.Menu.MenuType))
                .Select(d => new { MealMenuOrderId = d.MealMenuOrder.Id, d.TotalCount, d.AdjusmentCount, d.Rate })
                .AsEnumerable()
                .GroupBy(d => d.MealMenuOrderId)
                .Select(d => new
                {
                    OrderId = d.Key,
                    TotalCount = d.Sum(l => l.TotalCount),
                    TotalAdjusmentCredit = d.Sum(l => l.AdjusmentCount ?? 0 * l.Rate ?? 0)
                })
                .ToList();

            var invoiceResult = (from invoice in invoices
                                 join invoiceTotalCount in invoiceTotalCounts on invoice.Id equals invoiceTotalCount.OrderId
                                 select new InvoiceListItemView
                                 {
                                     Id = invoice.Id,
                                     OrderDate = invoice.OrderDate,
                                     OrderStatus = invoice.OrderStatus,
                                     SchoolId = invoice.SchoolId,
                                     SchoolName = invoice.SchoolName,
                                     RecordStatus = invoice.RecordStatus,
                                     Note = invoice.Note,
                                     Rate = invoice.Rate,
                                     TotalCredit = invoice.TotalCredit,
                                     TotalAdjusmentCredit = invoiceTotalCount.TotalAdjusmentCredit,
                                     TotalCount = invoiceTotalCount.TotalCount ?? 0,
                                     TotalAmount =
                                                          ((invoiceTotalCount.TotalCount ?? 0) * invoice.Rate ??
                                          0 - (invoice.TotalCredit ?? 0 + invoiceTotalCount.TotalAdjusmentCredit)) + invoice.DebitAmount,
                                     MealType = invoice.MealType
                                 }).ToList();

            return invoiceResult;
        }

        public List<DateRangeReportOrderItemView> GetDateRenageOrderItems(DateRangeOrderItemFilterView filter)
        {
            var currentYear = filter.EndDate.HasValue ? filter.EndDate.Value.Year : DateTime.Now.Year;
            var query = (
                from mm in Repository.Query<MealMenu>()
                join mmoi in Repository.Query<MealMenuOrderItem>() on
                    new { F1 = mm.Id, F2 = (long)RecordStatuses.Active } equals
                    new { F1 = mmoi.MealMenu.Id, F2 = mmoi.RecordStatus }
                join mmo in Repository.Query<MealMenuOrder>() on
                    new { F1 = mmoi.MealMenuOrder.Id, F2 = (long)RecordStatuses.Active } equals
                    new { F1 = mmo.Id, F2 = mmo.RecordStatus }
                join s in Repository.Query<School>() on mmo.School.Id equals s.Id
                join saa in Repository.Query<SchoolAnnualAgreement>() on new { F1 = s.Id, F2 = currentYear } equals
                    new { F1 = saa.School.Id, F2 = saa.Year }
                    //join soyMilk in Repository.Query<SchoolAnnualAgreement>() on 
                    //    new { F1 = s.Id, F2 = currentYear, F3 = (long)AnnualItemTypes.SoyMilk } equals 
                    //    new { F1 = soyMilk.School.Id, F2 = soyMilk.Year, F3 = soyMilk.ItemType }
                where
                    mm.ValidDate >= filter.StartDate && mm.ValidDate <= filter.EndDate &&
                    mm.Menu.MenuType != (int)MenuTypes.Milk
                    //&& mmo.MealType == saa.MealType &&
                    && mmo.MealType == saa.ItemType &&
                    saa.Year == currentYear
                select new
                {
                    mmo.MealType,
                    mm.Menu.MenuType,
                    mmoi.TotalCount,
                    //saa.Price,
                    Price = mmo.Rate,
                    SchoolName = s.Name
                    //,SoyMilkPrice = soyMilk.Price
                });
            if (!string.IsNullOrWhiteSpace(filter.SchoolName))
                query = query.Where(s => s.SchoolName.Contains(filter.SchoolName));

            var orderItems = query.OrderBy(d => d.SchoolName).ToList();
            var soyMilkPriceList = (
                from s in Repository.Query<School>()
                join saa in Repository.Query<SchoolAnnualAgreement>()
                    on new
                    {
                        F1 = s.Id,
                        F2 = currentYear
                        ,
                        F3 = (long)AnnualItemTypes.SoyMilk
                    } equals
                    new
                    {
                        F1 = saa.School.Id,
                        F2 = saa.Year
                        ,
                        F3 = saa.ItemType
                    }
                    //select new { SchoolName = s.Name, SoyMilkPrice = saa == null ? 0 : saa.Price })
                where saa.ItemType == (long)AnnualItemTypes.SoyMilk
                select new { SchoolName = s.Name, SoyMilkPrice = saa.Price })
                .ToList();

            return orderItems.GroupBy(d => new { d.SchoolName }).Select(d => new
                DateRangeReportOrderItemView
            {
                SchoolName = d.Key.SchoolName,
                //MealList = d.GroupBy(m => new { m.MealType, m.Price })
                MealList = d.GroupBy(m => new { m.MealType })
                    .Select(m =>
                    {
                        var meal = m.Where(l => l.MenuType != (long)MenuTypes.SoyMilk).ToList();
                        var soyMilk = m.Where(l => l.MenuType == (long)MenuTypes.SoyMilk).ToList();

                        var mealCount = meal.Sum(l => l.TotalCount ?? 0);
                        var soyMilkCount = soyMilk.Sum(l => l.TotalCount ?? 0);
                        var mealPrice = meal.Any() ? meal.First().Price ?? 0 : 0;
                        //var soyMilkPrice = soyMilk.Any() ? soyMilk.First().SoyMilkPrice:0;
                        var soyMilkItem = soyMilkPriceList.FirstOrDefault(x => x.SchoolName == d.Key.SchoolName);
                        var soyMilkPrice = soyMilkItem == null ? 0 : soyMilkItem.SoyMilkPrice;

                        return new DateRangeReportOrderItemMealView
                        {
                            MealType = Lookups.GetItem<MealTypes>(m.Key.MealType),
                            TotalCount = mealCount + soyMilkCount,
                            //TotalPrice = m.Sum(l => l.TotalCount ?? 0) * m.Key.Price,
                            TotalPrice = mealCount * mealPrice + soyMilkCount * soyMilkPrice,
                            MenuList = m.GroupBy(mt => mt.MenuType)
                                             .Select(mt => new DateRangeReportOrderItemMealMenuView
                                             {
                                                 MenuType = Lookups.GetItem<MenuTypes>(mt.Key),
                                                 TotalCount = mt.Sum(l => l.TotalCount ?? 0),
                                                 TotalPrice = mt.Sum(l => (l.TotalCount ?? 0)) * (mt.Key == (long)MenuTypes.SoyMilk ? soyMilkPrice : mealPrice)
                                             }).ToList()
                        };
                    }).ToList()
            }).ToList();

        }


        public InvoiceListItemView InvoiceUpdate(InvoiceListItemView view, long userId)
        {
            var menuTypes =
                Lookups.MenuTypeList.Where(d => d.Id != (long)MenuTypes.Milk).Select(d => d.Id).ToList();
            var model = Repository.GetById<MealMenuOrder>(view.Id);

            model.OrderStatus = view.OrderStatus.Id;
            model.Rate = view.Rate;
            model.TotalCredit = view.TotalCredit;
            model.DebitAmount = view.DebitAmount;
            model.Note = view.Note;
            ///TODO Get User Implementation
            //model.ModifiedBy = userId;
            //model.ModifiedByFullName = userId;
            model.ModifiedAt = DateTime.Now;
            Repository.Update(model);
            var orderItems = model.MealMenuOrderItems
                .Where(
                    d =>
                        d.RecordStatus == (long)RecordStatuses.Active &&
                        menuTypes.Contains(d.MealMenu.Menu.MenuType))
                .Select(d => new { d.TotalCount, d.Rate, d.AdjusmentCount })
                .ToList();
            var orderItemAggregations = orderItems.GroupBy(d => (0)).Select(d => new
            {
                TotalCount = d.Sum(l => l.TotalCount),
                TotalAdjusmetCredit = d.Sum(l => (l.Rate ?? 0 * l.AdjusmentCount ?? 0))
            }).First();

            var newView = new InvoiceListItemView
            {
                Id = model.Id,
                SchoolId = model.School.Id,
                SchoolName = model.School.Name,
                OrderDate = model.OrderDate,
                OrderStatus = view.OrderStatus,
                Rate = model.Rate,
                TotalCredit = model.TotalCredit,
                TotalAdjusmentCredit = orderItemAggregations.TotalAdjusmetCredit,
                TotalCount = (int)orderItemAggregations.TotalCount,
                TotalAmount =
                    ((model.Rate ?? 0) * orderItemAggregations.TotalCount ??
                     0 - (model.TotalCredit ?? 0 + orderItemAggregations.TotalAdjusmetCredit)) + model.DebitAmount,
                DebitAmount = (double)model.DebitAmount,
                Note = model.Note
            };
            return newView;
        }

        public List<OrderReportView> GetOrderReport(OrderReportFilterView filter, int pageSize, int pageIndex,
            string orderByField, bool orderByAsc, out int totalCount)
        {
            var nowDate = DateTime.Now;
            if (filter.OrderStartDate.HasValue)
            {
                nowDate = filter.OrderStartDate.Value;
            }
            //var nowDate = filter.OrderDate.Value;
            var minDate = new DateTime(nowDate.Year, nowDate.Month, 1);
            //var maxDate = new DateTime(nowDate.Year, nowDate.Month, DateTime.DaysInMonth(nowDate.Year, nowDate.Month));
            var orderQuery = Repository.Query<MealMenuOrder>();
            orderQuery = filter.OrderId.HasValue
                ? orderQuery.Where(d => d.Id == filter.OrderId.Value)
                : orderQuery.Where(
                    d =>
                        d.OrderDate == minDate && d.OrderStatus != (long)OrderStatuses.InitialState &&
                        d.RecordStatus == (long)RecordStatuses.Active);

            if (filter.OrderStartDate.HasValue && filter.OrderEndDate.HasValue)
            {
                orderQuery =
                    orderQuery.Where(d => d.MealMenuOrderItems.Any(k => k.MealMenu.ValidDate >= filter.OrderStartDate && k.MealMenu.ValidDate <= filter.OrderEndDate));
            }
            else if (filter.OrderStartDate.HasValue)
            {
                orderQuery =
                    orderQuery.Where(d => d.MealMenuOrderItems.Any(k => k.MealMenu.ValidDate >= filter.OrderStartDate));
            }
            else if (filter.OrderEndDate.HasValue)
            {
                orderQuery =
                    orderQuery.Where(d => d.MealMenuOrderItems.Any(k => k.MealMenu.ValidDate <= filter.OrderEndDate));
            }
            if (filter.MealTypeId > 0)
            {
                orderQuery = orderQuery.Where(d => d.MealType == filter.MealTypeId);
            }
            if (filter.SchoolTypeId > 0)
                orderQuery = orderQuery.Where(d => d.School.SchoolType == filter.SchoolTypeId);

            if (!string.IsNullOrWhiteSpace(filter.SchoolNameStartsWith))
                orderQuery =
                    orderQuery.Where(c => c.School.Name.ToLower().Contains(filter.SchoolNameStartsWith.ToLower()));


            var orderSelect = orderQuery.Select(d => new
            {
                d.Id,
                SchoolId = d.School.Id,
                SchoolName = d.School.Name,
                SchoolCode = d.School.Code,
                MealTypeId = d.MealType,
                SchoolTypeId = d.School.SchoolType,

                d.School.OVSType,
                d.School.FoodServiceType,
                d.TotalCredit,
                d.Rate,
                d.OrderDate
            });

            switch (orderByField)
            {
                case "Id":
                    orderSelect = orderByAsc ? orderSelect.OrderBy(c => c.Id) : orderSelect.OrderByDescending(c => c.Id);
                    break;
                case "SchoolName":
                    orderSelect = orderByAsc
                        ? orderSelect.OrderBy(c => c.SchoolName)
                        : orderSelect.OrderByDescending(c => c.SchoolName);
                    break;
                default:
                    orderSelect = orderByAsc ? orderSelect.OrderBy(c => c.Id) : orderSelect.OrderByDescending(c => c.Id);
                    break;
            }

            totalCount = orderSelect.Count();
            if (pageSize > 0) orderSelect = orderSelect.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var orders = orderSelect.ToList();
            var orderIds = orders.Select(d => d.Id).ToArray();
            var orderSchoolIds = orders.Select(d => d.SchoolId).ToArray();

            var schoolRoutes = Repository.Query<SchoolRoute>().
                Where(
                    d =>
                        orderSchoolIds.Contains(d.School.Id) && d.MealType == filter.MealTypeId &&
                        d.RecordStatus == (long)SysMngConfig.RecordStatuses.Active)
                .Select(
                    d =>
                        new
                        {
                            SchoolId = d.School.Id,
                            Route = string.IsNullOrWhiteSpace(d.Route) ? 0 : int.Parse(d.Route)
                        })
                .ToList();

            var orderItemQuery = Repository.Query<MealMenuOrderItem>()
                .Where(d => orderIds.Contains(d.MealMenuOrder.Id) && d.RecordStatus == (long)RecordStatuses.Active && d.MealMenu.RecordStatus == (long)RecordStatuses.Active);

            var orderDayList = Repository.Query<MealMenuOrderDay>()
                .Where(d => orderIds.Contains(d.MealMenuOrder.Id)).Select(d => new { d.DeliveryType, d.FruitCount, d.VegetableCount, d.ValidDate, OrderId = d.MealMenuOrder.Id }).ToList();

            if (!filter.OrderId.HasValue)
                orderItemQuery =
                    orderItemQuery.Where(
                        d =>
                            d.MealMenu.ValidDate >= filter.OrderStartDate && d.MealMenu.ValidDate <= filter.OrderEndDate);
            var orderItemData = orderItemQuery.Select(d => new
            {
                OrderId = d.MealMenuOrder.Id,
                d.RefId,
                d.TotalCount,
                OrderItemId = d.Id,
                MealMenuServiceType = d.MealServiceType,
                d.MealMenu.ValidDate,
                MenuTypeId = d.MealMenu.Menu.MenuType,
                MenuName = d.MealMenu.Menu.Name,
                d.AdjusmentCount,
                AdjusmentRate = d.Rate,
                d.ModifiedReason,

            })
                .ToList();

            var fruitVegData = orderDayList.Select(d => new
            {
                d.FruitCount,
                d.VegetableCount,
            })
               .ToList();

            var queryResult = new List<OrderReportView>();
            orders.ForEach(d =>
            {
                var schoolRoute = schoolRoutes.FirstOrDefault(k => k.SchoolId == d.SchoolId);
                var orderReport = new OrderReportView
                {
                    SchoolId = d.SchoolId,
                    SchoolName = d.SchoolName,
                    SchoolCode = d.SchoolCode,
                    SchoolRoute = schoolRoute == null ? 0 : schoolRoute.Route,
                    SchoolType = SysMngConfig.Lookups.GetItem<SysMngConfig.SchoolTypes>(d.SchoolTypeId).Text,
                    SchoolTypeId = d.SchoolTypeId,
                    FoodServiceType = SysMngConfig.Lookups.GetItem<SysMngConfig.FoodServiceType>(d.FoodServiceType).Text,
                    OVSType = SysMngConfig.Lookups.GetItem<SysMngConfig.OVSType>(d.OVSType).Text,
                    MealTypeId = d.MealTypeId,
                    OrderDate = d.OrderDate,
                    OrderId = d.Id,
                    TotalCredit = d.TotalCredit,
                    Rate = d.Rate,
                    Items = new List<OrderReportItemView>(),
                };
                orderReport.Items =
                    orderItemData.Where(k => k.OrderId == d.Id).GroupBy(s => new { s.ValidDate })
                        .Select(s => new OrderReportItemView
                        {
                            Date = s.Key.ValidDate,
                            DeliveryType = orderDayList.Any(vday => vday.OrderId == d.Id && vday.ValidDate == s.Key.ValidDate) ?
                                orderDayList.First(vday => vday.OrderId == d.Id && vday.ValidDate == s.Key.ValidDate).DeliveryType :
                                (long)DeliveryTypes.Breakfast,
                            Menus = s.Select(t => new OrderReportMenuView
                            {
                                Id = t.OrderItemId,
                                Name = t.MenuName,
                                MenuTypeId = t.MenuTypeId,
                                RefId = t.RefId,
                                TotalCount = t.TotalCount ?? 0,
                                AdjusmentCount =
                                    t.AdjusmentCount ?? 0,
                                Rate = t.AdjusmentRate ?? 0,
                                ModifiedReason = t.ModifiedReason,
                                MealServiceType =
                                    Lookups.MealServiceTypeList.FirstOrDefault(k => k.Id == t.MealMenuServiceType),

                                //fruitVeg = s.Select(fv => new MealMenuOrderDay
                                //{
                                //    FruitCount = orderDayList.Where(x => x.OrderId == t.OrderId).FirstOrDefault().FruitCount.Value,
                                //    VegetableCount = orderDayList.Where(x => x.OrderId == t.OrderId).FirstOrDefault().VegetableCount.Value,
                                //}).ToList()

                                ////fruitVeg = s.Select(d=> new MealMenuOrderDay { }).ToList()
                                fruitCount = orderDayList.Where(x => x.OrderId == t.OrderId && x.ValidDate == t.ValidDate).FirstOrDefault().FruitCount.Value,
                                vegetableCount = orderDayList.Where(x => x.OrderId == t.OrderId && x.ValidDate == t.ValidDate).LastOrDefault().VegetableCount.Value

                            }).ToList()
                        }).ToList();

                queryResult.Add(orderReport);
            });

            return queryResult;

        }

        public List<DailyChangesItemView> GetDailyChanges(DailyChangesFilterView filter, int pageSize, int pageIndex,
            string orderByField, bool orderByAsc, out int totalCount)
        {
            var query = from mealMenu in Repository.Query<MealMenu>()
                        join orderItem in Repository.Query<MealMenuOrderItem>()
                            on mealMenu.Id equals orderItem.MealMenu.Id
                        where
                            mealMenu.ValidDate.Date == filter.OrderItemDate.Value.Date &&
                            mealMenu.MealType == filter.MealTypeId &&
                            orderItem.RefId.Value > 0 &&
                            orderItem.Version > 0 &&
                                    orderItem.RecordStatus == (long)RecordStatuses.Active &&
                                    mealMenu.RecordStatus == (long)RecordStatuses.Active
                        select new
                        {
                            MenuId = mealMenu.Menu.Id,
                            MenuName = mealMenu.Menu.Name,
                            MenuTypeId = mealMenu.Menu.MenuType,
                            NewCount = orderItem.TotalCount ?? 0,
                            Version = orderItem.Version,
                            Notes = orderItem.ModifiedReason,
                            OrderItemRefId = orderItem.RefId,
                            SchoolCode = orderItem.MealMenuOrder.School.Code,
                            SchoolName = orderItem.MealMenuOrder.School.Name,
                            SchoolId = orderItem.MealMenuOrder.School.Id,
                            SchoolTypeId = orderItem.MealMenuOrder.School.SchoolType
                        };

            switch (orderByField)
            {
                case "MenuName":
                    query = orderByAsc ? query.OrderBy(c => c.MenuName) : query.OrderByDescending(c => c.MenuName);
                    break;
                case "SchoolName":
                    query = orderByAsc ? query.OrderBy(c => c.SchoolName) : query.OrderByDescending(c => c.SchoolName);
                    break;
                default:
                    query = orderByAsc ? query.OrderBy(c => c.SchoolName) : query.OrderByDescending(c => c.SchoolName);
                    break;
            }

            totalCount = query.Count();
            if (pageSize > 0) query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var result = query.ToList();

            var ids = result.Select(d => string.Format("{0}_{1}", d.OrderItemRefId, (d.Version - 1))).ToArray();
            var prevOrderItems =
                Repository.Query<MealMenuOrderItem>().Where(
                    d =>
                        d.RefId.HasValue &&
                        ids.Contains(d.RefId.ToString() + "_" + d.Version.ToString(CultureInfo.InvariantCulture)))
                    //d => ids.Contains(string.Format("{0}_{1}",d.RefId,d.Version)))
                    .Select(d => new { d.RefId, d.Version, d.TotalCount }).ToList();

            return (from orderItem in result
                    join prevOrderItem in prevOrderItems
                        on orderItem.OrderItemRefId equals prevOrderItem.RefId
                    select new DailyChangesItemView
                    {
                        PreviousCount = prevOrderItem.TotalCount ?? 0,
                        NewCount = orderItem.NewCount,
                        MenuName = orderItem.MenuName,
                        MenuId = orderItem.MenuId,
                        MenuType = Lookups.GetItem<MenuTypes>(orderItem.MenuTypeId),
                        Notes = orderItem.Notes,
                        OrderItemRefId = orderItem.OrderItemRefId,
                        SchoolCode = orderItem.SchoolCode,
                        SchoolId = orderItem.SchoolId,
                        SchoolName = orderItem.SchoolName,
                        //SchoolRoute = orderItem.SchoolRoute,
                        Version = orderItem.Version,
                        SchoolType = SysMngConfig.Lookups.GetItem<SysMngConfig.SchoolTypes>(orderItem.SchoolTypeId).Text
                    }).ToList();
        }

        public List<InvoiceSummaryView> GetInvoicesSummaryByFilter(InvoiceFilterView filter, int pageSize, int pageIndex,
            string orderByField, bool orderByAsc, out int totalCount)
        {

            var annualYear = DateTime.Now.Year;
            var menuTypes =
                Lookups.MenuTypeList.Where(d => d.Id != (long)MenuTypes.Milk).Select(d => d.Id).ToList();


            if (filter.RecordStatusId == 0)
                filter.RecordStatusId = (long)RecordStatuses.Active;
            var query = Repository.Query<MealMenuOrder>().Where(d => d.RecordStatus == filter.RecordStatusId);

            if (filter.OrderDate.HasValue)
            {
                query = query.Where(d => d.OrderDate.Year == filter.OrderDate.Value.Year);
                annualYear = filter.OrderDate.Value.Year;
            }
            if (filter.SchoolId > 0)
                query = query.Where(d => d.School.Id == filter.SchoolId);

            if (filter.MealTypeId > 0)
                query = query.Where(d => d.MealType == filter.MealTypeId);

            var querySelect = query.Select(d => new
            {
                d.Id,
                d.OrderDate,
                OrderStatusId = d.OrderStatus,
                SchoolId = d.School.Id,
                SchoolName = d.School.Name,
                SchoolTypeId = d.School.SchoolType,
                RecordStatusId = d.RecordStatus,
                d.Rate,
                d.TotalCredit,
                d.DebitAmount
            });

            totalCount = querySelect.Count();

            switch (orderByField)
            {
                case "Id":
                    querySelect = orderByAsc ? querySelect.OrderBy(c => c.Id) : querySelect.OrderByDescending(c => c.Id);
                    break;
                case "SchoolName":
                    querySelect = orderByAsc
                        ? querySelect.OrderBy(c => c.SchoolName)
                        : querySelect.OrderByDescending(c => c.SchoolName);
                    break;
                case "OrderStatus":
                    querySelect = orderByAsc
                        ? querySelect.OrderBy(c => c.OrderStatusId)
                        : querySelect.OrderByDescending(c => c.OrderStatusId);
                    break;
                case "TotalCredit":
                    querySelect = orderByAsc
                        ? querySelect.OrderBy(c => c.TotalCredit.Value)
                        : querySelect.OrderByDescending(c => c.TotalCredit.Value);
                    break;
                default:
                    querySelect = orderByAsc
                        ? querySelect.OrderBy(c => new { c.Id })
                        : querySelect.OrderByDescending(c => new { c.Id });
                    break;
            }

            if (pageSize > 0) querySelect = querySelect.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var invoices = querySelect.AsEnumerable().ToList();

            var invoicesId = invoices.Select(d => d.Id).ToArray();
            /*
            var invoiceTotalCounts = Repository.Query<MealMenuOrderItem>()
                .Where(d => invoicesId.Contains(d.MealMenuOrder.Id) &&
                            d.RecordStatus == (long)RecordStatuses.Active &&
                            menuTypes.Contains(d.MealMenu.Menu.MenuType))
                .GroupBy(d => d.MealMenuOrder.Id)
                .Select(d => new
                {
                    OrderId = d.Key, 
                    TotalCount = d.Sum(l => l.TotalCount)                 
                })
                .ToList();

            */
            var invoiceTotalCounts = Repository.Query<MealMenuOrderItem>()
                .Where(d => invoicesId.Contains(d.MealMenuOrder.Id) &&
                            d.RecordStatus == (long)RecordStatuses.Active &&
                            menuTypes.Contains(d.MealMenu.Menu.MenuType) &&
                            d.MealMenu.RecordStatus == (long)RecordStatuses.Active)
                .Select(d => new
                {
                    MealMenuOrderId = d.MealMenuOrder.Id,
                    TotalCount = (d.MealMenu.Menu.MenuType != (long)MenuTypes.SoyMilk ? d.TotalCount : 0),
                    SoyMilkCount = (d.MealMenu.Menu.MenuType == (long)MenuTypes.SoyMilk ? d.TotalCount : 0),
                    d.AdjusmentCount,
                    d.Rate
                })
                .AsEnumerable()
                .GroupBy(d => d.MealMenuOrderId)
                .Select(d => new
                {
                    OrderId = d.Key,
                    TotalCount = d.Sum(l => l.TotalCount),
                    SoyMilkCount = d.Sum(l => l.SoyMilkCount),
                    TotalAdjusmentCredit = d.Sum(l => (l.AdjusmentCount ?? 0) * (l.Rate ?? 0))
                })
                .ToList();

            var schoolIdList = invoices.Select(i => i.SchoolId).Distinct().ToList();
            var schoolInvoiceDocumentList = (from schoolInvoiceDocument in Repository.Query<SchoolInvoiceDocument>()
                                             where
                                                 schoolIdList.Contains(schoolInvoiceDocument.SchoolId)
                                                                              && schoolInvoiceDocument.RecordStatus == (long)RecordStatuses.Active
                                                 && schoolInvoiceDocument.InvoiceYear == filter.OrderDate.Value.Year
                                             select
                                                 new
                                                 {
                                                     schoolInvoiceDocument.DocumentGuid,
                                                     schoolInvoiceDocument.SchoolId,
                                                     schoolInvoiceDocument.InvoiceYear,
                                                     schoolInvoiceDocument.InvoiceMonth
                                                 }).ToList();

            var schoolSoyMilkRates = Repository.Query<SchoolAnnualAgreement>()
                .Where(d => schoolIdList.Contains(d.School.Id) && d.ItemType == (long)AnnualItemTypes.SoyMilk && d.Year == annualYear)
                .Select(d => new { d.Price, SchoolId = d.School.Id })
                .ToList();

            var invoiceResult = (from invoice in invoices
                                 join invoiceTotalCount in invoiceTotalCounts on invoice.Id equals invoiceTotalCount.OrderId
                                 join schoolSoyMilkRate in schoolSoyMilkRates on invoice.SchoolId equals schoolSoyMilkRate.SchoolId
                                  into subSoyMilkRate
                                 from soyMilkRate in subSoyMilkRate.DefaultIfEmpty()
                                 select new
                                 {
                                     invoice.Id,
                                     invoice.OrderDate,
                                     invoice.OrderStatusId,
                                     invoice.SchoolId,
                                     invoice.SchoolName,
                                     invoice.RecordStatusId,
                                     invoice.SchoolTypeId,
                                     invoice.Rate,
                                     invoice.TotalCredit,
                                     TotalCount = invoiceTotalCount.TotalCount ?? 0,
                                     SoyMilkCount = invoiceTotalCount.SoyMilkCount ?? 0,
                                     SoyMilkRate = soyMilkRate != null ? soyMilkRate.Price : 0,

                                     TotalAmount = (invoiceTotalCount.TotalCount ?? 0) * (invoice.Rate ?? 0)
                                                   + (invoiceTotalCount.SoyMilkCount ?? 0) * (soyMilkRate != null ? soyMilkRate.Price : 0)
                                                   - (invoice.TotalCredit ?? 0)
                                                   + invoiceTotalCount.TotalAdjusmentCredit + (invoice.DebitAmount ?? 0),
                                 }).GroupBy(d => new { d.SchoolId, d.SchoolName, d.SchoolTypeId })
                .Select(d => new InvoiceSummaryView
                {
                    SchoolName = d.Key.SchoolName,
                    SchoolId = d.Key.SchoolId,
                    SchoolType = SysMngConfig.Lookups.GetItem<SysMngConfig.SchoolTypes>(d.Key.SchoolTypeId).Text,
                    TotalAmount = d.Sum(k => k.TotalAmount),
                    TotalCount = d.Sum(k => k.TotalCount),
                    SoyMilkCount = d.Sum(k => k.SoyMilkCount),
                    InvoiceItems = d.Select(k => new InvoiceSummaryMonthView
                    {
                        Id = k.Id,
                        Month = k.OrderDate.Month,
                        OrderDate = k.OrderDate,
                        OrderStatus = Lookups.GetItem<OrderStatuses>(k.OrderStatusId),
                        Rate = k.Rate,
                        TotalCount = k.TotalCount,
                        SoyMilkRate = k.SoyMilkRate,
                        SoyMilkCount = k.SoyMilkCount,
                        TotalAmount = k.TotalAmount,
                        TotalCredit = k.TotalCredit,
                        SchoolInvoiceDocumentGuid = schoolInvoiceDocumentList.Where(doc =>
                            doc.SchoolId == k.SchoolId && doc.InvoiceYear == k.OrderDate.Year && doc.InvoiceMonth == k.OrderDate.Month)
                            .Select(doc => doc.DocumentGuid)
                            .FirstOrDefault()
                    }).OrderBy(k => k.Month).ToList()
                }).ToList();

            return invoiceResult;
        }

        public void SaveSchoolInvoiceDocument(SchoolInvoiceDocumentView schoolInvoiceView)
        {
            var existingSchoolInvoice = Repository.Query<SchoolInvoiceDocument>().Where(i =>
                i.SchoolId == schoolInvoiceView.SchoolId
                && i.InvoiceYear == schoolInvoiceView.InvoiceYear
                && i.InvoiceMonth == schoolInvoiceView.InvoiceMonth
                && i.RecordStatus == (long)RecordStatuses.Active
                ).ToList();

            foreach (var i in existingSchoolInvoice)
            {
                i.RecordStatus = (long)RecordStatuses.InActive;
                i.ModifiedAt = DateTime.Now;
                i.ModifiedBy = schoolInvoiceView.ModifiedBy;
                i.ModifiedByFullName = schoolInvoiceView.ModifiedByFullName;
                Repository.Update(i);
            }
            schoolInvoiceView.CreatedBy = schoolInvoiceView.ModifiedBy;
            schoolInvoiceView.CreatedByFullName = schoolInvoiceView.ModifiedByFullName;
            schoolInvoiceView.CreatedAt = DateTime.Now;
            schoolInvoiceView.ModifiedAt = DateTime.Now;
            schoolInvoiceView.ModifiedBy = schoolInvoiceView.ModifiedBy;
            schoolInvoiceView.ModifiedByFullName = schoolInvoiceView.ModifiedByFullName;
            schoolInvoiceView.RecordStatus =
                Lookups.RecordStatusList.FirstOrDefault(k => k.Id == (long)RecordStatuses.Active);


            int? version = Repository.Query<SchoolInvoiceDocument>().Where(i =>
                i.SchoolId == schoolInvoiceView.SchoolId
                && i.InvoiceYear == schoolInvoiceView.InvoiceYear
                && i.InvoiceMonth == schoolInvoiceView.InvoiceMonth)
                .OrderByDescending(i => i.Version).Select(i => i.Version).FirstOrDefault();
            schoolInvoiceView.Version = ((int)version) + 1;

            var schoolInvoice = schoolInvoiceView.ToModel();
            Repository.Update(schoolInvoice);
        }

        public SchoolInvoiceDocumentView GetSchoolInvoiceDocumentById(Guid id)
        {
            return
                Repository.Query<SchoolInvoiceDocument>()
                    .FirstOrDefault(d => d.DocumentGuid == id)
                    .ToView<SchoolInvoiceDocumentView>();
        }

        public MealOrderManageDayView SaveOrderForDay(MealOrderManageDayView day)
        {
            var milkCount = day.Items.Where(x => x.IsMilk).Select(x => x.Count).Sum();
            var otherCount = day.Items.Where(x => !x.IsMilk && !x.IsSoyMilk).Select(x => x.Count).Sum();
            if (milkCount > otherCount)
                throw new WarningException(
                    "The number of milk cannot be greater than the number of menu.");
            long orderId = 0;
            var dtNow = DateTime.Now;
            day.Items.ForEach(x =>
            {
                var y = new MealMenuOrderItemView
                {
                    Id = x.Id,
                    MealMenuId = x.MealMenuId,
                    MealMenuValidDate = day.Date,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedAt = dtNow,
                    ModifiedReason = x.ModifiedReason,
                    ModifiedByFullName = x.ModifiedByFullName,
                    TotalCount = x.Count,
                    MealServiceType = new GeneralItemView { Id = x.MealServiceTypeId },
                    MealType = new GeneralItemView { Id = x.MealTypeId }
                };
                var savedOrderItem = SaveOrderItem(y, day.SchoolId, day.OrderRate);
                x.Id = savedOrderItem.Id;
                orderId = savedOrderItem.MealMenuOrderId;
            });
            var mealMenuOrderDay = Repository.Query<MealMenuOrderDay>()
                .FirstOrDefault(x => x.ValidDate == day.Date &&
                                    x.MealMenuOrder.Id == orderId
                                     && x.MealMenuOrder.MealType == day.MealTypeId);


            if (mealMenuOrderDay != null)
            {
                mealMenuOrderDay.DeliveryType = day.DeliveryTypeId;
                mealMenuOrderDay.FruitCount = day.FruitCount;
                mealMenuOrderDay.VegetableCount = day.VegetableCount;
                Repository.Update(mealMenuOrderDay);
            }
            else
            {
                mealMenuOrderDay = new MealMenuOrderDay
                {
                    DeliveryType = day.DeliveryTypeId,
                    FruitCount = day.FruitCount,
                    VegetableCount = day.VegetableCount,
                    MealMenuOrder = new MealMenuOrder { Id = orderId },
                    ValidDate = day.Date
                };
                Repository.Create(mealMenuOrderDay);
            }
            return day;
        }

        public List<MealMenuSupplementaryView> GetSupplementaryList(GetSupplementaryListFilterView filter)
        {
            var query = Repository.Query<MealMenuSupplementary>().Where(
                x => x.School.Id == filter.SchoolId && x.MealType == filter.MealTypeId);

            var result = query.Select(
                x => new MealMenuSupplementaryView
                {
                    Id = x.Id,
                    SchoolId = x.School.Id,
                    MenuId = x.Menu.Id,
                    MealType = x.MealType,
                    Monday = x.Monday,
                    Tuesday = x.Tuesday,
                    Wednesday = x.Wednesday,
                    Thursday = x.Thursday,
                    Friday = x.Friday
                }).ToList();
            Repository.Query<Menu>().Where(x => x.MenuType == (long)MenuTypes.Milk)
                .ToList().ForEach(m =>
                {
                    if (m.RecordStatus == (int)RecordStatuses.Active && result.All(k => k.MenuId != m.Id))
                        result.Add(new MealMenuSupplementaryView
                        {
                            Id = 0,
                            SchoolId = filter.SchoolId,
                            MenuId = m.Id,
                            MealType = filter.MealTypeId,
                            Monday = 0,
                            Tuesday = 0,
                            Wednesday = 0,
                            Thursday = 0,
                            Friday = 0
                        });
                    else if (m.RecordStatus == (int)RecordStatuses.Deleted)
                    {
                        var deletedMenu = result.FirstOrDefault(d => d.MenuId == m.Id);
                        if (deletedMenu != null)
                            result.Remove(deletedMenu);
                    }
                });
            result = result.OrderBy(x => x.MenuId).ToList();
            return result;
        }

        public void SaveSupplementaryList(List<MealMenuSupplementaryView> list)
        {
            if (list == null) throw new ArgumentNullException("list");
            var invalidDays = new List<string>();
            if (list.Sum(x => x.Monday) > 100) invalidDays.Add(string.Format("Monday[Total: %{0}]", list.Sum(x => x.Monday)));
            if (list.Sum(x => x.Tuesday) > 100) invalidDays.Add(string.Format("Tuesday[Total: %{0}]", list.Sum(x => x.Tuesday)));
            if (list.Sum(x => x.Wednesday) > 100) invalidDays.Add(string.Format("Wednesday[Total: %{0}]", list.Sum(x => x.Wednesday)));
            if (list.Sum(x => x.Thursday) > 100) invalidDays.Add(string.Format("Thursday[Total: %{0}]", list.Sum(x => x.Thursday)));
            if (list.Sum(x => x.Friday) > 100) invalidDays.Add(string.Format("Friday[Total: %{0}]", list.Sum(x => x.Friday)));

            if (invalidDays.Any())
            {
                var validationMessage =
                    string.Format("Supplied percentage values for {0} is invalid. The total can be maximum %100 per day.",
                        string.Join(",", invalidDays.ToArray()));
                throw new Exception(validationMessage);
            }

            list.Where(x => x.Id == 0)
                .Select(x => new MealMenuSupplementary
                {
                    Menu = new Menu { Id = x.MenuId },
                    School = new School { Id = x.SchoolId },
                    MealType = x.MealType,
                    Monday = x.Monday,
                    Tuesday = x.Tuesday,
                    Wednesday = x.Wednesday,
                    Thursday = x.Thursday,
                    Friday = x.Friday
                }).ToList().ForEach(Repository.Create);
            list.Where(x => x.Id != 0).ToList().ForEach(x =>
            {
                var entity = Repository.GetById<MealMenuSupplementary>(x.Id);
                if (entity.Menu.Id != x.MenuId)
                    entity.Menu = new Menu { Id = x.MenuId };
                if (entity.MealType != x.MealType)
                    entity.MealType = x.MealType;
                if (entity.School.Id != x.SchoolId)
                    entity.School = new School { Id = x.SchoolId };
                if (entity.Monday != x.Monday)
                    entity.Monday = x.Monday;
                if (entity.Tuesday != x.Tuesday)
                    entity.Tuesday = x.Tuesday;
                if (entity.Wednesday != x.Wednesday)
                    entity.Wednesday = x.Wednesday;
                if (entity.Thursday != x.Thursday)
                    entity.Thursday = x.Thursday;
                if (x.Friday != entity.Friday)
                    entity.Friday = x.Friday;
                Repository.Update(entity);
            });
        }
    }
}