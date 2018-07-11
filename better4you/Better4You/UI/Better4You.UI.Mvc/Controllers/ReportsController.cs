using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Better4You.Meal.Config;
using Better4You.Meal.Service;
using Better4You.Meal.Service.Messages;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.UI.Mvc.Controllers
{
    public class ReportsController : ControllerBase
    {
        public IReportService ReportService
        {
            get { return ServiceLocator.Get<IReportService>(); }
        }
        //
        // GET: /Reports/

        public ActionResult Index()
        {
            var mealTypeId = (int)MealTypes.Breakfast;
            if (!string.IsNullOrWhiteSpace(Request["MealTypeId"]))
                mealTypeId = Int32.Parse(Request["MealTypeId"]);


            var mealTypes = Lookups.GetItems<MealTypes>().Where(d => d.Id > 0).Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Text,
                Selected = d.Id == mealTypeId
            }).ToList();

            ViewBag.MealTypes = mealTypes;
            ViewBag.Year = Year;
            ViewBag.Month = Month;

            return View();
        }

        public ActionResult MilkExport()
        {
            var orderStartDate = DateTime.Now;

            if (Request["OrderStartDate"] != null && !string.IsNullOrWhiteSpace(Request["OrderStartDate"]))
                orderStartDate = DateTime.Parse(Request["OrderStartDate"]);

            var mealTypeId = MealTypes.Breakfast;
            if (!string.IsNullOrWhiteSpace(Request["MealTypeId"]))
                Enum.TryParse(Request["MealTypeId"], true, out mealTypeId);

            orderStartDate = new DateTime(orderStartDate.Year, orderStartDate.Month, 1);
            var orderEndDate = new DateTime(orderStartDate.Year, orderStartDate.Month, DateTime.DaysInMonth(orderStartDate.Year, orderStartDate.Month));

            var response = ReportService.MontlyMilkExport(new MontlyMilkExportRequest
            {
                Filter = new OrderReportFilterView
                {
                    OrderStartDate = orderStartDate,
                    OrderEndDate = orderEndDate,
                    MealTypeId = (int)mealTypeId,
                }
            });
            if (response.Result == Result.Success)
            {
                var startIndex = response.FileName.LastIndexOf(response.FileName.IndexOf('\\') > -1 ? '\\' : '/');

                return new FileStreamResult(new FileStream(response.FileName, FileMode.Open), "application/vnd.ms-excel")
                {
                    FileDownloadName = response.FileName.Substring(startIndex + 1)//string.Format("{0}_{1}_Milk.xlsx", orderStartDate.ToString("yyyy-MMM"), mealTypeId.ToString("G"))

                };
            }
            ErrorMessage = response.Message + " Couldn't generate monthly milk export file";
            return null;
        }

        public ActionResult FruitVegExport()
        {
            var orderDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (Request["OrderMonth"] != null && Request["OrderYear"] != null)
                orderDate = new DateTime(Int32.Parse(Request["OrderYear"]), Int32.Parse(Request["OrderMonth"]), 1);

            var response = ReportService.OrderDayPropItemExport(new OrderDayPropItemRequest
            {
                Filter = new MealMenuOrderFilterView
                {
                    OrderDate = orderDate
                }
            });

            if (response.Result == Result.Success)
            {
                var startIndex = response.FileName.LastIndexOf(response.FileName.IndexOf('\\') > -1 ? '\\' : '/');

                return new FileStreamResult(new FileStream(response.FileName, FileMode.Open), "application/vnd.ms-excel")
                {
                    FileDownloadName = response.FileName.Substring(startIndex + 1)//string.Format("{0}_{1}_Milk.xlsx", orderStartDate.ToString("yyyy-MMM"), mealTypeId.ToString("G"))

                };
            }
            ErrorMessage = response.Message + " Couldn't generate Daily Item Props export file";
            return null;

        }
    }
}
