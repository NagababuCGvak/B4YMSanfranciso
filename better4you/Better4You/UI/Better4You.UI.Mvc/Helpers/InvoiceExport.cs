using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Better4You.Meal.Config;
using Better4You.Meal.Service;
using Better4You.Meal.Service.Messages;
using Better4You.Meal.ViewModel;
using Better4You.UI.Mvc.Models;
using Better4You.UserManagment.Service;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using RecordStatuses = Better4You.Core.RecordStatuses;

namespace Better4You.UI.Mvc.Helpers
{
    public class InvoiceExport : IDisposable
    {
        public string FilePath { get; set; }
        public string TemplateFile { get; set; }
        public SpreadsheetDocument InvoiceSheet { get; set; }
        private SheetData sheetData = null;

        public IMealMenuOrderService OrderService { get; set; }
        public ISchoolService SchoolService { get; set; }

        #region Rates
        double _breakfastRate = 0;
        double _lunchRate = 0;
        double _snackRate = 0;
        double _supperRate = 0;
        double _sackRate = 0;
        private double _soyMilkRate = 0;

        double breakfastCreditRate = 0;
        double lunchCreditRate = 0;
        double snackCreditRate = 0;
        double supperCreditRate = 0;
        double sackCreditRate = 0;
        #endregion

        #region Adjustments
        double adjustment = 0;
        double breakfastAdjustment = 0;
        double snackAdjustment = 0;
        double lunchAdjustment = 0;
        double sackLunchAdjustment = 0;
        double supperAdustment = 0;
        #endregion

        #region Meal Total
        int totalBreakfast = 0;
        int totalBreakfastSpecial = 0;
        int totalBreakfastVegetarian = 0;

        int totalLunch = 0;
        int totalLunchSpecial = 0;
        int totalLunchVegetarian = 0;



        int totalSnack = 0;
        int totalSupper = 0;
        #endregion

        private List<MealMenuOrderReportResponse> orderReports = new List<MealMenuOrderReportResponse>();
        private InvoiceExportViewModel model = new InvoiceExportViewModel();

        List<DateTime> orderDates = new List<DateTime>();


        readonly Dictionary<DateTime, int> breakfastOrderByDate = new Dictionary<DateTime, int>();
        readonly Dictionary<DateTime, int> breakfastSpecialOrderByDate = new Dictionary<DateTime, int>();
        readonly Dictionary<DateTime, int> breakfastVeggyOrderByDate = new Dictionary<DateTime, int>();
        readonly Dictionary<DateTime, int> lunchOrderByDate = new Dictionary<DateTime, int>();
        readonly Dictionary<DateTime, int> lunchSpecialOrderByDate = new Dictionary<DateTime, int>();
        readonly Dictionary<DateTime, int> lunchVeggyOrderByDate = new Dictionary<DateTime, int>();
        readonly Dictionary<DateTime, int> snackOrderByDate = new Dictionary<DateTime, int>();
        readonly Dictionary<DateTime, int> supperOrderByDate = new Dictionary<DateTime, int>();

        public InvoiceExport(InvoiceExportViewModel model, string filePath,
            string templateFile, IMealMenuOrderService OrderService, ISchoolService SchoolService)
        {
            this.FilePath = filePath;
            this.TemplateFile = templateFile;
            this.OrderService = OrderService;
            this.SchoolService = SchoolService;

            this.model = model;
            orderReports = model.orderReports;
            InitializeRates();
            InitializeAdjustments();
            ParseOrdersIntoMealTypesAndDates();
            //ParseOrdersIntoMealTypes();
            //InitalizeTotalMeals();
            OpenInvoiceSheet();
        }

