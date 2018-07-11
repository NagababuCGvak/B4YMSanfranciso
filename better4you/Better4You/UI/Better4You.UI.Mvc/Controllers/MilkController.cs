using System.Linq;
using System.Web.Mvc;
using Better4You.Core;
using Better4You.Meal.Config;
using Better4You.Meal.Service;
using Better4You.Meal.Service.Messages;
using Better4You.Meal.ViewModel;
using Better4You.UI.Mvc.Models;
using Better4You.UserManagment.Service;
using Tar.Core.Extensions;
using RecordStatuses = Better4You.Meal.Config.RecordStatuses;

namespace Better4You.UI.Mvc.Controllers
{
    public class MilkController : ControllerBase
    {
        public IMealMenuOrderService MealMenuOrderService { get; set; }
        public IMenuService MenuService { get; set; }
        public ISchoolService SchoolService { get; set; }

        public ActionResult Percentage(string id)
        {
            long mealTypeId;
            if (string.IsNullOrEmpty(id)
                || !long.TryParse(id, out mealTypeId) || mealTypeId <= 0)
                return RedirectToAction("Percentage", new {id = 1});
            var model = new MilkPercentagePageModel
            {
                MealType = mealTypeId,
                SchoolId = CurrentUser.CurrentSchoolId(),
                Schools = CurrentUser.Schools().ToList(),
                Menus = MenuService.GetAllByFilter(new MenuGetAllRequest
                {
                    Filter = new MenuFilterView
                    {
                        MealTypeId = mealTypeId,
                        MenuTypeId = MenuTypes.Milk.ToInt64(),
                        RecordStatusId = RecordStatuses.Active.ToInt64()
                    },
                    OrderByField = "Id"
                }).Menus.OrderByDescending(y => y.Name).ToList()
        };
            return View(model);
        }

        public JsonResult GetSupplementaryList(GetSupplementaryListRequest request)
        {
            return Json(MealMenuOrderService.GetSupplementaryList(request));
        }

        public JsonResult SaveSupplementaryList(SaveSupplementaryListRequest request)
        {
            return Json(MealMenuOrderService.SaveSupplementaryList(request));
        }
    }
}