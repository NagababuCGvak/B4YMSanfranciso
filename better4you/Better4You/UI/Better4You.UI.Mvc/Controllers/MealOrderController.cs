using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Better4You.Core;
using Better4You.Meal.Config;
using Better4You.Meal.Service;
using Better4You.Meal.Service.Messages;
using Better4You.Meal.ViewModel;
using Better4You.UserManagment.Service;
using Better4You.UserManagment.Service.Messages;
using Tar.Core.Configuration;
using Tar.Core.Extensions;
using Tar.Service.Messages;
using RecordStatuses = Better4You.Meal.Config.RecordStatuses;
using SysMngConfig=Better4You.UserManagement.Config;
using Better4You.Meal.Business.Implementation;
using Better4You.Meal.EntityModel;


namespace Better4You.UI.Mvc.Controllers
{

    public class MealOrderController : ControllerBase
    {
        //
        // GET: /MealOrder/
        public IMealMenuOrderService MealMenuOrderService { get; set; }
        public ISettings Settings { get; set; }
        public IMealMenuService MealMenuService { get; set; }
        public IFoodService FoodService { get; set; }
        public IReportService ReportService {
            get {return ServiceLocator.Get<IReportService>(); }
        }
        public ISchoolService SchoolService { get; set; }
        public ActionResult Index()
        {

            ViewBag.MealTypes = Lookups.GetItems<MealTypes>();
            return View();
        }
        public ActionResult Manage(MealOrderManageView model)
        {
            if (model.MealTypeId == 0) model.MealTypeId = MealTypes.Breakfast.ToInt64();
            
            if (model.SchoolId == 0)
                model.SchoolId = CurrentUser.CurrentSchoolId();
            else
                model.FromOutSide = true;


            var schoolResponse = SchoolService.Get(new SchoolGetRequest { Id = model.SchoolId });
            long schoolType=0 ;
            if (schoolResponse.Result == Result.Success)
            {
                
                schoolType = schoolResponse.School.SchoolType;
                model.SchoolType = schoolType;
                model.SchoolName = schoolResponse.School.Name;
            }

            var isAllow = (model.FromOutSide && CurrentUser.IsInRole("comp_mealordering") && IsCompanyUser())
                          || IsSchoolUser();
            if (!isAllow) return RedirectToHomeIndex();

            var order = MealMenuOrderService.GetSchoolOrder(new SchoolOrderGetRequest
            {
                Filter = new MealMenuOrderFilterView
                {
                    RecordStatusId = (int)RecordStatuses.Active,
                    OrderDate = model.StartDate,
                    SchoolId = model.SchoolId,
                    MealTypeId = model.MealTypeId,
                    SchoolType = schoolType
                }
            });
           

            order.Order.SchoolName = model.SchoolName;
            order.Order.SchoolType = model.SchoolType;
            order.Order.SchoolId = model.SchoolId;
            order.Order.FromOutSide = model.FromOutSide;

            order.Order.SupplementaryList = new List<MealMenuSupplementaryView>();
            order.Order.FoodPercentage = new FoodPercentageView
            {
                MealType = Lookups.GetItem<MealTypes>(model.MealTypeId),
                Fruit = 100,
                Vegetable = 100,
                SchoolId = model.SchoolId
            };
            
            var getSupplementaryListResponse =
                MealMenuOrderService.GetSupplementaryList(new GetSupplementaryListRequest
                {
                    Filter = new GetSupplementaryListFilterView
                    {
                        MealTypeId = model.MealTypeId,
                        SchoolId = model.SchoolId
                    }
                });

            if (getSupplementaryListResponse.Result == Result.Success
                && getSupplementaryListResponse.List != null)
                order.Order.SupplementaryList = getSupplementaryListResponse.List;


            var foodPercentageResponse = FoodService.GetFoodPercentages(new FoodPercentagesRequest{SchoolId = model.SchoolId});

            if (foodPercentageResponse.Result == Result.Success &&
                foodPercentageResponse.PercentageList.Any(d => d.MealType.Id == model.MealTypeId))
                order.Order.FoodPercentage = foodPercentageResponse.PercentageList.First(d => d.MealType.Id == model.MealTypeId);


            return View(order.Order);
        }
        
        [HttpPost]
        public JsonResult MealMenuOrder(MealMenuOrderGetAllRequest request)
        {
            //currentUser.Data.Add("UserId"
            request.Filter.RecordStatusId = (int)RecordStatuses.Active;
            request.Filter.SchoolId = CurrentUser.CurrentSchoolId();
            var orders = MealMenuOrderService.GetAllByFilter(request);
            return Json(orders, JsonRequestBehavior.DenyGet);

        }
        