        private void ParseOrdersIntoMealTypesAndDates()
        {
            List<OrderReportView> breakfastOrders = new List<OrderReportView>();
            List<OrderReportView> lunchOrders = new List<OrderReportView>();
            List<OrderReportView> snackOrders = new List<OrderReportView>();
            List<OrderReportView> supperOrders = new List<OrderReportView>();
            List<OrderReportView> sackLuncOrders = new List<OrderReportView>();

            try
            {
                breakfastOrders = orderReports.FirstOrDefault(c => c.Orders.Any(a => a.MealTypeId == (int)MealTypes.Breakfast)).Orders;
            }
            catch { }
            try
            {
                lunchOrders = orderReports.FirstOrDefault(c => c.Orders.Any(a => a.MealTypeId == (int)MealTypes.Lunch)).Orders;

            }
            catch { }
            try
            {
                sackLuncOrders = orderReports.FirstOrDefault(c => c.Orders.Any(a => a.MealTypeId == (int)MealTypes.SackLunch)).Orders;
                sackLuncOrders.ForEach(c => lunchOrders.Add(c));
            }
            catch { }

            try
            {
                snackOrders = orderReports.FirstOrDefault(c => c.Orders.Any(a => a.MealTypeId == (int)MealTypes.Snack)).Orders;
            }

            catch { }


            try
            {
                supperOrders = orderReports.FirstOrDefault(c => c.Orders.Any(a => a.MealTypeId == (int)MealTypes.Supper)).Orders;
            }
            catch { }


            foreach (DateTime dt in orderDates)
            {

                var breakfastStandardCount = (from b in breakfastOrders
                                              from i in b.Items
                                              from m in i.Menus
                                              where ((i.Date.Day == dt.Day && i.Date.Month == dt.Month) &&
                                              (
                                                (m.MenuTypeId == (int)MenuTypes.Breakfast1) ||
                                                (m.MenuTypeId == (int)MenuTypes.Breakfast2) ||
                                                (m.MenuTypeId == (int)MenuTypes.Breakfast3) ||
                                                (m.MenuTypeId == (int)MenuTypes.Breakfast4) ||
                                                (m.MenuTypeId == (int)MenuTypes.Breakfast5) ||
                                                (m.MenuTypeId == (int)MenuTypes.ComptonBreakfast) ||
                                                (m.MenuTypeId == (int)MenuTypes.PancakeBk)))
                                              select m).Sum(s => s.TotalCount);

                var breakfastSpecialCount = (from b in breakfastOrders
                                             from i in b.Items
                                             from m in i.Menus
                                             where ((i.Date.Day == dt.Day && i.Date.Month == dt.Month) && (m.MenuTypeId == (int)MenuTypes.Special))
                                             select m).Sum(s => s.TotalCount);

                var breakfastVeggyCount = (from b in breakfastOrders
                                           from i in b.Items
                                           from m in i.Menus
                                           where ((i.Date.Day == dt.Day && i.Date.Month == dt.Month) && (m.MenuTypeId == (int)MenuTypes.Vegetarian))
                                           select m).Sum(s => s.TotalCount);

                int lunchCount = (from b in lunchOrders
                                  from i in b.Items
                                  from m in i.Menus
                                  where ((i.Date.Day == dt.Day && i.Date.Month == dt.Month) &&
                                         (
                                            (m.MenuTypeId == (int)MenuTypes.LunchOption1) ||
                                            (m.MenuTypeId == (int)MenuTypes.LunchOption2) ||
                                            (m.MenuTypeId == (int)MenuTypes.LunchOption3) ||
                                            (m.MenuTypeId == (int)MenuTypes.LunchOption4) ||
                                            (m.MenuTypeId == (int)MenuTypes.LunchOption5) ||
                                            (m.MenuTypeId == (int)MenuTypes.PickupStix) ||
                                            (m.MenuTypeId == (int)MenuTypes.Bbq) ||
                                            (m.MenuTypeId == (int)MenuTypes.Other) ||
                                            (m.MenuTypeId == (int)MenuTypes.ComptonLunch) ||
                                            (m.MenuTypeId == (int)MenuTypes.ComptonSack) ||
                                            (m.MenuTypeId == (int)MenuTypes.SackLunch1) ||
                                            (m.MenuTypeId == (int)MenuTypes.SackLunch2)
                                            ))
                                  select m).Sum(s => s.TotalCount);

                var lunchCountSpecial = (from b in lunchOrders
                                         from i in b.Items
                                         from m in i.Menus
                                         where ((i.Date.Day == dt.Day && i.Date.Month == dt.Month) &&
                                                (m.MenuTypeId == (int)MenuTypes.Special))
                                         select m).Sum(s => s.TotalCount);

                var lunchCountVeggy = (from b in lunchOrders
                                       from i in b.Items
                                       from m in i.Menus
                                       where ((i.Date.Day == dt.Day && i.Date.Month == dt.Month) &&
                                              (m.MenuTypeId == (int)MenuTypes.Vegetarian))
                                       select m).Sum(s => s.TotalCount);
                int snackCount = 0;
                snackCount = (from b in snackOrders
                                  from i in b.Items
                                  from m in i.Menus
                                  where ((i.Date.Day == dt.Day && i.Date.Month == dt.Month) &&
                                         ((m.MenuTypeId == (int)MenuTypes.Snack1) ||
                                         (m.MenuTypeId == (int)MenuTypes.Snack2)))
                                  select m).Sum(s => s.TotalCount);

                var supperCount = (from b in supperOrders
                                   from i in b.Items
                                   from m in i.Menus
                                   where ((i.Date.Day == dt.Day && i.Date.Month == dt.Month) &&
                                   (
                                    (m.MenuTypeId == (int)MenuTypes.Supper1) ||
                                    (m.MenuTypeId == (int)MenuTypes.Supper2) ||
                                    (m.MenuTypeId == (int)MenuTypes.ComptonSupper)))
                                   select m).Sum(s => s.TotalCount);

                breakfastOrderByDate[dt] = breakfastStandardCount;
                breakfastVeggyOrderByDate[dt] = breakfastVeggyCount;
                breakfastSpecialOrderByDate[dt] = breakfastSpecialCount;
                lunchOrderByDate[dt] = lunchCount;
                lunchSpecialOrderByDate[dt] = lunchCountSpecial;
                lunchVeggyOrderByDate[dt] = lunchCountVeggy;
                snackOrderByDate[dt] = snackCount;
                supperOrderByDate[dt] = supperCount;
            }

        }

