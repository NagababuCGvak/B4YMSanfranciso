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
using Tar.Service.Messages;
using Better4You.UI.Mvc.Helpers;
using Better4You.UI.Mvc.Models;


namespace Better4You.UI.Mvc.Controllers
{
    public class InvoiceController : ControllerBase
    {
        //
        // GET: /Invoice/

        public IMealMenuOrderService OrderService { get; set; }
        public ISchoolService SchoolService { get; set; }
        public IMealMenuService MealMenuService { get; set; }
        public ISettings Settings { get; set; }
        public IReportService ReportService { get; set; }
        public ActionResult Index()
        {
            ViewBag.Year = Year;
            ViewBag.Month = Month;
            var orderDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (Request["OrderMonth"] != null && Request["OrderYear"] != null)
                orderDate = new DateTime(Int32.Parse(Request["OrderYear"]), Int32.Parse(Request["OrderMonth"]), 1);

            var recordCount = 1000;
            if (!string.IsNullOrWhiteSpace(Request["RecordCount"]))
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


            var request = new InvoiceGetAllRequest
            {
                Filter = new InvoiceFilterView { OrderDate = orderDate, SchoolNameStartsWith = Request["SchoolNameStartsWith"] ?? string.Empty },
                OrderByAsc = true,
                OrderByField = "SchoolName",
                PageIndex = 1,
                PageSize = recordCount
            };
            TryUpdateModel(request.Filter);
            var result = OrderService.SchoolInvoicesGetAllByFilter(request);
            return View(result.SchoolInvoices);
        }

        public ActionResult InvoiceSummary()
        {
            var mealTypes = Lookups.GetItems<MealTypes>().Where(m => m.Id != (int)MealTypes.None).ToList();
            ViewBag.MealTypes = mealTypes;

            ViewBag.Year = Year;
            ViewBag.Month = Month;
            var orderDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            int mealTypeId = (int)mealTypes[0].Id;

            if (Request["OrderYear"] != null)
                orderDate = new DateTime(Int32.Parse(Request["OrderYear"]), 1, 1);

            if (Request["MealTypeId"] != null && !string.IsNullOrWhiteSpace(Request["MealTypeId"]))
                mealTypeId = int.Parse(Request["MealTypeId"]);

            var request = new InvoiceSummaryGetAllRequest
            {
                Filter = new InvoiceFilterView { OrderDate = orderDate, MealTypeId = mealTypeId },
                OrderByAsc = true,
                OrderByField = "SchoolName",
                PageIndex = 1,
                PageSize = 100
            };
            TryUpdateModel(request.Filter);
            var result = OrderService.InvoicesSummaryGetAllByFilter(request);
            return View(result.Invoices);

        }
        public ActionResult SchoolInvoice(string id)
        {
            long mealTypeId;
            if (string.IsNullOrEmpty(id)
                || !long.TryParse(id, out mealTypeId) || mealTypeId <= 0)
                return RedirectToAction("SchoolInvoice", new { id = 1 });

            var mealTypes = Lookups.GetItems<MealTypes>().Where(m => m.Id != (int)MealTypes.None).ToList();
            ViewBag.MealTypes = mealTypes;

            ViewBag.Year = Year;
            ViewBag.Month = Month;
            ViewBag.MealTypeId = mealTypeId;
            var schoolInvoices = new List<KeyValuePair<long, List<InvoiceSummaryView>>>();

            for (var i = Year[0]; i <= DateTime.Now.Year; i++)
            {
                var request = new InvoiceSummaryGetAllRequest
                {
                    Filter = new InvoiceFilterView
                    {
                        OrderDate = new DateTime(i, 1, 1),
                        SchoolId = CurrentUser.CurrentSchoolId(),
                        MealTypeId = mealTypeId
                    },
                    OrderByAsc = true,
                    OrderByField = "SchoolName",
                    PageIndex = 1,
                    PageSize = 100
                };
                var result = OrderService.InvoicesSummaryGetAllByFilter(request);
                schoolInvoices.Add(new KeyValuePair<long, List<InvoiceSummaryView>>(i, result.Invoices));
            }

            return View(schoolInvoices);

        }

