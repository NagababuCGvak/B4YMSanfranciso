using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using MealConfig=Better4You.Meal.Config;
using Better4You.Meal.Service;
using Better4You.Meal.Service.Messages;
using Better4You.Meal.ViewModel;
using Better4You.Service;

using Tar.Service.Messages;
using TarView = Tar.ViewModel;



using Better4You.Core;


namespace Better4You.UI.Mvc.Controllers
{
    public class FoodController : ControllerBase
    {
        public ILookupService LookupService { get; set; }
        public IFoodService FoodService { get; set; }
        //
        // GET: /Food/

        public ActionResult Index()
        {
            ViewBag.FoodTypes = MealConfig.Lookups.GetItems<MealConfig.FoodTypes>();
            

            var request = new FoodGetAllRequest
                              {
                                  Filter = new FoodFilterView(),
                                  OrderByAsc = true,
                                  OrderByField = "Name",
                                  PageIndex = 1,
                                  PageSize = 20000
                              };
            TryUpdateModel(request.Filter);
            var result = FoodService.GetAllByFilter(request);
            return View(result.Foods);
        }

        [HttpPost]
        public JsonResult List()
        {
            var request = new FoodGetAllRequest
            {
                Filter = new FoodFilterView(),
                OrderByAsc = true,
                OrderByField = "Name",
                PageIndex = 1,
                PageSize = 20
            };
            TryUpdateModel(request.Filter);
            var result = FoodService.GetAllByFilter(request);
            return Json(result,JsonRequestBehavior.DenyGet);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var mealTypeId = (int)MealConfig.MealTypes.Breakfast;

            if (!string.IsNullOrWhiteSpace(Request["MealTypeId"]))
                mealTypeId = Int32.Parse(Request["MealTypeId"]);


  
            var food = id.HasValue
                           ? FoodService.Get(new FoodGetRequest {Id = id.Value}).Food
                           : new FoodView { FoodType = MealConfig.Lookups.GetItem<MealConfig.FoodTypes>((long)MealConfig.FoodTypes.None) };


            var foodTypes = MealConfig.Lookups.GetItems<MealConfig.FoodTypes>().Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Text,
                Selected = d.Id == food.FoodType.Id
            }).ToList();


            ViewBag.FoodTypes = foodTypes;

            var ingredientTypes = MealConfig.Lookups.GetItems<MealConfig.IngredientTypes>().Where(d => d.Id > 0).ToList(); 

            var existIngredients = food.FoodIngredients.Select(d => d.IngredientType.Id).ToList();

            ingredientTypes.Where(d => existIngredients.Contains(d.Id)).ToList()
                .ForEach(d => food.FoodIngredients
                    .First(k => k.IngredientType.Id == d.Id).IngredientType = d);

            ingredientTypes.Where(d => !existIngredients.Contains(d.Id)).ToList()
                .ForEach(d => food.FoodIngredients.Add(new FoodIngredientView {IngredientType = d}));
            return View(food);
        }

        [HttpPost]
        public ActionResult Edit(FoodView request)
        {


            var foodTypeId = string.IsNullOrWhiteSpace(Request["FoodTypeId"])?0:Convert.ToInt32(Request["FoodTypeId"]);
            request.FoodType = MealConfig.Lookups.GetItem<MealConfig.FoodTypes>(foodTypeId);
            request.RecordStatus = MealConfig.Lookups.GetItem<RecordStatuses>((int)RecordStatuses.Active);
            
            for (int i = 0; i < request.FoodIngredients.Count; i++)
            {
                var ingredientTypeKey = Request[string.Format("FoodIngredients[{0}].IngredientType", i)];
                request.FoodIngredients[i].IngredientType = new TarView.GeneralItemView(Convert.ToInt32(ingredientTypeKey),
                    "", "");
                ModelState.Remove(string.Format("FoodIngredients[{0}].IngredientType", i));
            }


            var foodTypes = MealConfig.Lookups.GetItems<MealConfig.FoodTypes>().Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Text,
                Selected = d.Id == foodTypeId
            }).ToList();

            ViewBag.FoodTypes = foodTypes;

            if (foodTypeId > 0)
            {
                request.ModifiedBy = CurrentUser.Name;
                request.ModifiedByFullName = CurrentUser.FullName;
                if (!ModelState.IsValid) return View(request);
                if (request.Id == 0)
                {
                    var response = FoodService.Create(
                        new FoodCreateRequest
                            {
                                Food = request
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
                    var response = FoodService.Update(new FoodUpdateRequest
                                                          {
                                                              Food = request
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
                ErrorMessage = "Food Type Not Selected";
            }

            var ingredientTypes = MealConfig.Lookups.GetItems<MealConfig.IngredientTypes>().Where(d => d.Id > 0).ToList();

            var existIngredients = request.FoodIngredients.Select(d => d.IngredientType.Id).ToList();
            ingredientTypes.Where(d => !existIngredients.Contains(d.Id)).ToList()
                .ForEach(d => request.FoodIngredients.Add(new FoodIngredientView {IngredientType = d}));


            ingredientTypes.Where(d => existIngredients.Contains(d.Id)).ToList()
                .ForEach(d => request.FoodIngredients
                                  .First(k => k.IngredientType.Id == d.Id).IngredientType = d);
            return View(request);
        }

        [HttpPost]
        public ActionResult Delete(FoodView request)
        {
            var response = FoodService.Delete(new FoodDeleteRequest { Food = request});
            if (response.Result == Result.Success)
            {
                InfoMessage = response.Message;
                return RedirectToAction("Index");
            }
            ErrorMessage = response.Message;
            return RedirectToAction("Edit", request);
        }

        public ActionResult Percentage()
        {
            var percentages = FoodService.GetFoodPercentages(new FoodPercentagesRequest { SchoolId = CurrentUser.CurrentSchoolId() });
            return View(percentages.PercentageList);
        }

        [HttpPost]
        public ActionResult Percentage(int foodPercentageCount)
        {
            var percentages = new List<FoodPercentageView>();
            for (int i = 0; i < foodPercentageCount; i++)
            {
                var mealType = Request[string.Format("FoodPercentages[{0}].MealType", i)];
                int fruitPr=200 ;
                int.TryParse(Request[string.Format("FoodPercentages[{0}].Fruit", i)], out fruitPr);
                int vegPr = 200;
                int.TryParse(Request[string.Format("FoodPercentages[{0}].Vegetable", i)], out vegPr);
                fruitPr = (fruitPr > 200 ? 200 : fruitPr);
                vegPr = (vegPr > 200 ? 200 : vegPr);
                percentages.Add(new FoodPercentageView
                {
                    SchoolId = CurrentUser.CurrentSchoolId() ,
                    Id = Convert.ToInt64(Request[string.Format("FoodPercentages[{0}].Id", i)]),
                    Fruit = fruitPr, Vegetable = vegPr, MealType = new TarView.GeneralItemView(Convert.ToInt32(mealType), "", "") });
            }
            var response = FoodService.SaveFoodPercentages(new FoodPercentagesSaveRequest{PercentageList = percentages});
            
            if (response.Result == Result.Success)
                InfoMessage = response.Message;                
            else ErrorMessage = response.Message;

            return RedirectToAction("Percentage");
        }

    }
}