        private void InitializeRates()
        {
            //var annualAggrements1 = model.orderReports.ToList();
            var annualAggrements = model.School.SchoolAnnualAgreements.Where(c => c.RecordStatus.Text == RecordStatuses.Active.ToString()).Where(c => c.Year == model.orderReports[0].Orders[0].OrderDate.Year).ToList();
            var bbAnnualAgrement = annualAggrements.SingleOrDefault(c => c.ItemType.Id == (int)AnnualItemTypes.Breakfast);
            if (bbAnnualAgrement != null)
            {
                _breakfastRate = bbAnnualAgrement.Price;
            }

            var lnAnnualAgrement = annualAggrements.SingleOrDefault(c => c.ItemType.Id == (int)AnnualItemTypes.Lunch);
            //var orders = model.orderReports.SelectMany(x => x.Orders).ToList();

            //var breakfastRate = orders.Where(x => x.MealTypeId == 1).Select(y => y.Rate).FirstOrDefault();
            //var snacksRate = orders.Where(x => x.MealTypeId == 2).Select(y => y.Rate).FirstOrDefault();
            //var lunchRate = orders.Where(x => x.MealTypeId == 3).Select(y => y.Rate).FirstOrDefault();
            //var sackRate = orders.Where(x => x.MealTypeId == 4).Select(y => y.Rate).FirstOrDefault();
            //var supperRate = orders.Where(x => x.MealTypeId == 5).Select(y => y.Rate).FirstOrDefault();
            //var soyMilkRate = orders.Where(x => x.MealTypeId == 6).Select(y => y.Rate).FirstOrDefault();



            //Break Fast

            //_lunchRate = lunchRate != null ? Convert.ToDouble(lunchRate) : 0;
            //_snackRate = snacksRate != null ? Convert.ToDouble(snacksRate) : 0;
            //_breakfastRate = breakfastRate != null ? Convert.ToDouble(breakfastRate) : 0;
            //_sackRate = sackRate != null ? Convert.ToDouble(sackRate) : 0;
            //_supperRate = supperRate != null ? Convert.ToDouble(supperRate) : 0;
            //_soyMilkRate = soyMilkRate != null ? Convert.ToDouble(soyMilkRate) : 0;
            if (lnAnnualAgrement != null)
            {
                _lunchRate = lnAnnualAgrement.Price;

                //_lunchRate = temp.Rate != null ? Convert.ToDouble(temp.Rate) : 0;
            }

            var snackAnnualAgrement = annualAggrements.SingleOrDefault(c => c.ItemType.Id == (int)AnnualItemTypes.Snack);
            if (snackAnnualAgrement != null)
            {
                _snackRate = snackAnnualAgrement.Price;
            }

            var sackLunchAnnualAgrement = annualAggrements.SingleOrDefault(c => c.ItemType.Id == (int)AnnualItemTypes.SackLunch);
            if (sackLunchAnnualAgrement != null)
            {
                _sackRate = sackLunchAnnualAgrement.Price;
            }

            var supperAnnualAgrement = annualAggrements.SingleOrDefault(c => c.ItemType.Id == (int)AnnualItemTypes.Supper);
            if (supperAnnualAgrement != null)
            {
                _supperRate = supperAnnualAgrement.Price;
            }

            var soyMilkAgreement = annualAggrements.SingleOrDefault(c => c.ItemType.Id == (int)AnnualItemTypes.SoyMilk);
            if (soyMilkAgreement != null)
            {
                _soyMilkRate = soyMilkAgreement.Price;
            }


            foreach (var orderReport in orderReports)
            {
                foreach (var order in orderReport.Orders)
                {
                    double oRate = order.Rate ?? 0;
                    //double oRate = order.TotalCredit ?? 0;
                    switch (order.MealTypeId)
                    {
                        case (int)MealTypes.Breakfast:
                            _breakfastRate = oRate > 0 ? oRate : _breakfastRate;
                            //breakfastCreditRate = oRate;
                            break;
                        case (int)MealTypes.Lunch:
                            _lunchRate = oRate > 0 ? oRate : _lunchRate;
                            //lunchCreditRate = oRate;
                            break;
                        case (int)MealTypes.SackLunch:
                            _sackRate = oRate > 0 ? oRate : _sackRate;
                            //sackCreditRate = oRate;
                            break;
                        case (int)MealTypes.Snack:
                            _snackRate = oRate > 0 ? oRate : _snackRate;
                            //snackCreditRate = oRate;
                            break;
                        case (int)MealTypes.Supper:
                            _supperRate = oRate > 0 ? oRate : _supperRate;
                            //supperCreditRate = oRate;
                            break;

                    }

                    foreach (var item in order.Items)
                    {
                        if (!(orderDates.Any(c => c.Day == item.Date.Day && c.Month == item.Date.Month)))
                        {
                            orderDates.Add(item.Date);
                            breakfastOrderByDate.Add(item.Date, 0);
                            breakfastSpecialOrderByDate.Add(item.Date, 0);
                            breakfastVeggyOrderByDate.Add(item.Date, 0);
                            lunchOrderByDate.Add(item.Date, 0);
                            lunchSpecialOrderByDate.Add(item.Date, 0);
                            lunchVeggyOrderByDate.Add(item.Date, 0);
                            snackOrderByDate.Add(item.Date, 0);
                            supperOrderByDate.Add(item.Date, 0);
                        }

                    }
                }
            }
            orderDates = orderDates.OrderBy(c => c).ToList();
        }


