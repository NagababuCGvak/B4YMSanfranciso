using System;
using System.Collections.Generic;
using System.Linq;
using Better4You.Core;
using Better4You.Meal.Config;
using Better4You.Meal.ViewModel;
using Tar.Core.Extensions;

namespace Better4You.UI.Mvc.Models
{
    public class FruitVegPercentagePageModel : PageModel1
    {
        public FruitVegPercentagePageModel()
            : base("Fruit Vegetable Percentages")
        {
        }

        public override List<BreadcrumbsModel> Breadcrumbs
        {
            get
            {
                return new List<BreadcrumbsModel>
                {
                    new BreadcrumbsModel {Title = "Home", IconClass = "clip-home-3", Url = "~/"},
                    new BreadcrumbsModel {Title = "Fruit Vegetable Percentages", IconClass = ""}
                };
            }
        }

        public long MealType { get; set; }
        public long SchoolId { get; set; }
        public List<Tar.ViewModel.GeneralItemView> Schools { get; set; }

        public List<Tar.ViewModel.GeneralItemView> MealTypes
        {
            get
            {
                return Lookups.GetItems<MealTypes>().Where(x => x.Id != Meal.Config.MealTypes.None.ToInt64()).ToList();
            }
        }

        public List<MenuListItemView> Menus { get; set; }

    }
}