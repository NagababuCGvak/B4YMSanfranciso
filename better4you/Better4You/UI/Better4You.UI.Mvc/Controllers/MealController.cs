using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Better4You.Meal.Config;
using Better4You.Meal.Service;
using Better4You.Meal.Service.Messages;
using Better4You.Meal.ViewModel;
using Better4You.UI.Mvc.Models;
using Tar.ViewModel;

namespace Better4You.UI.Mvc.Controllers
{
    public class MealController : ControllerBase
    {
        public IMenuService MenuService { get; set; }
        public IMealMenuService MealMenuService { get; set; }

        //
        // GET: /Meal/
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.MealTypes = Lookups.GetItems<MealTypes>().Where(d=>d.Id>0).ToList();
            return View();
        }
        [HttpGet]
        public ActionResult Monthly()
        {
            ViewBag.MealTypes = Lookups.GetItems<MealTypes>().Where(d => d.Id > 0).ToList();
            return View();
        }
        [HttpPost]
        public JsonResult Menus(int mealTypeId, string name)
        {
            var request = new MenuGetAllRequest
            {
                Filter = new MenuFilterView { RecordStatusId = (int)RecordStatuses.Active, MealTypeId = mealTypeId, Name = name },
                OrderByAsc = true,
                OrderByField = "Name",
                PageIndex = 1,
                PageSize = 50
            };
            var response = MenuService.GetAllByFilter(request);
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult MealMenus(MealMenuGetAllRequest request)
        {
            request.Filter.RecordStatusId = (int) RecordStatuses.Active;
            return Json(MealMenuService.GetAllByFilter(request), JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public JsonResult MealMenusByType(MealMenuGetAllRequest request)
        {
            request.Filter.RecordStatusId = (int) RecordStatuses.Active;
            var response = MealMenuService.GetAllByFilter(request);
            var list = response.MealMenus.GroupBy(x => new { x.ValidDate, x.Menu.MenuType.Id, x.Menu.MenuType.Text },
                view => view).Select(x =>
                    new
                    {
                        x.Key.ValidDate,
                        MenuTypeId = x.Key.Id,
                        MenuTypeName = x.Key.Text,
                        Count = x.Count()
                    });
            var result = new {List = list, Result = 1};
            return Json(result , JsonRequestBehavior.DenyGet);
        }
        

        [HttpPost]
        public JsonResult SaveMealMenus(MealMenuSaveRequest request)
        {
            if (request != null)
            {
                var now = DateTime.Now;
                request.MealMenus.ForEach(d =>
                {
                    d.ModifiedBy = CurrentUser.Name;
                    d.ModifiedAt = now;
                    d.ModifiedByFullName = CurrentUser.FullName;
                    d.CreatedAt = d.CreatedAt == DateTime.MinValue ? now : d.CreatedAt;
                });
            }
            return Json(MealMenuService.Save(request), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult SaveMealMenus2(MealMenuSaveRequest request, int mealTypeId, int year, int month, int day)
        {
            if (request != null)
            {
                var now = DateTime.Now;
                request.MealMenus.ForEach(d =>
                {
                    d.MealType = new GeneralItemView(mealTypeId, "", "");
                    d.ModifiedBy = CurrentUser.Name;
                    d.ModifiedAt = now;
                    d.ModifiedByFullName = CurrentUser.FullName;
                    d.CreatedAt = d.CreatedAt == DateTime.MinValue ? now : d.CreatedAt;
                    d.ValidDate = d.ValidDate == DateTime.MinValue ? new DateTime(year, month, day) : d.ValidDate;
                });
            }
            return Json(MealMenuService.Save(request), JsonRequestBehavior.DenyGet);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var response = MealMenuService.Get(new MealMenuGetRequest {Id = id});
            return View(response.MealMenu);
        }

        [HttpPost]
        public JsonResult Delete(MealMenuView mealMenu)
        {
            mealMenu.ModifiedBy = CurrentUser.Name;
            mealMenu.ModifiedByFullName = CurrentUser.FullName;
            return Json(MealMenuService.Delete(new MealMenuDeleteRequest{MealMenu = mealMenu}),JsonRequestBehavior.DenyGet);
        }


        [HttpGet] //{mealType}/{year}/{month}/{day}
        public ActionResult Daily(int year, int month, int day, int mealType)
        {
            var model = new MealDailyViewModel
            {
                Year = year,
                Month = month,
                Day = day,
                MealType = mealType,
                //MealTypes = Lookups.GetItems<MealTypes>().Where(x => x.Id != 0).ToList(),
                MealMenus = new List<MealMenuListItemView>()
            };
            return View(model);
        }
    }
}