        private void InitializeAdjustments()
        {
            adjustment = (from orderReport in orderReports
                          from order in orderReport.Orders
                          where order.TotalCredit.HasValue
                          select order.TotalCredit ?? 0).Sum();

            breakfastAdjustment = (from orderReport in orderReports
                                   from order in orderReport.Orders
                                   where ((order.MealTypeId == (int)MealTypes.Breakfast) && order.TotalCredit.HasValue)
                                   select order.TotalCredit ?? 0).Sum();

            snackAdjustment = (from orderReport in orderReports
                               from order in orderReport.Orders
                               where ((order.MealTypeId == (int)MealTypes.Snack) && order.TotalCredit.HasValue)
                               select order.TotalCredit ?? 0).Sum();

            lunchAdjustment = (from orderReport in orderReports
                               from order in orderReport.Orders
                               where ((order.MealTypeId == (int)MealTypes.Lunch) && order.TotalCredit.HasValue)
                               select order.TotalCredit ?? 0).Sum();

            sackLunchAdjustment = (from orderReport in orderReports
                                   from order in orderReport.Orders
                                   where ((order.MealTypeId == (int)MealTypes.SackLunch) && order.TotalCredit.HasValue)
                                   select order.TotalCredit ?? 0).Sum();

            supperAdustment = (from orderReport in orderReports
                               from order in orderReport.Orders
                               where ((order.MealTypeId == (int)MealTypes.Supper) && order.TotalCredit.HasValue)
                               select order.TotalCredit ?? 0).Sum();

        }

        private void OpenInvoiceSheet()
        {
            File.Copy(TemplateFile, FilePath, false);
            InvoiceSheet = SpreadsheetDocument.Open(FilePath, true);

            sheetData = InvoiceSheet.WorkbookPart.GetPartsOfType<WorksheetPart>().First().Worksheet.GetFirstChild<SheetData>();
        }

