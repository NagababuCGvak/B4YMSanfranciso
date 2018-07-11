using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Better4You.Meal.Config;
using Better4You.Meal.Service;
using Better4You.Meal.Service.Messages;
using Better4You.Meal.ViewModel;
using Better4You.Service;
using NHibernate.Linq;
using Tar.Service.Messages;
using Tar.ViewModel;


namespace Better4You.UI.Mvc.Controllers
{
    public class MenuController : ControllerBase
    {

        public ILookupService LookupService { get; set; }
        public IMenuService MenuService { get; set; }
        public IFoodService FoodService { get; set; }
        public IMealMenuService MealMenuService { get; set; }
        //
        // GET: /Menu/

        public ActionResult Index()
        {

            //ViewBag.MenuTypes = Lookups.GetItems<MenuTypes>();
            ViewBag.MenuTypes = Lookups.MenuTypeList;
            ViewBag.RecordStatuses = Lookups.GetItems<RecordStatuses>();

            var request = new MenuGetAllRequest
            {
                Filter = new MenuFilterView { RecordStatusId = (int)RecordStatuses.Active },
                OrderByAsc = true,
                OrderByField = "Name",
                PageIndex = 1,
                PageSize = 20000
            };
            TryUpdateModel(request.Filter);
            var result = MenuService.GetAllByFilter(request);
            return View(result.Menus);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {

            var menu = id.HasValue
                           ? MenuService.Get(new MenuGetRequest { Id = id.Value }).Menu
                           : new MenuView { MenuType = Lookups.GetItem<MenuTypes>((long)MenuTypes.None), SchoolType = Lookups.GetItem<MenuSchoolTypes>((long)MenuSchoolTypes.None) };


            //ViewBag.MenuTypes = Lookups.GetItems<MenuTypes>().Select(d=>new SelectListItem{Value = d.Id.ToString(),Text = d.Text,Selected = d.Id==menu.MenuType.Id}).ToList();
            ViewBag.MenuTypes = Lookups.MenuTypeList.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Text, Selected = d.Id == menu.MenuType.Id }).ToList();
            ViewBag.MenuSchoolTypes = Lookups.GetItems<MenuSchoolTypes>().Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Text, Selected = d.Id == menu.SchoolType.Id }).ToList();
            return View(menu);
        }
        [HttpPost]
        public ActionResult Edit(MenuView request)
        {
            var menuTypeId = (long)MenuTypes.None;
            if (!string.IsNullOrWhiteSpace(Request["MenuTypeId"]))
                menuTypeId = long.Parse(Request["MenuTypeId"]);

            request.MenuType =Lookups.GetItem<MenuTypes>(menuTypeId) ;

            var schoolTypeId = (long)MenuSchoolTypes.None;
            if (!string.IsNullOrWhiteSpace(Request["SchoolTypeId"]))
                schoolTypeId = long.Parse(Request["SchoolTypeId"]);

            request.SchoolType = Lookups.GetItem<MenuSchoolTypes>(schoolTypeId);

            //TryUpdateModel(request);

            request.Foods = new List<FoodListItemView>();
            Request.Form.AllKeys.Where(d => d.StartsWith("Foods")).ForEach(d => request.Schools.Add(new GeneralItemView { Id = long.Parse(Request[d]) }));
                      

            request.Schools = new List<GeneralItemView>();
            Request.Form.AllKeys.Where(d => d.StartsWith("Schools")).ForEach(d => request.Schools.Add(new GeneralItemView { Id = long.Parse(Request[d]) }));
            

            if (menuTypeId > 0 && (!string.IsNullOrWhiteSpace(request.Name)))
            {
                request.ModifiedBy = CurrentUser.Name;
                request.ModifiedByFullName = CurrentUser.FullName;
                if (!ModelState.IsValid) return View(request);
                if (request.Id == 0)
                {
                    var response = MenuService.Create(
                        new MenuCreateRequest
                        {
                            Menu = request
                        });
                    if (response.Result == Result.Success)
                    {
                        InfoMessage = response.Message;
                        return RedirectToAction("Index");
                    }
                    ErrorMessage = response.Message;
                }
                else
                {
                    var response = MenuService.Update(new MenuUpdateRequest
                    {
                        Menu = request
                    });
                    if (response.Result == Result.Success)
                    {
                        InfoMessage = response.Message;
                        return RedirectToAction("Index");
                    }
                    ErrorMessage = response.Message;
                }
                
            }
            else
            {
                ErrorMessage = "Fill Name, Select Menu Type";
            }

            //ViewBag.MenuTypes = Lookups.GetItems<MenuTypes>().Select(d => new SelectListItem {Value = d.Id.ToString(), Text = d.Text, Selected = d.Id == menuTypeId}).ToList();
            ViewBag.MenuTypes = Lookups.MenuTypeList.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Text, Selected = d.Id == menuTypeId }).ToList();
            ViewBag.MenuSchoolTypes = Lookups.GetItems<MenuSchoolTypes>().Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Text, Selected = d.Id == schoolTypeId }).ToList();

            return View(request);

        }

        [HttpPost]
        public ActionResult Delete(MenuView request)
        {
            request.ModifiedBy = CurrentUser.Name;
            request.ModifiedByFullName = CurrentUser.FullName;
            var response = MenuService.Delete(new MenuDeleteRequest { Menu = request });
            if (response.Result == Result.Success)
            {
                InfoMessage = response.Message;
                return RedirectToAction("Index");
            }
            ErrorMessage = response.Message;
            return RedirectToAction("Edit", request);
        }
    }
}
