using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    public class MealOrderManageDayView:IView
    {

        public int Day
        {
            get { return Date.Day; }
        }

        public DateTime Date { get; set; }

        public DateTime OrderDate
        {
            get { return new DateTime(Date.Year, Date.Month, 1); }
        }

        public string DayName
        {
            get { return Date.DayOfWeek.ToString("G"); }
        }
        
        public long DeliveryTypeId { get; set; }

        public List<MealOrderManageDayItemView> Items { get; set; }

        public long SchoolId { get; set; }
        
        public double? OrderRate { get; set; }
        
        public long MealTypeId { get; set; }

        public bool IsEditable { get; set; }


        public int FruitCount { get; set; }

        public int VegetableCount { get; set; }
    }
}