        public void UpdateInvoiceNumberInSheet()
        {
            var firstInvoice = orderReports[0];

            var invoiceNumber = string.Format("{0}-{1}", firstInvoice.Orders[0].OrderDate.ToString("MMyy"), firstInvoice.Orders[0].SchoolCode);
            var cellInvoiceNumber = sheetData.Descendants<Cell>().First(c => c.CellReference == "J2");
            cellInvoiceNumber.InlineString = new InlineString { Text = new Text { Text = invoiceNumber } };
            cellInvoiceNumber.DataType = CellValues.InlineString;

        }

        public void UpdateSchoolDataInSheet()
        {
            var cellSchoolName = sheetData.Descendants<Cell>().First(c => c.CellReference == "I5");
            cellSchoolName.InlineString = new InlineString { Text = new Text { Text = model.School.Name } };
            cellSchoolName.DataType = CellValues.InlineString;


            var cellSchoolAddress = sheetData.Descendants<Cell>().First(c => c.CellReference == "I6");
            cellSchoolAddress.InlineString = new InlineString { Text = new Text { Text = string.Format("{0}{1}", model.School.Address1, model.School.Address2) } };
            cellSchoolAddress.DataType = CellValues.InlineString;

            var cellSchoolLocation = sheetData.Descendants<Cell>().First(c => c.CellReference == "I7");
            cellSchoolLocation.InlineString = new InlineString { Text = new Text { Text = string.Format("{0},{1} {2}", model.School.City, model.School.FirstAdminDivision, model.School.ZipCode) } };
            cellSchoolLocation.DataType = CellValues.InlineString;

        }

        public void UpdateRatesInSheet()
        {
            var cellRate = sheetData.Descendants<Cell>().First(c => c.CellReference == "J10");
            cellRate.CellValue = new CellValue((_breakfastRate).ToString("N2"));
            cellRate.DataType = CellValues.Number;

            cellRate = sheetData.Descendants<Cell>().First(c => c.CellReference == "J11");
            cellRate.CellValue = new CellValue((_breakfastRate).ToString("N2"));
            cellRate.DataType = CellValues.Number;

            cellRate = sheetData.Descendants<Cell>().First(c => c.CellReference == "J12");
            cellRate.CellValue = new CellValue((_breakfastRate).ToString("N2"));
            cellRate.DataType = CellValues.Number;

            cellRate = sheetData.Descendants<Cell>().First(c => c.CellReference == "J13");
            cellRate.CellValue = new CellValue((_lunchRate).ToString("N2"));
            cellRate.DataType = CellValues.Number;

            cellRate = sheetData.Descendants<Cell>().First(c => c.CellReference == "J14");
            cellRate.CellValue = new CellValue((_lunchRate).ToString("N2"));
            cellRate.DataType = CellValues.Number;

            cellRate = sheetData.Descendants<Cell>().First(c => c.CellReference == "J15");
            cellRate.CellValue = new CellValue((_lunchRate).ToString("N2"));
            cellRate.DataType = CellValues.Number;

            cellRate = sheetData.Descendants<Cell>().First(c => c.CellReference == "J16");
            cellRate.CellValue = new CellValue((_snackRate).ToString("N2"));
            cellRate.DataType = CellValues.Number;

            cellRate = sheetData.Descendants<Cell>().First(c => c.CellReference == "J17");
            cellRate.CellValue = new CellValue((_supperRate).ToString("N2"));
            cellRate.DataType = CellValues.Number;

            cellRate = sheetData.Descendants<Cell>().First(c => c.CellReference == "J19");
            cellRate.CellValue = new CellValue((_soyMilkRate).ToString("N2"));
            cellRate.DataType = CellValues.Number;
        }

        public void UpdateDueDateInSheet()
        {
            var firstInvoice = orderReports[0];

            var dueDate = sheetData.Descendants<Cell>().First(c => c.CellReference == "K29");
            dueDate.InlineString = new InlineString { Text = new Text { Text = firstInvoice.Orders[0].OrderDate.AddMonths(2).ToString("d MMM yyyy") } };
            dueDate.DataType = CellValues.InlineString;

        }