        [HttpPost]
        public JsonResult SubmitOrder(MealMenuOrderSubmitOrderRequest request)
        {
            request.Filter.RecordStatusId = (int) RecordStatuses.Active;
            request.UserId = Convert.ToInt32(CurrentUser.Data["UserId"]);
            return Json(MealMenuOrderService.SubmitOrder(request), JsonRequestBehavior.DenyGet);

        }
        [HttpGet]
        public ActionResult EditOrderItem(int? id, int mealMenuId)
        {
            ViewBag.MealServiceTypes = Lookups.GetItems<MealServiceTypes>();
            var orderItem = MealMenuOrderService.GetOrderItemByFilter(
                new MealMenuOrderItemGetAllRequest
                    {
                        Filter = new MealMenuOrderItemFilterView { OrderItemId = id, MealMenuId = mealMenuId }
                    });
            ViewBag.UserTypeId = CurrentUser.UserTypeId();
            if (orderItem.OrderItem == null)
                orderItem.OrderItem = new MealMenuOrderItemView
                {
                    MealMenuId = mealMenuId,
                    Foods = new List<FoodListItemView>()
                };
            return View(orderItem.OrderItem);
        }

        [HttpPost]
        //public JsonResult EditOrderItem(MealMenuOrderItemView orderItem)
        public JsonResult EditOrderItem(MealMenuOrderItemSaveRequest request)
        {
            request.OrderItem.ModifiedBy = CurrentUser.Name;
            request.OrderItem.ModifiedByFullName = CurrentUser.FullName;
            if (CurrentUser.UserTypeId() == (int)UserTypes.School)
                request.SchoolId = CurrentUser.CurrentSchoolId();

            return Json(MealMenuOrderService.SaveOrderItem(request), JsonRequestBehavior.DenyGet);
        }
        
        [HttpPost]
        public JsonResult DeleteOrderItem(MealMenuOrderItemView orderItem)
        {
            orderItem.ModifiedBy = CurrentUser.Name;
            orderItem.ModifiedByFullName = CurrentUser.FullName;
            var request = new MealMenuOrderItemSaveRequest
            {
                SchoolId = CurrentUser.CurrentSchoolId(),
                OrderItem = orderItem
            };
            return Json(MealMenuOrderService.DeleteOrderItem(request), JsonRequestBehavior.DenyGet);

        }



        public ActionResult OrderList()
        {

            var mealTypeId = (int) MealTypes.Breakfast;
            if (!string.IsNullOrWhiteSpace(Request["MealTypeId"]))
                mealTypeId = Int32.Parse(Request["MealTypeId"]);


            var mealTypes = Lookups.GetItems<MealTypes>().Where(d=>d.Id>0).Select(d=>new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Text,
                Selected = d.Id==mealTypeId
            }).ToList();
            ViewBag.MealTypes = mealTypes;

          var schoolTypeId = 0;
            if(!string.IsNullOrWhiteSpace(Request["SchoolTypeId"]))
                schoolTypeId = Int32.Parse(Request["SchoolTypeId"]);

            var schoolTypes = SysMngConfig.Lookups.GetItems<SysMngConfig.SchoolTypes>();
            ViewBag.SchoolTypes = schoolTypes.Select(d => new SelectListItem { Text = d.Text, Value = d.Id.ToString(), Selected = (d.Id == schoolTypeId) }).ToList();


            var recordCount = 25;
            if(!string.IsNullOrWhiteSpace(Request["RecordCount"]))
                recordCount = Int32.Parse(Request["RecordCount"]);

            ViewBag.RecordCounts = new List<SelectListItem>
            {
                new SelectListItem{Value = "25",Text = "25",Selected = recordCount==25},
                new SelectListItem{Value = "50",Text="50",Selected = recordCount==50},
                new SelectListItem{Value= "100",Text="100",Selected = recordCount==100},
                new SelectListItem{Value="250",Text="250",Selected = recordCount==250},
                new SelectListItem{Value="500",Text="500",Selected=recordCount==500},
                new SelectListItem{Value="1000",Text="1000",Selected=recordCount==1000}  
            };



            //var orderStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //var orderEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(orderStartDate.Year, orderStartDate.Month));

