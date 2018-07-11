using System;
using System.Collections.Generic;
using System.Linq;
using Better4You.Meal.Config;
using Better4You.Meal.ViewModel;
using Tar.ViewModel;

namespace Better4You.UI.Mvc.Models
{
    public class MealDailyViewModel
    {
        public bool Editable
        {
            get { return StartDate > DateTime.Today; }
        }

        public int MealType { set; get; }

        public IEnumerable<GeneralItemView> MealTypes
        {
            get { return Lookups.GetItems<MealTypes>().Where(x => x.Id != 0); }
        }

        public List<MealMenuListItemView> MealMenus { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }
        
        public DateTime StartDate
        {
            get { return new DateTime(Year, Month, Day == 0 ? 1 : Day); }
        }

        public DateTime EndDate
        {
            get { return new DateTime(Year, Month, Day == 0 ? DateTime.DaysInMonth(Year, Month) : Day); }
        }
    }
}