        public void UpdateQuantitiesInSheet()
        {

            #region Breakfast Quantities
            var bbQuantity = sheetData.Descendants<Cell>().First(c => c.CellReference == "E10");
            bbQuantity.InlineString = new InlineString { Text = new Text { Text = totalBreakfast.ToString() } };
            bbQuantity.DataType = CellValues.InlineString;

            var bbVeggyQuantity = sheetData.Descendants<Cell>().First(c => c.CellReference == "E11");
            bbVeggyQuantity.InlineString = new InlineString { Text = new Text { Text = totalBreakfastVegetarian.ToString() } };
            bbVeggyQuantity.DataType = CellValues.InlineString;

            var bbSpecialQuantity = sheetData.Descendants<Cell>().First(c => c.CellReference == "E12");
            bbSpecialQuantity.InlineString = new InlineString { Text = new Text { Text = totalBreakfastSpecial.ToString() } };
            bbSpecialQuantity.DataType = CellValues.InlineString;
            #endregion

            #region Lunch Quantities
            var lunchQuantity = sheetData.Descendants<Cell>().First(c => c.CellReference == "E13");
            lunchQuantity.InlineString = new InlineString { Text = new Text { Text = totalLunch.ToString() } };
            lunchQuantity.DataType = CellValues.InlineString;

            var lunchVeggyQuantity = sheetData.Descendants<Cell>().First(c => c.CellReference == "E15");
            lunchVeggyQuantity.InlineString = new InlineString { Text = new Text { Text = totalLunchVegetarian.ToString() } };
            lunchVeggyQuantity.DataType = CellValues.InlineString;

            var lunchSpecialQuantity = sheetData.Descendants<Cell>().First(c => c.CellReference == "E14");
            lunchSpecialQuantity.InlineString = new InlineString { Text = new Text { Text = totalLunchSpecial.ToString() } };
            lunchSpecialQuantity.DataType = CellValues.InlineString;
            #endregion

            #region Snack Quantities
            var snackQuantity = sheetData.Descendants<Cell>().First(c => c.CellReference == "E16");
            snackQuantity.InlineString = new InlineString { Text = new Text { Text = totalSnack.ToString() } };
            snackQuantity.DataType = CellValues.InlineString;
            #endregion

            #region Supper Quantities
            var supperQuantity = sheetData.Descendants<Cell>().First(c => c.CellReference == "E17");
            supperQuantity.InlineString = new InlineString { Text = new Text { Text = totalSupper.ToString() } };
            supperQuantity.DataType = CellValues.InlineString;
            #endregion

            #region Soy Milk Quantities
            var soyMilkItems = orderReports.SelectMany(d => d.Orders).SelectMany(o => o.Items).SelectMany(i => i.Menus.Where(m => m.MenuTypeId == (int)AnnualItemTypes.SoyMilk));
            var soyMilkCount = soyMilkItems.Sum(sm => sm.TotalCount);
            var soyMilkQuantity = sheetData.Descendants<Cell>().First(c => c.CellReference == "E19");
            soyMilkQuantity.InlineString = new InlineString { Text = new Text { Text = soyMilkCount.ToString() } };
            soyMilkQuantity.DataType = CellValues.InlineString;
            #endregion Soy Milk Quantities
        }