            var orderStartDate = DateTime.Now.AddDays(-6);
            var orderEndDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(Request["OrderStartDate"]))
                orderStartDate = DateTime.Parse(Request["OrderStartDate"]);
            if (!string.IsNullOrWhiteSpace(Request["OrderEndDate"]))
                orderEndDate = DateTime.Parse(Request["OrderEndDate"]);



            var request = new MealMenuOrderReportRequest
            {
                Filter = new OrderReportFilterView
                {
                    OrderStartDate = orderStartDate,
                    OrderEndDate = orderEndDate,
                    SchoolNameStartsWith = Request["SchoolNameStartsWith"] ?? string.Empty,
                    MealTypeId = mealTypeId,
                    SchoolTypeId = schoolTypeId
                },
                OrderByAsc = true,
                OrderByField = "SchoolName",
                PageIndex = 1,
                PageSize = recordCount,
            };
            var result = MealMenuOrderService.GetOrderReport(request);
            return View(result.Orders);

        }
        public ActionResult DailyChanges()
        {
            var mealTypes = Lookups.GetItems<MealTypes>().Where(d => d.Id > 0).ToList();

            ViewBag.MealTypes = mealTypes;

            var orderItemDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var mealTypeId = (int)mealTypes[0].Id;

            if (!string.IsNullOrWhiteSpace(Request["OrderItemDate"]))
                orderItemDate = DateTime.Parse(Request["OrderItemDate"]);

            if (!string.IsNullOrWhiteSpace(Request["MealTypeId"]))
                mealTypeId = int.Parse(Request["MealTypeId"].ToString(CultureInfo.InvariantCulture));

            var request = new MealMenuOrderDailyChangesRequest
            {
                Filter = new DailyChangesFilterView
                {
                    OrderItemDate = orderItemDate,
                    MealTypeId = mealTypeId
                },
                OrderByAsc = true,
                OrderByField = "Route",
                PageIndex = 1,
                PageSize = Int32.MaxValue
            };

            //Get Order Data
            var result = MealMenuOrderService.GetDailyChanges(request);
            return View(result.OrderItems);
        }

        [HttpPost]
        public JsonResult OrderItemHistory(MealMenuOrderItemGetHistoryRequest request)
        {
            return Json(MealMenuOrderService.GetOrderItemHistory(request), JsonRequestBehavior.DenyGet);
        }

        public ActionResult MonthlyExport()
        {
            var orderStartDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(Request["OrderStartDate"]))
                orderStartDate = DateTime.Parse(Request["OrderStartDate"]);

            orderStartDate = new DateTime(orderStartDate.Year, orderStartDate.Month, 1);
            var orderEndDate = new DateTime(orderStartDate.Year, orderStartDate.Month, DateTime.DaysInMonth(orderStartDate.Year, orderStartDate.Month));

            var response = ReportService.MonthlyExport(new MonthlyExportRequest
            {
                Filter = new OrderReportFilterView
                {
                    OrderStartDate = orderStartDate,
                    OrderEndDate = orderEndDate,
                    MealTypeId = Int32.Parse(Request["MealTypeId"]),
                }
            });
            if(response.Result==Result.Success)
            {

                return new FileStreamResult(new FileStream(response.FileName, FileMode.Open), "application/zip")
                    {
                        FileDownloadName = string.Format("{0}-{1}-MonthlyExport.zip",
                            orderStartDate.Year, orderStartDate.ToString("MMM"))
                    };
            }
            ErrorMessage = "Couldn't generate monthly order export file";
            return null;
        }

        public ActionResult SchoolMenuExport(DateTime orderDate, int mealTypeId, int schoolId,int schoolType)
        {
            var response = ReportService.SchoolMenuExport(new SchoolMenuExportRequest
            {
                Filter = new SchoolMenuExportFilterView
                {
                    MealTypeId = mealTypeId,
                    SchoolId = schoolId,
                    OrderDate = orderDate,
                    SchoolType = schoolType
                }
            });
            if (response.Result == Result.Success)
            {

                return new FileStreamResult(new FileStream(response.FileName, FileMode.Open), "application/vnd.ms-excel")
                {
                    FileDownloadName = string.Format("{0}-{1}-SchoolMenuExport.xlsx",
                        orderDate.Year, orderDate.ToString("MMM"))
                };
            }
            ErrorMessage = "Couldn't generate school mountly menu";
            return null;
        }
        
        [HttpPost]
        public JsonResult SaveOrderForDay(SaveOrderForDayRequest request)
        {
            var response = MealMenuOrderService.SaveOrderForDay(request);
            return Json(response);
        }
    }
}