        [HttpGet]
        public ActionResult Edit(int[] invoiceIds)
        {

            var menuOrderList = new List<InvoiceListItemView>();
            foreach (var i in invoiceIds)
            {
                menuOrderList.AddRange(OrderService.InvoicesGetAllByFilter(new InvoiceGetAllRequest { Filter = new InvoiceFilterView { OrderId = i } }).Invoices);
            }

            ViewBag.OrderStatuses = Lookups.GetItems<OrderStatuses>().Where(d => d.Id != (int)OrderStatuses.InitialState).ToList();
            return View(menuOrderList);
        }

        [HttpPost]
        public ActionResult Edit(List<InvoiceListItemView> invoiceList)
        {
            TryUpdateModel(invoiceList);
            //var updatedInvoices = new List<InvoiceListItemView>();
            var infoMessage = "";
            foreach (var invoice in invoiceList)
            {
                invoice.OrderStatus = new Tar.ViewModel.GeneralItemView(int.Parse(Request["OrderStatus.Key_" + invoice.Id]), "", "");
                var response = OrderService.InvoiceUpdate(new InvoiceUpdateRequest { Invoice = invoice, UserId = Convert.ToInt32(CurrentUser.Data["UserId"]) });
                if (response.Result == Result.Success)
                    infoMessage = response.Message;
                else
                {
                    ErrorMessage = response.Message;
                    return RedirectToAction("Index");
                }
            }
            InfoMessage = infoMessage;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Generate(int[] invoiceIds)//block içerisindeki invoiceId'ler invoiceIds[0] yapıldı, düzenlenmeli!
        {
            var orderReports = new List<MealMenuOrderReportResponse>();
            var model = new InvoiceExportViewModel();

            foreach (var i in invoiceIds)
            {
                var orderList = OrderService.InvoicesGetAllByFilter(new InvoiceGetAllRequest { Filter = new InvoiceFilterView() { OrderId = i } });
                if (orderList != null && orderList.Invoices != null && orderList.Invoices.Count > 0)
                {
                    var note = orderList.Invoices[0].Note;
                    var mealType = (MealTypes)orderList.Invoices[0].MealType.Id;
                    model.Notes.Add(mealType, note);
                }

            }

            foreach (int i in invoiceIds)
            {
                var orderReport =
                    OrderService.GetOrderReport(new MealMenuOrderReportRequest
                    {
                        Filter = new OrderReportFilterView { OrderId = i },
                        OrderByAsc = true,
                        OrderByField = "Id",
                        PageIndex = 0,
                        // PageSize = 20
                    });
                if (orderReport.Result == Result.Success && orderReport.Orders.Any())
                {
                    orderReports.Add(orderReport);
                }

            }
            if (orderReports.Count == 0)
            {
                ErrorMessage = "Invoice Couldn't be found.";
                return null;
            }

            var school = SchoolService.Get(new SchoolGetRequest { Id = orderReports[0].Orders[0].SchoolId });

            model.School = school.School;
            model.orderReports = orderReports;


            var templateDirectory = Settings.GetSetting<string>("TemplateDirectory");
            var templateFile = Server.MapPath(string.Format(@"\{0}\invoice5.xlsx", templateDirectory));

            var directoryPath =
                Server.MapPath(string.Format(@"~\{0}\Invoices\{1}", templateDirectory, invoiceIds[0]));

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            var dateString = DateTime.Now.ToString("yyyyMMdd");
            var files = Directory.GetFiles(directoryPath, string.Format("{0}*", dateString),
                                           SearchOption.TopDirectoryOnly);
            var fileName = string.Format("{0}_{1}.xlsx", dateString, files.Count() + 1);
            var filePath = string.Format(@"{0}\{1}", directoryPath, fileName);


            InvoiceExport invoice = new InvoiceExport(model, filePath, templateFile, OrderService, SchoolService);
            invoice.GenerateInvoiceSheet();
            invoice.CloseWorkSheet();

            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            var fileContent = new byte[fs.Length];
            fs.Read(fileContent, 0, fileContent.Length);

            return File(fileContent, "application/vnd.ms-excel", fileName);
        }

        [HttpGet]
        public ActionResult UploadDocument(int schoolId, string schoolName, string invoiceMonth, string invoiceYear)
        {
            ViewBag.SchoolId = schoolId;
            ViewBag.SchoolName = schoolName;
            ViewBag.InvoiceMonth = invoiceMonth;
            ViewBag.InvoiceYear = invoiceYear;
            return PartialView();
        }

        [HttpPost]
        public ActionResult UploadDocument()
        {
            if (HttpContext.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Request.Files["invoiceDocument"];
                if (!string.IsNullOrEmpty(httpPostedFile.FileName))
                {
                    var request = new SchoolInvoiceDocumentSaveRequest
                    {
                        SchoolInvoice = new SchoolInvoiceDocumentView()
                        {
                            DocumentStream = new byte[httpPostedFile.InputStream.Length],
                            ModifiedBy = CurrentUser.Name,
                            ModifiedByFullName = CurrentUser.FullName
                        }
                    };

                    httpPostedFile.InputStream.Read(request.SchoolInvoice.DocumentStream, 0, (int)httpPostedFile.InputStream.Length);
                    request.SchoolInvoice.DocumentName = httpPostedFile.FileName.Split('\\').Last();
                    request.SchoolInvoice.InvoiceMonth = int.Parse(Request.QueryString["InvoiceMonth"]);
                    request.SchoolInvoice.InvoiceYear = int.Parse(Request.QueryString["InvoiceYear"]);
                    request.SchoolInvoice.SchoolId = int.Parse(Request.QueryString["SchoolId"]);
                    request.SchoolInvoice.DocumentGuid = Guid.NewGuid();
                    var response = OrderService.SaveSchoolInvoiceDocument(request);
                    if (response.Result == Result.Success)
                        InfoMessage = response.Message;
                    else
                        ErrorMessage = response.Message;
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public FileResult DownloadDocument(Guid id)
        {
            var response = OrderService.GetSchoolInvoiceDocumentById(new SchoolInvoiceDocumentGetByIdRequest() { SchoolInvoiceDocumentGuid = id });
            return File(response.SchoolInvoiceDocument.DocumentStream, "application/pdf", response.SchoolInvoiceDocument.DocumentName);
        }


        public ActionResult DateRangeBilling()
        {
            var orderStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var orderEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                DateTime.DaysInMonth(orderStartDate.Year, orderStartDate.Month));
            if (Request["OrderStartDate"] != null && !string.IsNullOrWhiteSpace(Request["OrderStartDate"]))
                orderStartDate = DateTime.Parse(Request["OrderStartDate"]);
            if (Request["OrderEndDate"] != null && !string.IsNullOrWhiteSpace(Request["OrderEndDate"]))
                orderEndDate = DateTime.Parse(Request["OrderEndDate"]);
            var response = OrderService.DateRangeOrderItem(new DateRangeOrderItemRequest
            {
                Filter = new DateRangeOrderItemFilterView
                {
                    SchoolName = Request["SchoolNameStartsWith"] ?? string.Empty,
                    EndDate = orderEndDate,
                    StartDate = orderStartDate
                }
            });
            ViewBag.MealTypes = Lookups.GetItems<MealTypes>().Where(d => d.Id > 0).OrderBy(d => d.Id).ToList();
            return View(response.OrderItems);
        }

        public ActionResult DateRangeBillingExport()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                DateTime.DaysInMonth(startDate.Year, startDate.Month));
            if (!string.IsNullOrWhiteSpace(Request["OrderStartDate"]))
                startDate = DateTime.ParseExact(Request["OrderStartDate"], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (!string.IsNullOrWhiteSpace(Request["OrderEndDate"]))
                endDate = DateTime.ParseExact(Request["OrderEndDate"], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var response = ReportService.DateRangeOrderItemExport(new DateRangeOrderItemRequest
            {
                Filter = new DateRangeOrderItemFilterView
                {
                    SchoolName = Request["SchoolNameStartsWith"] ?? string.Empty,
                    EndDate = endDate,
                    StartDate = startDate
                }
            });

            var fs = new FileStream(response.FileName, FileMode.Open, FileAccess.Read);

            var fileContent = new byte[fs.Length];
            fs.Read(fileContent, 0, fileContent.Length);

            return File(fileContent, "application/vnd.ms-excel", string.Format("{0}_{1}_BillingReport.xlsx", startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd")));
        }

    }
}