        public void UpdateCreditsInSheet()
        {
            if (breakfastAdjustment > 0)
            {

                var bbCreditNo = sheetData.Descendants<Cell>().First(c => c.CellReference == "E20");
                bbCreditNo.DataType = CellValues.Number;
                //bbCreditNo.CellValue = new CellValue(breakfastAdjustment.ToString(CultureInfo.InvariantCulture));
                bbCreditNo.CellValue = new CellValue("1");

                var bbCredit = sheetData.Descendants<Cell>().First(c => c.CellReference == "J20");
                //bbCredit.CellValue = new CellValue((-1*breakfastCreditRate).ToString(CultureInfo.InvariantCulture));
                bbCredit.CellValue = new CellValue((-1 * breakfastAdjustment).ToString(CultureInfo.InvariantCulture));
                bbCredit.DataType = CellValues.Number;



                //var bbCreditNo = sheetData.Descendants<Cell>().First(c => c.CellReference == "E20");
                //bbCreditNo.DataType = CellValues.Number;
                //bbCreditNo.CellValue = new CellValue(breakfastAdjustment.ToString(CultureInfo.InvariantCulture));

                //var bbCredit = sheetData.Descendants<Cell>().First(c => c.CellReference == "J20");
                //bbCredit.CellValue = new CellValue((-1*breakfastCreditRate).ToString(CultureInfo.InvariantCulture));
                //bbCredit.DataType = CellValues.Number;
            }

            if (lunchAdjustment > 0)
            {

                var lnCreditNo = sheetData.Descendants<Cell>().First(c => c.CellReference == "E21");
                lnCreditNo.DataType = CellValues.Number;
                //lnCreditNo.CellValue = new CellValue(lunchAdjustment.ToString(CultureInfo.InvariantCulture));
                lnCreditNo.CellValue = new CellValue("1");

                var lunchCredit = sheetData.Descendants<Cell>().First(c => c.CellReference == "J21");
                //lunchCredit.CellValue = new CellValue((-1*lunchCreditRate).ToString(CultureInfo.InvariantCulture));
                lunchCredit.CellValue = new CellValue((-1 * lunchAdjustment).ToString(CultureInfo.InvariantCulture));
                lunchCredit.DataType = CellValues.Number;


                //var lnCreditNo = sheetData.Descendants<Cell>().First(c => c.CellReference == "E21");
                //lnCreditNo.DataType = CellValues.Number;
                //lnCreditNo.CellValue = new CellValue(lunchAdjustment.ToString(CultureInfo.InvariantCulture));

                //var lunchCredit = sheetData.Descendants<Cell>().First(c => c.CellReference == "J21");
                //lunchCredit.CellValue = new CellValue((-1*lunchCreditRate).ToString(CultureInfo.InvariantCulture));
                //lunchCredit.DataType = CellValues.Number;

            }

            if (snackAdjustment > 0)
            {

                var snCreditNo = sheetData.Descendants<Cell>().First(c => c.CellReference == "E22");
                snCreditNo.DataType = CellValues.Number;
                snCreditNo.CellValue = new CellValue("1");
                //snCreditNo.CellValue = new CellValue(snackAdjustment.ToString(CultureInfo.InvariantCulture));

                var snackCredit = sheetData.Descendants<Cell>().First(c => c.CellReference == "J22");
                //snackCredit.CellValue = new CellValue((-1*snackCreditRate).ToString(CultureInfo.InvariantCulture));
                snackCredit.CellValue = new CellValue((-1 * snackAdjustment).ToString(CultureInfo.InvariantCulture));
                snackCredit.DataType = CellValues.Number;


                //var snCreditNo = sheetData.Descendants<Cell>().First(c => c.CellReference == "E22");
                //snCreditNo.DataType = CellValues.Number;
                //snCreditNo.CellValue = new CellValue(snackAdjustment.ToString(CultureInfo.InvariantCulture));

                //var snackCredit = sheetData.Descendants<Cell>().First(c => c.CellReference == "J22");
                //snackCredit.CellValue = new CellValue((-1*snackCreditRate).ToString(CultureInfo.InvariantCulture));
                //snackCredit.DataType = CellValues.Number;           
            }

            if (supperAdustment > 0)
            {
                var spCreditNo = sheetData.Descendants<Cell>().First(c => c.CellReference == "E23");
                spCreditNo.DataType = CellValues.Number;
                //spCreditNo.CellValue = new CellValue(supperAdustment.ToString(CultureInfo.InvariantCulture));
                spCreditNo.CellValue = new CellValue("1");

                var supperCredit = sheetData.Descendants<Cell>().First(c => c.CellReference == "J23");
                //supperCredit.CellValue = new CellValue((-1*supperCreditRate).ToString(CultureInfo.InvariantCulture));
                supperCredit.CellValue = new CellValue((-1 * supperAdustment).ToString(CultureInfo.InvariantCulture));
                supperCredit.DataType = CellValues.Number;

                //var spCreditNo = sheetData.Descendants<Cell>().First(c => c.CellReference == "E23");
                //spCreditNo.DataType = CellValues.Number;
                //spCreditNo.CellValue = new CellValue(supperAdustment.ToString(CultureInfo.InvariantCulture));

                //var supperCredit = sheetData.Descendants<Cell>().First(c => c.CellReference == "J23");
                //supperCredit.CellValue = new CellValue((-1*supperCreditRate).ToString(CultureInfo.InvariantCulture));
                //supperCredit.DataType = CellValues.Number;

            }

            if (sackLunchAdjustment > 0)
            {
                var slCreditNo = sheetData.Descendants<Cell>().First(c => c.CellReference == "E24");
                slCreditNo.DataType = CellValues.Number;
                //spCreditNo.CellValue = new CellValue(supperAdustment.ToString(CultureInfo.InvariantCulture));
                slCreditNo.CellValue = new CellValue("1");

                var sackLunchCredit = sheetData.Descendants<Cell>().First(c => c.CellReference == "J24");
                //supperCredit.CellValue = new CellValue((-1*supperCreditRate).ToString(CultureInfo.InvariantCulture));
                sackLunchCredit.CellValue = new CellValue((-1 * sackLunchAdjustment).ToString(CultureInfo.InvariantCulture));
                sackLunchCredit.DataType = CellValues.Number;
            }
        }

