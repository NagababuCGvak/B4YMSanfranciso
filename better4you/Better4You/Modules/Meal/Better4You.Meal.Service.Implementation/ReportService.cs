using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Better4You.Meal.Business;
using Better4You.Meal.Config;
using Better4You.Meal.Service.Messages;
using Better4You.Meal.ViewModel;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Tar.Core.Compression;
using Tar.Core.Configuration;
using Tar.Service;
using Tar.Service.Messages;
using Better4You.Core;

namespace Better4You.Meal.Service.Implementation
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReportService : Service<IReportService, ReportService>, IReportService
    {
        private readonly IMealMenuOrderFacade _mealMenuOrderFacade;
        private readonly IMenuFacade _menuFacade;
        private readonly IApplicationSettings _appSetting;
        private readonly IMealMenuOrderService _mealMenuOrderService;

        public ReportService(IMealMenuOrderFacade mealMenuOrderFacade, IApplicationSettings appSetting,
            IMealMenuOrderService mealMenuOrderService, IMenuFacade menuFacade)
        {
            if (mealMenuOrderFacade == null) throw new ArgumentNullException("mealMenuOrderFacade");
            if (menuFacade == null) throw new ArgumentNullException("menuFacade");
            if (appSetting == null) throw new ArgumentNullException("appSetting");
            if (mealMenuOrderService == null) throw new ArgumentNullException("mealMenuOrderService");
            _mealMenuOrderFacade = mealMenuOrderFacade;
            _menuFacade = menuFacade;
            _appSetting = appSetting;
            _mealMenuOrderService = mealMenuOrderService;
        }

        private string FileRepositoryPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    _appSetting.GetSetting<string>("FileRepositoryPath"));
            }
        }

        public ReportResponse MonthlyExport(MonthlyExportRequest request)
        {
            return Execute<MonthlyExportRequest, ReportResponse>(
                request,
                response =>
                {
                    int totalCount;
                    if (!request.Filter.OrderStartDate.HasValue)
                        throw new Exception("Order StartDate Is Null");
                    var orderStartDate = request.Filter.OrderStartDate.Value;
                    var eMealType = (MealTypes)request.Filter.MealTypeId;

                    //Dictionary<long, MealOrderManageView> sackLunches = new Dictionary<long, MealOrderManageView>();

                    var orders = _mealMenuOrderFacade.GetOrderReport(request.Filter, int.MaxValue, 1, "Route", true,
                        out totalCount);

                    var fileRepository = FileRepositoryPath;//_appSetting.GetSetting<string>("FileRepositoryPath");
                    var templateFile = Path.Combine(fileRepository, "Templates", "MonthlyOrder.xlsx");
                    var zipFolder = Path.Combine(fileRepository, "MonthlyOrder",
                        string.Format("{0}_{1}", orderStartDate.ToString("yyyy-MMM"), eMealType.ToString("G")));
                    var zipFile = Path.Combine(fileRepository, "MonthlyOrder",
                        string.Format("{0}_{1}.zip", orderStartDate.ToString("yyyy-MMM"), eMealType.ToString("G")));
                    if (Directory.Exists(zipFolder))
                        Directory.Delete(zipFolder, true);
                    Directory.CreateDirectory(zipFolder);


                    var orderMenuTypes =
                        orders.SelectMany(o => o.Items.SelectMany(i => i.Menus.Where(k => k.MenuTypeId != (int)MenuTypes.Milk).Select(m => m.MenuTypeId)))
                            .GroupBy(d => d)
                            .Select(d => d.Key)
                            .ToList();


                    orderMenuTypes.ForEach(menuType =>
                    {
                        var eMenuType = (MenuTypes)menuType;
                        var menuTypeText = eMenuType.ToString("G");

                        var lMenuType = Lookups.MenuTypeList.FirstOrDefault(lm => lm.Id == menuType);
                        if (lMenuType != null)
                            menuTypeText = lMenuType.Text;
                        //var filePath = Path.Combine(zipFolder, string.Format("{0}.xlsx", eMenuType.ToString("G")));
                        var filePath = Path.Combine(zipFolder, string.Format("{0}.xlsx", menuTypeText));
                        File.Copy(templateFile, filePath, false);
                        File.SetAttributes(filePath, FileAttributes.Normal | FileAttributes.Archive);
                        using (var workBook = new XLWorkbook(filePath, XLEventTracking.Disabled))
                        {
                            /*
                            var headerTitle = string.Format("{2} {0} Order -  {1}  Menu Type", eMealType.ToString("G"),
                                eMenuType.ToString("G"), orderStartDate.ToString("yyyy-MMM"));
                            */
                            var headerTitle = string.Format("{2} {0} Order -  {1}  Menu Type", eMealType.ToString("G"),
                                menuTypeText, orderStartDate.ToString("yyyy-MMM"));

                            var orderList =
                                orders.Where(o => o.Items.Any(i => i.Menus.Any(m => m.MenuTypeId == menuType)))
                                    .OrderBy(d => d.SchoolRoute)
                                    .ThenBy(d => d.SchoolName)
                                    .ToList();

                            var workSheet = workBook.Worksheets.First();

                            var lastDayOfMonth = DateTime.DaysInMonth(orderStartDate.Year, orderStartDate.Month);

                            var rowStart = 6;
                            var columnStart = 6;

                            for (var day = orderStartDate.Day; day <= lastDayOfMonth; day++)
                            {
                                var currentDay = new DateTime(orderStartDate.Year, orderStartDate.Month, day);
                                if (currentDay.DayOfWeek == DayOfWeek.Saturday ||
                                    currentDay.DayOfWeek == DayOfWeek.Sunday)
                                    continue;

                                workSheet.Cell(rowStart, columnStart)
                                    .SetValue(currentDay.DayOfWeek.ToString().Substring(0, 2));
                                workSheet.Cell(rowStart + 1, columnStart).SetValue(day.ToString());
                                columnStart++;
                            }

                            workSheet.Range(rowStart, columnStart, rowStart + 1, columnStart)
                                .Merge()
                                .SetValue("Notes")
                                .Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                            var tableHeaderRange =
                                workSheet.Range(rowStart, 6, rowStart + 1, columnStart).AddToNamed("tableHeader");
                            tableHeaderRange.Style.Fill.BackgroundColor = XLColor.FromArgb(218, 238, 243);
                            tableHeaderRange.Style.Border.InsideBorderColor = XLColor.FromArgb(0, 0, 221);
                            tableHeaderRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                            tableHeaderRange.Style.Border.OutsideBorderColor = XLColor.FromArgb(0, 0, 139);
                            tableHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                            var headerTitleRange =
                                workSheet.Range(2, 6, 3, columnStart).Merge().AddToNamed("headerTitle");
                            headerTitleRange.SetValue(headerTitle);
                            headerTitleRange.Style.Fill.BackgroundColor = XLColor.FromArgb(67, 255, 152);
                            headerTitleRange.Style.Font.SetFontSize(16);
                            headerTitleRange.Style.Font.SetBold(true);
                            headerTitleRange.Style.Font.SetItalic(true);
                            headerTitleRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            headerTitleRange.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);



                            var orderRowStart = 8;
                            var orderColumnStart = 1;

                            var summaryTotal = new Dictionary<DateTime, long>();

                            orderList.ForEach(o =>
                            {
                                var schoolRoute = o.SchoolRoute;
                                workSheet.Cell(orderRowStart, orderColumnStart).SetValue(schoolRoute);


                                workSheet.Cell(orderRowStart, orderColumnStart + 1).SetValue(o.FoodServiceType);
                                workSheet.Cell(orderRowStart, orderColumnStart + 2).SetValue(o.OVSType);
                                workSheet.Cell(orderRowStart, orderColumnStart + 3).SetValue(o.SchoolType);
                                workSheet.Cell(orderRowStart, orderColumnStart + 4).SetValue(o.SchoolName);
                                var mealNames = "";
                                if (eMenuType == MenuTypes.Special)
                                {
                                    mealNames = string.Join(Environment.NewLine,
                                        o.Items.SelectMany(l => l.Menus.Select(m => m))
                                            .Where(m => m.MenuTypeId == menuType)
                                            .GroupBy(m => m.Name)
                                            .Select(m => m.Key));
                                }
                                /*
                                if(eMenuType==MenuTypes.SackLunch)
                                {
                                    MealOrderManageView sackLunchView = null;
                                    if (sackLunches.ContainsKey(o.SchoolId))
                                    {
                                        sackLunchView = sackLunches[o.SchoolId];
                                    }
                                    else
                                    {
                                        sackLunchView = GetSackLunches(o.SchoolId, o.OrderDate);
                                        sackLunches.Add(o.SchoolId, sackLunchView);
                                    }
                                }
                                */
                                workSheet.Cell(orderRowStart, orderColumnStart + 4)
                                    .SetValue(string.IsNullOrWhiteSpace(mealNames)
                                        ? o.SchoolName
                                        : string.Format("{0}{1}{2}", o.SchoolName, Environment.NewLine, mealNames));

                                columnStart = 6;
                                for (var day = orderStartDate.Day; day <= lastDayOfMonth; day++)
                                {
                                    var currentDay = new DateTime(orderStartDate.Year, orderStartDate.Month, day);
                                    if (currentDay.DayOfWeek == DayOfWeek.Saturday ||
                                        currentDay.DayOfWeek == DayOfWeek.Sunday)
                                        continue;
                                    var currentDayInfo = o.Items.FirstOrDefault(d => Equals(d.Date, currentDay));

                                    if (!summaryTotal.ContainsKey(currentDay))
                                        summaryTotal.Add(currentDay, 0);

                                    if (currentDayInfo != null)
                                    {
                                        var currentDayMenus =
                                            currentDayInfo.Menus.Where(m => m.MenuTypeId == menuType).ToList();
                                        var sumCount = currentDayMenus.Sum(m => m.TotalCount);
                                        workSheet.Cell(orderRowStart, columnStart).SetValue(sumCount);
                                        summaryTotal[currentDay] += sumCount;
                                        if (currentDayMenus.Any(m => m.RefId.HasValue && m.RefId.Value > 0))
                                            workSheet.Cell(orderRowStart, columnStart).Style.Fill.BackgroundColor =
                                                XLColor.PowderBlue;

                                        if (eMenuType == MenuTypes.SackLunch1 && currentDayInfo.DeliveryType == (long)DeliveryTypes.Breakfast)

                                            workSheet.Cell(orderRowStart, columnStart).Style.Fill.BackgroundColor = XLColor.AppleGreen;
                                    }
                                    columnStart++;
                                }

                                orderRowStart++;
                            });
                            //workSheet.Range(8, 1, orderRowStart, columnStart).AddToNamed("tableContent");

                            var tableContent =
                                workSheet.Range(8, 1, orderRowStart, columnStart).AddToNamed("tableContent");
                            tableContent.Style.Border.InsideBorderColor = XLColor.FromArgb(0, 0, 221);
                            tableContent.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                            tableContent.Style.Border.OutsideBorderColor = XLColor.FromArgb(0, 0, 139);
                            tableContent.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;


                            workSheet.Range(6, 1, 6, 4)
                                .Merge()
                                .SetValue(string.Format("Report Date : {0:G}", DateTime.Now));

                            var dailyTotal =
                                workSheet.Range(orderRowStart, 1, orderRowStart, 5).Merge().AddToNamed("dailyTotal");
                            dailyTotal.SetValue("Daily Total");
                            dailyTotal.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                            columnStart = 6;
                            summaryTotal.OrderBy(s => s.Key).ForEach(s =>
                            {
                                workSheet.Cell(orderRowStart, columnStart).SetValue(s.Value);
                                columnStart++;
                            });
                            var footerRow =
                                workSheet.Range(orderRowStart, 1, orderRowStart, columnStart).AddToNamed("footerRow");
                            footerRow.Style.Font.SetBold(true);
                            footerRow.Style.Font.SetItalic(true);
                            footerRow.Style.Border.InsideBorderColor = XLColor.FromArgb(0, 0, 221);
                            footerRow.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                            footerRow.Style.Border.OutsideBorderColor = XLColor.FromArgb(0, 0, 139);
                            footerRow.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;


                            workSheet.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                            workSheet.PageSetup.PagesWide = 1;
                            workSheet.Columns().AdjustToContents();

                            workBook.Save();
                        }

                    });
                    using (var stream = new FileStream(zipFile, FileMode.Create))
                    {
                        ZipComponentFactory.CreateZipComponent().Zip(stream, zipFolder, true);
                        Directory.Delete(zipFolder, true);
                    }
                    response.FileName = zipFile;
                });
        }

        public ReportResponse OrderDayPropItemExport(OrderDayPropItemRequest request)
        {
            return Execute<OrderDayPropItemRequest, ReportResponse>(
                request,
                response =>
                {

                    var orderItems = _mealMenuOrderFacade.GetDailyItemsReport(request.Filter);
                    var fileRepository = FileRepositoryPath; //_appSetting.GetSetting<string>("FileRepositoryPath");
                    var dayPropItemReport = Path.Combine(fileRepository, "DayPropItemReport");
                    if (!Directory.Exists(dayPropItemReport))
                        Directory.CreateDirectory(Path.Combine(fileRepository, "DayPropItemReport"));
                    var startDate = request.Filter.OrderDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var endDate = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
                    var filePath = Path.Combine(dayPropItemReport,
                        string.Format("{0}_{1}_DayPropItemReport.xlsx", startDate.ToString("yyyyMMdd"),
                            endDate.ToString("yyyyMMdd")));
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    using (var workBook = new XLWorkbook(XLEventTracking.Disabled))
                    {
                        var wsFruit = workBook.AddWorksheet("FruitReport");
                        var wsVeg = workBook.AddWorksheet("VegetableReport");
                        var meals = Lookups.MealTypeShortList.OrderBy(d => d.Id).ToList();
                        var rowIndex = 5;
                        var colIndex = 2;
                        var counter = 0;
                        var schoolList = orderItems.GroupBy(d => d.SchoolName).Select(d => new {
                            SchoolName = d.Key,
                            FruitCount = d.Sum(k => k.FruitCount ?? 0),
                            VegetableCount = d.Sum(k => k.VegetableCount ?? 0)
                        }).ToList();
                        for (var day = 1; day <= endDate.Day; day++)
                        {
                            var tableDay = new DateTime(endDate.Year, endDate.Month, day);
                            if (tableDay.DayOfWeek == DayOfWeek.Saturday || tableDay.DayOfWeek == DayOfWeek.Sunday)
                                continue;
                            var fruitDayRange = wsFruit.Range(rowIndex - 1, colIndex + counter, rowIndex - 1, colIndex + counter + meals.Count - 1);
                            fruitDayRange.Merge().AddToNamed("dayRange");
                            fruitDayRange.SetValue(day);

                            var vegDayRange = wsVeg.Range(rowIndex - 1, colIndex + counter, rowIndex - 1, colIndex + counter + meals.Count - 1);
                            vegDayRange.Merge().AddToNamed("dayRange");
                            vegDayRange.SetValue(day);
                            meals.ForEach(mm =>
                            {

                                wsFruit.Cell(rowIndex, colIndex + counter).SetValue(mm.Text);
                                wsVeg.Cell(rowIndex, colIndex + counter).SetValue(mm.Text);
                                counter++;
                            });
                        }

                        //counter++;
                        wsFruit.Cell(rowIndex, colIndex + counter).SetValue("Total Count");
                        wsFruit.Range(rowIndex + 1, colIndex, rowIndex + 1 + schoolList.Count, colIndex + counter).AddToNamed("numberFormat");


                        wsVeg.Cell(rowIndex, colIndex + counter).SetValue("Total Count");
                        wsVeg.Range(rowIndex + 1, colIndex, rowIndex + 1 + schoolList.Count, colIndex + counter).AddToNamed("numberFormat");



                        wsFruit.Cell(rowIndex, 1).SetValue("School");
                        wsVeg.Cell(rowIndex, 1).SetValue("School");

                        wsFruit.Range(rowIndex, 1, rowIndex, colIndex + counter).AddToNamed("tableHeaderRange");
                        wsFruit.Range(rowIndex + 1, colIndex, rowIndex + schoolList.Count, colIndex + counter).AddToNamed("numberFormat");

                        wsVeg.Range(rowIndex, 1, rowIndex, colIndex + counter).AddToNamed("tableHeaderRange");
                        wsVeg.Range(rowIndex + 1, colIndex, rowIndex + schoolList.Count, colIndex + counter).AddToNamed("numberFormat");


                        wsFruit.Range(rowIndex, 1, rowIndex + schoolList.Count, 2 + counter).AddToNamed("tableContent");
                        wsVeg.Range(rowIndex, 1, rowIndex + schoolList.Count, 2 + counter).AddToNamed("tableContent");

                        wsFruit.Range(rowIndex - 2, 1, rowIndex - 2, 2 + counter).Merge().AddToNamed("reportHeader").SetValue(string.Format("FRUIT ITEMS , {0} - {1}",
                            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd")));
                        wsVeg.Range(rowIndex - 2, 1, rowIndex - 2, 2 + counter).Merge().AddToNamed("reportHeader").SetValue(string.Format("VEGETABLE ITEMS , {0} - {1}",
                                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd")));

                        var rowItemIndex = 6;

                        schoolList.ForEach(oi =>
                        {
                            wsFruit.Cell(rowItemIndex, 1).SetValue(oi.SchoolName);
                            wsVeg.Cell(rowItemIndex, 1).SetValue(oi.SchoolName);
                            var colItemIndex = 2;

                            for (var day = 1; day <= endDate.Day; day++)
                            {
                                var tableDay = new DateTime(endDate.Year, endDate.Month, day);
                                if (tableDay.DayOfWeek == DayOfWeek.Saturday || tableDay.DayOfWeek == DayOfWeek.Sunday)
                                    continue;
                                meals.ForEach(m =>
                                {
                                    var item = orderItems.FirstOrDefault(d => d.SchoolName == oi.SchoolName && d.OrderDay == tableDay && d.MealType.Id == m.Id);
                                    if (item != null)
                                    {
                                        wsFruit.Cell(rowItemIndex, colItemIndex).SetValue(item.FruitCount);
                                        wsVeg.Cell(rowItemIndex, colItemIndex).SetValue(item.VegetableCount);
                                    }
                                    colItemIndex++;
                                });
                            }
                            wsFruit.Cell(rowItemIndex, colItemIndex).SetValue(oi.FruitCount);
                            wsVeg.Cell(rowItemIndex, colItemIndex).SetValue(oi.VegetableCount);
                            rowItemIndex++;

                        });
                        var reportHeaderRanges = workBook.NamedRanges.NamedRange("reportHeader");
                        if (reportHeaderRanges != null)
                        {
                            reportHeaderRanges.Ranges.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            reportHeaderRanges.Ranges.Style.Font.Bold = true;
                            reportHeaderRanges.Ranges.Style.Font.FontSize = 16;
                        }

                        var mealTypeRanges = workBook.NamedRanges.NamedRange("dayRange");
                        if (mealTypeRanges != null)
                        {
                            mealTypeRanges.Ranges.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            mealTypeRanges.Ranges.Style.Font.Bold = true;
                            mealTypeRanges.Ranges.Style.Font.FontSize = 14;
                        }

                        var menuTypeRanges = workBook.NamedRanges.NamedRange("tableHeaderRange");
                        if (menuTypeRanges != null)
                        {
                            menuTypeRanges.Ranges.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            menuTypeRanges.Ranges.Style.Font.Bold = true;
                            menuTypeRanges.Ranges.Style.Font.FontSize = 10;
                        }

                        var numberFormatRanges = workBook.NamedRanges.NamedRange("numberFormat");
                        if (numberFormatRanges != null)
                        {
                            numberFormatRanges.Ranges.Style.NumberFormat.Format = "#,##0";
                            numberFormatRanges.Ranges.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            numberFormatRanges.Ranges.Style.Font.FontSize = 8;
                        }

                        var tableContentRanges = workBook.NamedRanges.NamedRange("tableContent");
                        if (tableContentRanges != null)
                        {
                            tableContentRanges.Ranges.Style.Border.InsideBorderColor = XLColor.FromArgb(0, 0, 221);
                            tableContentRanges.Ranges.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                            tableContentRanges.Ranges.Style.Border.OutsideBorderColor = XLColor.FromArgb(0, 0, 139);
                            tableContentRanges.Ranges.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                        }
                        wsFruit.Columns().AdjustToContents();
                        wsVeg.Columns().AdjustToContents();
                        workBook.SaveAs(filePath);


                    }
                    response.FileName = filePath;
                });
        }

        public ReportResponse DateRangeOrderItemExport(DateRangeOrderItemRequest request)
        {
            return Execute<DateRangeOrderItemRequest, ReportResponse>(
                request,
                response =>
                {

                    var orderItems = _mealMenuOrderFacade.GetDateRenageOrderItems(request.Filter);
                    var fileRepository = FileRepositoryPath;//_appSetting.GetSetting<string>("FileRepositoryPath");
                    var billingReportFolder = Path.Combine(fileRepository, "BillingReport");
                    if (!Directory.Exists(billingReportFolder))
                        Directory.CreateDirectory(Path.Combine(fileRepository, "BillingReport"));
                    var startDate = request.Filter.StartDate ?? DateTime.Now.AddDays(-7);
                    var endDate = request.Filter.EndDate ?? DateTime.Now;
                    var filePath = Path.Combine(billingReportFolder,
                        string.Format("{0}_{1}_BillingReport.xlsx", startDate.ToString("yyyyMMdd"),
                            endDate.ToString("yyyyMMdd")));
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    using (var workBook = new XLWorkbook(XLEventTracking.Disabled))
                    {
                        var workSheet = workBook.AddWorksheet("BillingReport");
                        var meals = Lookups.GetItems<MealTypes>().Where(d => d.Id > 0).OrderBy(d => d.Id).ToList();
                        var rowIndex = 5;
                        var colIndex = 2;
                        var counter = 0;
                        meals.ForEach(m =>
                        {
                            var mealMenus =
                                Lookups.MealMenuTypeList.Where(d => (long)d.Key == m.Id)
                                    .SelectMany(
                                        d => d.Value.Where(k => k != 0 && k != MenuTypes.Milk).Select(k => (long)k))
                                    .OrderBy(d => d)
                                    .ToList();

                            var mealTypeRange = workSheet.Range(rowIndex - 1, colIndex + counter, rowIndex - 1,
                                colIndex + counter + mealMenus.Count * 2 - 1);

                            mealTypeRange.Merge().AddToNamed("mealType");
                            mealTypeRange.SetValue(m.Text);
                            mealMenus.ForEach(mm =>
                            {
                                var menuTypeRange =
                                    workSheet.Range(rowIndex, colIndex + counter, rowIndex, colIndex + counter + 1)
                                        .Merge()
                                        .AddToNamed("menuType");
                                menuTypeRange.SetValue(Lookups.GetItem<MenuTypes>(mm).Text);

                                workSheet.Cell(rowIndex + 1, colIndex + counter).SetValue("Count");
                                workSheet.Range(rowIndex + 2, colIndex + counter, rowIndex + 2 + orderItems.Count,
                                    colIndex + counter).AddToNamed("numberFormat");
                                counter++;
                                workSheet.Cell(rowIndex + 1, colIndex + counter).SetValue("Price");
                                workSheet.Range(rowIndex + 2, colIndex + counter, rowIndex + 2 + orderItems.Count,
                                    colIndex + counter).AddToNamed("decimalFormat");
                                counter++;
                            });

                        });

                        workSheet.Cell(rowIndex + 1, colIndex + counter).SetValue("Total Price");
                        workSheet.Range(rowIndex + 2, colIndex + counter, rowIndex + 2 + orderItems.Count,
                            colIndex + counter).AddToNamed("decimalFormat");

                        workSheet.Cell(rowIndex + 1, 1).SetValue("School");
                        var tableSubColumnRange =
                            workSheet.Range(rowIndex + 1, 1, rowIndex + 1, 2 + counter).AddToNamed("tableSubColumn");
                        tableSubColumnRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        tableSubColumnRange.Style.Font.Bold = true;
                        tableSubColumnRange.Style.Font.FontSize = 10;

                        workSheet.Range(rowIndex + 1, 1, rowIndex + 1 + orderItems.Count, 2 + counter)
                            .AddToNamed("tableContent");
                        var reportHeaderRange =
                            workSheet.Range(rowIndex - 2, 1, rowIndex - 2, 2 + counter)
                                .Merge()
                                .AddToNamed("reportHeader");
                        reportHeaderRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        reportHeaderRange.Style.Font.Bold = true;
                        reportHeaderRange.Style.Font.FontSize = 16;
                        reportHeaderRange.SetValue(string.Format("INVOICE ITEMS , {0} - {1}",
                            startDate.ToString("yyyy-MM-dd"),
                            endDate.ToString("yyyy-MM-dd")));

                        var rowItemIndex = 7;

                        orderItems.ForEach(oi =>
                        {
                            workSheet.Cell(rowItemIndex, 1).SetValue(oi.SchoolName);
                            var colItemIndex = 2;
                            meals.ForEach(m =>
                            {
                                var mealMenus =
                                    Lookups.MealMenuTypeList.Where(d => (long)d.Key == m.Id)
                                        .SelectMany(
                                            d => d.Value.Where(k => k != 0 && k != MenuTypes.Milk).Select(k => (long)k))
                                        .OrderBy(d => d)
                                        .ToList();
                                var mealOrderItems = oi.MealList.FirstOrDefault(ml => ml.MealType.Id == m.Id);
                                mealMenus.ForEach(mm =>
                                {

                                    if (mealOrderItems == null)
                                    {
                                        workSheet.Cell(rowItemIndex, colItemIndex).SetValue(0);
                                        colItemIndex++;
                                        workSheet.Cell(rowItemIndex, colItemIndex).SetValue(0.00);
                                    }

                                    else
                                    {
                                        var menuOrderItems =
                                            mealOrderItems.MenuList.FirstOrDefault(ml => ml.MenuType.Id == mm);
                                        if (menuOrderItems == null)
                                        {
                                            workSheet.Cell(rowItemIndex, colItemIndex).SetValue(0);
                                            colItemIndex++;
                                            workSheet.Cell(rowItemIndex, colItemIndex).SetValue(0.00);
                                        }
                                        else
                                        {
                                            workSheet.Cell(rowItemIndex, colItemIndex)
                                                .SetValue(menuOrderItems.TotalCount);
                                            colItemIndex++;
                                            workSheet.Cell(rowItemIndex, colItemIndex)
                                                .SetValue(menuOrderItems.TotalPrice);
                                        }
                                    }
                                    colItemIndex++;
                                });

                            });
                            workSheet.Cell(rowItemIndex, colItemIndex).SetValue(oi.MealList.Sum(d => d.TotalPrice));
                            rowItemIndex++;

                        });
                        var mealTypeRanges = workBook.NamedRanges.NamedRange("mealType");
                        if (mealTypeRanges != null)
                        {
                            mealTypeRanges.Ranges.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            mealTypeRanges.Ranges.Style.Font.Bold = true;
                            mealTypeRanges.Ranges.Style.Font.FontSize = 14;
                        }
                        var menuTypeRanges = workBook.NamedRanges.NamedRange("menuType");
                        if (menuTypeRanges != null)
                        {
                            menuTypeRanges.Ranges.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            menuTypeRanges.Ranges.Style.Font.Bold = true;
                            menuTypeRanges.Ranges.Style.Font.FontSize = 12;
                        }

                        var numberFormatRanges = workBook.NamedRanges.NamedRange("numberFormat");
                        if (numberFormatRanges != null)
                        {
                            numberFormatRanges.Ranges.Style.NumberFormat.Format = "#,##0";
                            numberFormatRanges.Ranges.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        }
                        var decimalFormatRanges = workBook.NamedRanges.NamedRange("decimalFormat");
                        if (decimalFormatRanges != null)
                        {
                            decimalFormatRanges.Ranges.Style.NumberFormat.Format = "#,##0.00";
                            decimalFormatRanges.Ranges.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        }
                        var tableContentRanges = workBook.NamedRanges.NamedRange("tableContent");
                        if (tableContentRanges != null)
                        {
                            tableContentRanges.Ranges.Style.Border.InsideBorderColor = XLColor.FromArgb(0, 0, 221);
                            tableContentRanges.Ranges.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                            tableContentRanges.Ranges.Style.Border.OutsideBorderColor = XLColor.FromArgb(0, 0, 139);
                            tableContentRanges.Ranges.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                        }
                        workSheet.Columns().AdjustToContents();
                        workBook.SaveAs(filePath);


                    }
                    response.FileName = filePath;
                });
        }

        public ReportResponse SchoolMenuExport(SchoolMenuExportRequest request)
        {
            return Execute<SchoolMenuExportRequest, ReportResponse>(
                request,
                response =>
                {
                    var filter = new MealMenuOrderFilterView
                    {
                        OrderDate = request.Filter.OrderDate,
                        RecordStatusId = (int)Config.RecordStatuses.Active,
                        SchoolId = request.Filter.SchoolId,
                        SchoolType = request.Filter.SchoolType,
                        MealTypeId = request.Filter.MealTypeId
                    };


                    var result = _mealMenuOrderService.GetSchoolOrder(new SchoolOrderGetRequest { Filter = filter });
                    if (result.Result == Result.Success)
                    {
                        var fileRepository = FileRepositoryPath;//_appSetting.GetSetting<string>("FileRepositoryPath");
                        var templateFile = Path.Combine(fileRepository, "Templates", "SchoolMenuExport.xlsx");
                        var directoryPath = Path.Combine(fileRepository, "SchoolMenuExport");
                        if (!Directory.Exists(directoryPath))
                            Directory.CreateDirectory(directoryPath);

                        var fileName = string.Format("{0}_{1}_{2}_{3}.xlsx",
                            request.Filter.SchoolId,
                            request.Filter.MealTypeId,
                            request.Filter.OrderDate.ToString("yyyyMMdd"),
                            Directory.GetFiles(directoryPath).Length);

                        var filePath = Path.Combine(directoryPath, fileName);
                        File.Copy(templateFile, filePath, true);
                        var spreadsheet = SpreadsheetDocument.Open(filePath, true);


                        var sheetData =
                            spreadsheet.WorkbookPart.GetPartsOfType<WorksheetPart>()
                                .First()
                                .Worksheet.GetFirstChild<SheetData>();

                        //TODO BEGIN TO WRITE EXCEL
                        var menuInfoCell = sheetData.Descendants<Cell>().First(c => c.CellReference == "B2");
                        menuInfoCell.DataType = CellValues.InlineString;
                        menuInfoCell.InlineString = new InlineString
                        {
                            Text =
                                new Text
                                {
                                    Text = Lookups.GetItem<MealTypes>(request.Filter.MealTypeId).Text + " Menu"
                                }
                        };

                        var cellMonth = sheetData.Descendants<Cell>().First(c => c.CellReference == "B4");
                        cellMonth.InlineString = new InlineString
                        {
                            Text =
                                new Text
                                {
                                    Text =
                                        String.Format("{0} {1}",
                                            CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(
                                                request.Filter.OrderDate.Month), request.Filter.OrderDate.Year)
                                }
                        };
                        cellMonth.DataType = CellValues.InlineString;

                        var dayCols = new List<string> { "B", "E", "H", "K", "N" };
                        var tempDate = new DateTime(request.Filter.OrderDate.Year, request.Filter.OrderDate.Month, 1);
                        var daysInMonth = DateTime.DaysInMonth(tempDate.Year, tempDate.Month);
                        while (tempDate.DayOfWeek == DayOfWeek.Saturday || tempDate.DayOfWeek == DayOfWeek.Sunday)
                            tempDate = tempDate.AddDays(1);

                        var startDay = tempDate.Day;
                        var firstDay = new DateTime(tempDate.Year, tempDate.Month, 1);
                        var diff = (firstDay.DayOfWeek == DayOfWeek.Saturday)
                            ? 1
                            : 0;
                        for (var i = startDay; i <= daysInMonth; i++)
                        {
                            tempDate = new DateTime(tempDate.Year, tempDate.Month, i);
                            if (tempDate.DayOfWeek == DayOfWeek.Saturday || tempDate.DayOfWeek == DayOfWeek.Sunday)
                                continue;

                            var columnNumber = (tempDate.GetWeekOfMonth() * 7) + 1; // - (diff*5);
                            columnNumber = columnNumber - (diff * 7);
                            var dataColumnNumber = (tempDate.GetWeekOfMonth() * 7) + 2;
                            dataColumnNumber = dataColumnNumber - (diff * 7);
                            var dayColsIndex = (int)tempDate.DayOfWeek - 1;
                            //if (firstDay.DayOfWeek == DayOfWeek.Saturday || firstDay.DayOfWeek == DayOfWeek.Sunday)
                            //    dayColsIndex = dayColsIndex - 1;

                            var columnIndex = string.Format("{0}{1}", dayCols[dayColsIndex], columnNumber);
                            var dataColumnIndex = string.Format("{0}{1}", dayCols[(int)tempDate.DayOfWeek - 1],
                                dataColumnNumber);

                            var cellMenu = sheetData.Descendants<Cell>().First(c => c.CellReference == columnIndex);
                            cellMenu.InlineString = new InlineString { Text = new Text { Text = i.ToString() } };
                            cellMenu.DataType = CellValues.InlineString;

                            //var orderItems = result.Order.OrderItems.Where(c => c.MealMenuValidDate.Date == tempDate);
                            var orderDate = result.Order.Days.FirstOrDefault(c => c.Date == tempDate);
                            if (orderDate != null)
                            {
                                var mealMenuOrderItemViews = orderDate.Items;

                                if (mealMenuOrderItemViews.Any())
                                {
                                    var sb = new StringBuilder();
                                    sb = mealMenuOrderItemViews
                                        .Where(item => item.Count != 0)
                                        .Aggregate(sb,
                                            (current, item) =>
                                                current.Append(string.Format("{0} ({1})\r\n", item.MenuName,
                                                    item.Count)));

                                    var cellMeal =
                                        sheetData.Descendants<Cell>().First(c => c.CellReference == dataColumnIndex);
                                    cellMeal.InlineString = new InlineString { Text = new Text { Text = sb.ToString() } };
                                    cellMeal.DataType = CellValues.InlineString;
                                }
                            }

                        }

                        spreadsheet.WorkbookPart.Workbook.Save();

                        spreadsheet.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
                        spreadsheet.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;
                        spreadsheet.Close();

                        response.FileName = filePath;
                    }
                });
        }

        public ReportResponse MontlyMilkExport(MontlyMilkExportRequest request)
        {
            return Execute<MontlyMilkExportRequest, ReportResponse>(
                request,
                response =>
                {
                    int totalCount;
                    if (!request.Filter.OrderStartDate.HasValue)
                        throw new Exception("Order StartDate Is Null");
                    var orderStartDate = request.Filter.OrderStartDate.Value;
                    var eMealType = (MealTypes)request.Filter.MealTypeId;


                    var orders = _mealMenuOrderFacade.GetOrderReport(request.Filter, int.MaxValue, 1, "Route", true, out totalCount);

                    var fileRepository = FileRepositoryPath;
                    var templateFile = Path.Combine(fileRepository, "Templates", "MilkItem.xlsx");
                    var fileFolder = Path.Combine(fileRepository, "MonthlyOrder");
                    if (!Directory.Exists(fileFolder))
                        Directory.CreateDirectory(fileFolder);

                    var milkTotalCount = 0;
                    var milkMenus = _menuFacade.GetByFilter(new MenuFilterView { MenuTypeId = (int)MenuTypes.Milk, RecordStatusId = (int)Meal.Config.RecordStatuses.Active }, 0, 1, "Name", true, out milkTotalCount);

                    var filePath = Path.Combine(fileFolder, string.Format("{0}_{1}_Milk.xlsx", orderStartDate.ToString("yyyy-MMM"), eMealType.ToString("G")));
                    File.Copy(templateFile, filePath, true);
                    File.SetAttributes(filePath, FileAttributes.Normal | FileAttributes.Archive);
                    using (var workBook = new XLWorkbook(filePath, XLEventTracking.Disabled))
                    {
                        var wsMeal = workBook.Worksheets.First();

                        var lastDayOfMonth = DateTime.DaysInMonth(orderStartDate.Year, orderStartDate.Month);
                        if (request.Filter.OrderEndDate.HasValue)
                            lastDayOfMonth = request.Filter.OrderEndDate.Value.Day;

                        var headerTitle = string.Format("[{1} - {2}-{3}] {0} Order -  Milk Menu", eMealType.ToString("G"), orderStartDate.ToString("yyyy-MM-dd"), orderStartDate.ToString("yyyy-MM"), lastDayOfMonth);
                        var orderList =
                            orders.Where(o => o.MealTypeId == (int)eMealType /*&& o.Items.Any(i => i.Menus.Any(m => m.MenuTypeId == (int)MenuTypes.Milk))*/)
                                .OrderBy(k => k.SchoolRoute)
                                .ThenBy(k => k.SchoolName)
                                .ToList();

                        var rowStart = 6;
                        var columnStart = 4;
                        for (var day = orderStartDate.Day; day <= lastDayOfMonth; day++)
                        {
                            var currentDay = new DateTime(orderStartDate.Year, orderStartDate.Month, day);
                            if (currentDay.DayOfWeek == DayOfWeek.Saturday ||
                                currentDay.DayOfWeek == DayOfWeek.Sunday)
                                continue;
                            var weekDayRange = wsMeal.Range(rowStart - 1, columnStart, rowStart - 1, columnStart + milkMenus.Count).Merge();
                            weekDayRange.SetValue(currentDay.DayOfWeek.ToString().Substring(0, 2));

                            var monthDayRange = wsMeal.Range(rowStart, columnStart, rowStart, columnStart + milkMenus.Count).Merge();
                            monthDayRange.SetValue(day.ToString());

                            wsMeal.Cell(rowStart + 1, columnStart).SetValue("Total");
                            columnStart++;
                            for (var mm = 0; mm < milkMenus.Count; mm++)
                            {
                                wsMeal.Cell(rowStart + 1, columnStart + mm).SetValue(milkMenus[mm].Name);
                            }
                            columnStart += milkMenus.Count;
                        }



                        var tableHeaderRange = wsMeal.Range(rowStart - 1, 4, rowStart + 1, columnStart - 1).AddToNamed("tableHeader");
                        tableHeaderRange.Style.Fill.BackgroundColor = XLColor.FromArgb(218, 238, 243);
                        tableHeaderRange.Style.Border.InsideBorderColor = XLColor.FromArgb(0, 0, 221);
                        tableHeaderRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        tableHeaderRange.Style.Border.OutsideBorderColor = XLColor.FromArgb(0, 0, 139);
                        tableHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                        var headerTitleRange =
                            wsMeal.Range(2, 6, 3, 20).Merge().AddToNamed("headerTitle");
                        headerTitleRange.SetValue(headerTitle);
                        headerTitleRange.Style.Fill.BackgroundColor = XLColor.FromArgb(67, 255, 152);
                        headerTitleRange.Style.Font.SetFontSize(16);
                        headerTitleRange.Style.Font.SetBold(true);
                        headerTitleRange.Style.Font.SetItalic(true);
                        headerTitleRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        headerTitleRange.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);



                        var orderRowStart = 8;
                        var orderColumnStart = 1;
                        orderList.ForEach(o =>
                        {
                            var schoolRoute = o.SchoolRoute;
                            wsMeal.Cell(orderRowStart, orderColumnStart).SetValue(schoolRoute);
                            wsMeal.Cell(orderRowStart, orderColumnStart + 1).SetValue(o.SchoolType);
                            wsMeal.Cell(orderRowStart, orderColumnStart + 2).SetValue(o.SchoolName);
                            orderColumnStart += 3;
                            for (var day = orderStartDate.Day; day <= lastDayOfMonth; day++)
                            {
                                var currentDay = new DateTime(orderStartDate.Year, orderStartDate.Month, day);
                                if (currentDay.DayOfWeek == DayOfWeek.Saturday ||
                                    currentDay.DayOfWeek == DayOfWeek.Sunday)
                                    continue;

                                var dateMenus = o.Items.Where(m => m.Date == currentDay)
                                    .SelectMany(m => m.Menus)
                                    .Where(t => t.MenuTypeId == (int)MenuTypes.Milk).Select(m => m)
                                    .ToList();
                                if (dateMenus.Any())
                                    wsMeal.Cell(orderRowStart, orderColumnStart).SetValue(dateMenus.Sum(t => t.TotalCount));
                                orderColumnStart++;
                                for (var mm = 0; mm < milkMenus.Count; mm++)
                                {
                                    var milkMenu = dateMenus.FirstOrDefault(dm => dm.Name == milkMenus[mm].Name);
                                    if (milkMenu != null)
                                        wsMeal.Cell(orderRowStart, orderColumnStart + mm).SetValue(milkMenu.TotalCount);
                                }
                                orderColumnStart += milkMenus.Count;
                            }
                            orderColumnStart = 1;
                            orderRowStart++;

                        });



                        var tableContent =
                            wsMeal.Range(8, 1, orderRowStart - 1, columnStart - 1).AddToNamed("tableContent");
                        tableContent.Style.Border.InsideBorderColor = XLColor.FromArgb(0, 0, 221);
                        tableContent.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        tableContent.Style.Border.OutsideBorderColor = XLColor.FromArgb(0, 0, 139);
                        tableContent.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;


                        wsMeal.Range(6, 1, 6, 3)
                            .Merge()
                            .SetValue(string.Format("Report Date : {0:G}", DateTime.Now));

                        rowStart = orderRowStart + 2;
                        columnStart = 4;
                        for (var day = orderStartDate.Day; day <= lastDayOfMonth; day++)
                        {
                            var currentDay = new DateTime(orderStartDate.Year, orderStartDate.Month, day);
                            if (currentDay.DayOfWeek == DayOfWeek.Saturday ||
                                currentDay.DayOfWeek == DayOfWeek.Sunday)
                                continue;
                            var weekDayRange = wsMeal.Range(rowStart + 1, columnStart, rowStart + 1, columnStart + milkMenus.Count).Merge();
                            weekDayRange.SetValue(currentDay.DayOfWeek.ToString().Substring(0, 2));

                            var monthDayRange = wsMeal.Range(rowStart, columnStart, rowStart, columnStart + milkMenus.Count).Merge();
                            monthDayRange.SetValue(day.ToString());

                            var dateMenus = orderList.SelectMany(k => k.Items.Where(m => m.Date == currentDay)
                                .SelectMany(m => m.Menus.Where(l => l.MenuTypeId == (int)MenuTypes.Milk)))
                                .Select(m => m)
                                .ToList();


                            wsMeal.Cell(rowStart - 1, columnStart).SetValue("Total");
                            wsMeal.Cell(rowStart - 2, columnStart).SetValue(dateMenus.Sum(k => k.TotalCount));

                            columnStart++;
                            for (var mm = 0; mm < milkMenus.Count; mm++)
                            {
                                wsMeal.Cell(rowStart - 1, columnStart + mm).SetValue(milkMenus[mm].Name);
                                wsMeal.Cell(rowStart - 2, columnStart + mm).SetValue(dateMenus.Where(k => k.Name == milkMenus[mm].Name).Sum(k => k.TotalCount));
                            }

                            columnStart += milkMenus.Count;
                        }
                        var footerRow = wsMeal.Range(orderRowStart, 4, rowStart + 1, columnStart - 1).AddToNamed("footerRow");
                        footerRow.Style.Font.SetBold(true);
                        footerRow.Style.Font.SetItalic(true);
                        footerRow.Style.Border.InsideBorderColor = XLColor.FromArgb(0, 0, 221);
                        footerRow.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                        footerRow.Style.Border.OutsideBorderColor = XLColor.FromArgb(0, 0, 139);
                        footerRow.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;


                        wsMeal.PageSetup.PageOrientation = XLPageOrientation.Landscape;
                        wsMeal.PageSetup.PagesWide = 1;
                        wsMeal.Columns().AdjustToContents();

                        workBook.Save();
                    }
                    response.FileName = filePath;
                });
        }
    }
}