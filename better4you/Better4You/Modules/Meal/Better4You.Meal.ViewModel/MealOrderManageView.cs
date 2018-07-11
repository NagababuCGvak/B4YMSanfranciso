using System;
using System.Collections.Generic;
using System.Linq;
using Better4You.Meal.Config;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    public class MealOrderManageView
    {
        private const string DateFormatForTitle = "yyyy MMMM";

        public long OrderId { get; set; }
        public DateTime NextMonth
        {
            get { return StartDate.AddMonths(1); }
        }

        public DateTime PreviousMonth
        {
            get { return StartDate.AddMonths(-1); }
        }
        public string PreviousMonthTitle
        {
            get { return PreviousMonth.ToString(DateFormatForTitle); }
        }
        public string NextMonthTitle
        {
            get { return NextMonth.ToString(DateFormatForTitle); }
        }

        public IEnumerable<GeneralItemView> MealTypes
        {
            get { return Lookups.GetItems<MealTypes>().Where(x => x.Id != 0); }
        }
        public IEnumerable<GeneralItemView> MealServiceTypes
        {
            get { return Lookups.GetItems<MealServiceTypes>().Where(x => x.Id != 0); }
        }

        public IEnumerable<GeneralItemView> DeliveryTypes
        {
            get { return Lookups.GetItems<DeliveryTypes>().Where(x => x.Id != 0); }
        }

        public int Year { get; set; }
        public int Month { get; set; }

        public string Title
        {
            get { return StartDate.ToString(DateFormatForTitle); }
        }

        public DateTime StartDate
        {
            get { return new DateTime(Year, Month, 1); }
        }

        public DateTime EndDate
        {
            get { return new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month)); }
        }

        public long MealTypeId { get; set; }
        public List<MealOrderManageDayView> Days { get; set; }
        public List<MealMenuSupplementaryView> SupplementaryList { get; set; }
        
        public long SchoolId { get; set; }
        public long SchoolType { get; set; }

        public string SchoolName { get; set; }
        public bool FromOutSide { get; set; }
        public bool OrderIsSubmitted { get; set; }

        public FoodPercentageView FoodPercentage { get; set; }

        public MealOrderManageView()
        {
            Year = DateTime.Today.Year;
            Month = DateTime.Today.Month;
        }
    }
}