        public void UpdateDatesAndOrdersInSheet()
        {
            int rowIndex = 33;

            foreach (var orderDate in orderDates)
            {
                var dateCell = sheetData.Descendants<Cell>().First(c => c.CellReference == string.Format("B{0}", rowIndex));
                var bbCell = sheetData.Descendants<Cell>().First(c => c.CellReference == string.Format("C{0}", rowIndex));
                var bbVegCell = sheetData.Descendants<Cell>().First(c => c.CellReference == string.Format("D{0}", rowIndex));
                var bbSpecialCell = sheetData.Descendants<Cell>().First(c => c.CellReference == string.Format("E{0}", rowIndex));
                var lnCell = sheetData.Descendants<Cell>().First(c => c.CellReference == string.Format("F{0}", rowIndex));
                var lnSpecialCell = sheetData.Descendants<Cell>().First(c => c.CellReference == string.Format("G{0}", rowIndex));
                var lnVegCell = sheetData.Descendants<Cell>().First(c => c.CellReference == string.Format("H{0}", rowIndex));
                var snackCell = sheetData.Descendants<Cell>().First(c => c.CellReference == string.Format("I{0}", rowIndex));
                var supperCell = sheetData.Descendants<Cell>().First(c => c.CellReference == string.Format("J{0}", rowIndex));

                dateCell.InlineString = new InlineString { Text = new Text { Text = orderDate.ToShortDateString() } };
                dateCell.DataType = CellValues.InlineString;

                bbCell.DataType = CellValues.Number;
                bbCell.CellValue = new CellValue(breakfastOrderByDate[orderDate].ToString());

                bbSpecialCell.DataType = CellValues.Number;
                bbSpecialCell.CellValue = new CellValue(breakfastSpecialOrderByDate[orderDate].ToString());

                bbVegCell.DataType = CellValues.Number;
                bbVegCell.CellValue = new CellValue(breakfastVeggyOrderByDate[orderDate].ToString());

                lnCell.DataType = CellValues.Number;
                lnCell.CellValue = new CellValue(lunchOrderByDate[orderDate].ToString());

                lnSpecialCell.DataType = CellValues.Number;
                lnSpecialCell.CellValue = new CellValue(lunchSpecialOrderByDate[orderDate].ToString());

                lnVegCell.DataType = CellValues.Number;
                lnVegCell.CellValue = new CellValue(lunchVeggyOrderByDate[orderDate].ToString());

                snackCell.DataType = CellValues.Number;
                snackCell.CellValue = new CellValue(snackOrderByDate[orderDate].ToString());

                supperCell.DataType = CellValues.Number;
                supperCell.CellValue = new CellValue(supperOrderByDate[orderDate].ToString());

                rowIndex++;
            }
        }

        private void UpdatesNotesInSheet()
        {
            //  model.orderReports[0].
            var cellNotes = sheetData.Descendants<Cell>().First(c => c.CellReference == "B58");
            StringBuilder sb = new StringBuilder();
            foreach (var note in model.Notes)
            {
                sb.AppendFormat("{0} : {1} \n", note.Key.ToString(), note.Value);
            }
            cellNotes.InlineString = new InlineString { Text = new Text { Text = sb.ToString() } };
            cellNotes.DataType = CellValues.InlineString;
        }


        public string GenerateInvoiceSheet()
        {
            UpdateInvoiceNumberInSheet();
            UpdateSchoolDataInSheet();
            UpdateQuantitiesInSheet();
            UpdateDueDateInSheet();
            UpdateCreditsInSheet();
            UpdateRatesInSheet();
            UpdateDatesAndOrdersInSheet();
            UpdatesNotesInSheet();


            return FilePath;
        }



        public void Dispose()
        {
            if (sheetData != null)
            {
                try
                {
                    CloseWorkSheet();
                    sheetData = null;
                }
                finally { }
            }
        }

        public void CloseWorkSheet()
        {
            InvoiceSheet.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
            InvoiceSheet.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;
            InvoiceSheet.WorkbookPart.Workbook.Save();

            InvoiceSheet.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
            InvoiceSheet.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;
            InvoiceSheet.Close();
        }
    }
}
