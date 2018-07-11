using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Tar.ViewModel;

namespace Better4You.Meal.Config
{
    public static class Lookups
    {
        private static readonly Dictionary<Type, List<GeneralItemView>> Source = new Dictionary<Type, List<GeneralItemView>>();

        public static GeneralItemView GetItem<T>(T val)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            AddIfNotExist<T>();
            return Source[typeof(T)].FirstOrDefault(x => x.Id == (long)(object)val);
        }
        public static GeneralItemView GetItem<T>(long val)
            where T : struct, IComparable, IFormattable, IConvertible
        {
            AddIfNotExist<T>();
            return Source[typeof(T)].FirstOrDefault(x => x.Id == (long)(object)val);
        }
        public static List<GeneralItemView> GetItems<T>()
            where T : struct, IComparable, IFormattable, IConvertible
        {
            AddIfNotExist<T>();
            return Source[typeof (T)];
        }

        private static void AddIfNotExist<T>() 
            where T:struct , IComparable, IFormattable, IConvertible
        {
            if (Source.ContainsKey(typeof (T))) return;
            
            var members = typeof(T).GetMembers();
            var list = new List<GeneralItemView>();
            foreach (var enumVal in Enum.GetValues(typeof(T)))
            {
                var text = enumVal.ToString();                
                members.First(x => x.Name == text)
                    .GetCustomAttributes(typeof (DescriptionAttribute), true)
                    .Cast<DescriptionAttribute>()
                    .Where(x => !string.IsNullOrEmpty(x.Description))
                    .ToList().ForEach(x => text = x.Description);
                list.Add(new GeneralItemView((int) enumVal, ((int) enumVal).ToString("G"), text));
            }
            Source.Add(typeof (T), list);
        }



        public static List<GeneralItemView> IngredientTypeList
        {
            get
            {
                return new List<GeneralItemView>
                    {
                        new GeneralItemView((int)IngredientTypes.Size,IngredientTypes.Size.ToString("G"),"Size"),
                        new GeneralItemView((int)IngredientTypes.Calories,IngredientTypes.Calories.ToString("G"),"Calories"),
                        new GeneralItemView((int)IngredientTypes.Cholesterol,IngredientTypes.Cholesterol.ToString("G"),"Cholesterol"),
                        new GeneralItemView((int)IngredientTypes.Fiber,IngredientTypes.Fiber.ToString("G"),"Fiber"),
                        new GeneralItemView((int)IngredientTypes.Iron,IngredientTypes.Iron.ToString("G"),"Iron"),
                        new GeneralItemView((int)IngredientTypes.Calcium,IngredientTypes.Calcium.ToString("G"),"Calcium"),
                        new GeneralItemView((int)IngredientTypes.VitaminA,IngredientTypes.VitaminA.ToString("G"),"Vitamin A"),
                        new GeneralItemView((int)IngredientTypes.VitaminB,IngredientTypes.VitaminB.ToString("G"),"Vitamin B"),
                        new GeneralItemView((int)IngredientTypes.VitaminC,IngredientTypes.VitaminC.ToString("G"),"Vitamin C"),
                        new GeneralItemView((int)IngredientTypes.Protein,IngredientTypes.Protein.ToString("G"),"Protein"),
                        new GeneralItemView((int)IngredientTypes.Carbs,IngredientTypes.Carbs.ToString("G"),"Carbs"),
                        new GeneralItemView((int)IngredientTypes.TotalFat,IngredientTypes.TotalFat.ToString("G"),"Total Fat"),
                        new GeneralItemView((int)IngredientTypes.SatFat,IngredientTypes.SatFat.ToString("G"),"Sat Fat"),
                        new GeneralItemView((int)IngredientTypes.TransFat,IngredientTypes.TransFat.ToString("G"),"Trans Fat")
                    };
            }    
        } 

        public static List<GeneralItemView> MealServiceTypeList
        {
            get
            {
                return new List<GeneralItemView>
                       {

                           new GeneralItemView((int)MealServiceTypes.Prepack,MealServiceTypes.Prepack.ToString("G"),"PREPACK"),
                           new GeneralItemView((int)MealServiceTypes.PrepackNpb,MealServiceTypes.PrepackNpb.ToString("G"),"PREPACK/NPB"),
                           new GeneralItemView((int)MealServiceTypes.PrepackNp,MealServiceTypes.PrepackNp.ToString("G"),"PREPACK/NP"),
                           new GeneralItemView((int)MealServiceTypes.PrepackNcs,MealServiceTypes.PrepackNcs.ToString("G"),"PREPACK/NCS"),
                           new GeneralItemView((int)MealServiceTypes.PrepackNnut,MealServiceTypes.PrepackNnut.ToString("G"),"PREPACK/NNUT"),
                           new GeneralItemView((int)MealServiceTypes.Family,MealServiceTypes.Family.ToString("G"),"FAMILY"),
                           new GeneralItemView((int)MealServiceTypes.FamilyNp,MealServiceTypes.FamilyNp.ToString("G"),"FAMILY/NP"),
                           new GeneralItemView((int)MealServiceTypes.FamilyPreW,MealServiceTypes.FamilyPreW.ToString("G"),"FAM / PRE W")
                       };
            }
        } 
        public static List<GeneralItemView> OrderStatusList
        {
            get
            {
                return new List<GeneralItemView>
                       {
                           new GeneralItemView((int)OrderStatuses.InitialState,OrderStatuses.InitialState.ToString("G"),"Initial"),
                           new GeneralItemView((int)OrderStatuses.InvoiceSent,OrderStatuses.InvoiceSent.ToString("G"),"Invoice Sent"),
                           new GeneralItemView((int)OrderStatuses.Pending,OrderStatuses.Pending.ToString("G"),"Pending"),
                           new GeneralItemView((int)OrderStatuses.Paid,OrderStatuses.Paid.ToString("G"),"Paid"),
                           new GeneralItemView((int)OrderStatuses.Submitted,OrderStatuses.Submitted.ToString("G"),"Order Submitted"),
                       };

            }
        } 
        public static List<GeneralItemView> DeliveryTypeList
        {
            get
            {
                return new List<GeneralItemView>
                       {
                           new GeneralItemView((int)DeliveryTypes.Breakfast,DeliveryTypes.Breakfast.ToString("G"),"Breakfast"),
                           new GeneralItemView((int)DeliveryTypes.Lunch,DeliveryTypes.Lunch.ToString("G"),"Lunch")
                       };
            }
        } 
        public static List<GeneralItemView> FoodTypeList
        {
            get
            {
                return new List<GeneralItemView>
                       {
                           new GeneralItemView((int)FoodTypes.Milk,FoodTypes.Milk.ToString("G"),"Milk"),
                           new GeneralItemView((int)FoodTypes.Fruit,FoodTypes.Fruit.ToString("G"),"Fruit"),
                           new GeneralItemView((int)FoodTypes.Grain,FoodTypes.Grain.ToString("G"),"Grain"),
                           new GeneralItemView((int)FoodTypes.MeatAlternative,FoodTypes.MeatAlternative.ToString("G"),"Meat Alternative")
                       };
            }
        } 

        public static List<GeneralItemView> MenuTypeList
        {
            get
            {
                return new List<GeneralItemView>
                       {
                           new GeneralItemView((int)MenuTypes.Breakfast1,MenuTypes.Breakfast1.ToString("G"),"Breakfast Option 1"),
                           new GeneralItemView((int)MenuTypes.Breakfast2,MenuTypes.Breakfast2.ToString("G"),"Breakfast Option 2"),
                           new GeneralItemView((int)MenuTypes.Breakfast3,MenuTypes.Breakfast3.ToString("G"),"Breakfast Option 3"),
                           new GeneralItemView((int)MenuTypes.Breakfast4,MenuTypes.Breakfast4.ToString("G"),"Breakfast Option 4"),
                           new GeneralItemView((int)MenuTypes.Breakfast5,MenuTypes.Breakfast5.ToString("G"),"Breakfast Option 5"),
                           new GeneralItemView((int)MenuTypes.ComptonBreakfast,MenuTypes.ComptonBreakfast.ToString("G"),"Compton Breakfast"),
                           new GeneralItemView((int)MenuTypes.Special,MenuTypes.Special.ToString("G"),"Special"),
                           new GeneralItemView((int)MenuTypes.Vegetarian,MenuTypes.Vegetarian.ToString("G"),"Vegetarian"),
                           new GeneralItemView((int)MenuTypes.Milk,MenuTypes.Milk.ToString("G"),"Milk"),
                           new GeneralItemView((int)MenuTypes.SackLunch1,MenuTypes.SackLunch1.ToString("G"),"Sack Lunch Option 1"),
                           new GeneralItemView((int)MenuTypes.SackLunch1,MenuTypes.SackLunch1.ToString("G"),"Sack Lunch Option 2"),
                           new GeneralItemView((int)MenuTypes.LunchOption1,MenuTypes.LunchOption1.ToString("G"),"Lunch Option 1"),
                           new GeneralItemView((int)MenuTypes.LunchOption2,MenuTypes.LunchOption2.ToString("G"),"Lunch Option 2"),
                           new GeneralItemView((int)MenuTypes.LunchOption3,MenuTypes.LunchOption3.ToString("G"),"Lunch Option 3"),
                           new GeneralItemView((int)MenuTypes.LunchOption4,MenuTypes.LunchOption4.ToString("G"),"Lunch Option 4"),
                           new GeneralItemView((int)MenuTypes.LunchOption5,MenuTypes.LunchOption5.ToString("G"),"Lunch Option 5"),
                           new GeneralItemView((int)MenuTypes.LunchOption3,MenuTypes.LunchOption3.ToString("G"),"Salad Lunch"),
                           new GeneralItemView((int)MenuTypes.ComptonLunch,MenuTypes.ComptonLunch.ToString("G"),"Compton Lunch"),
                           new GeneralItemView((int)MenuTypes.Snack1,MenuTypes.Snack1.ToString("G"),"Snack Option 1"),
                           new GeneralItemView((int)MenuTypes.Snack2,MenuTypes.Snack2.ToString("G"),"Snack Option 2"),
                           new GeneralItemView((int)MenuTypes.ComptonSack,MenuTypes.SackLunch1.ToString("G"),"Compton Sack"),
                           new GeneralItemView((int)MenuTypes.Supper1,MenuTypes.Supper1.ToString("G"),"Supper Option 1"),
                           new GeneralItemView((int)MenuTypes.Supper2,MenuTypes.Supper2.ToString("G"),"Supper Option 2"),
                           new GeneralItemView((int)MenuTypes.ComptonSupper,MenuTypes.ComptonSupper.ToString("G"),"Compton Supper"),
                           new GeneralItemView((int)MenuTypes.PickupStix,MenuTypes.PickupStix.ToString("G"),"Pickup Stix"),
                           new GeneralItemView((int)MenuTypes.Bbq,MenuTypes.Bbq.ToString("G"),"Bbq"),
                           new GeneralItemView((int)MenuTypes.PancakeBk,MenuTypes.PancakeBk.ToString("G"),"Pancake Bk"),
                           new GeneralItemView((int)MenuTypes.Other,MenuTypes.Other.ToString("G"),"Pizza"),
                           new GeneralItemView((int)MenuTypes.SoyMilk,MenuTypes.SoyMilk.ToString("G"),"Soy Milk")
                       };
            }
        } 

        public static List<GeneralItemView> MealTypeList
        {
            get
            {
                return new List<GeneralItemView>
                       {
                           new GeneralItemView((int) MealTypes.Breakfast,MealTypes.Breakfast.ToString("G"),"Breakfast") ,
                           new GeneralItemView((int) MealTypes.Snack,MealTypes.Snack.ToString("G"),"Snack") ,
                           new GeneralItemView((int) MealTypes.Lunch,MealTypes.Lunch.ToString("G"),"Lunch") ,
                           new GeneralItemView((int) MealTypes.SackLunch,MealTypes.SackLunch.ToString("G"),"Sack Lunch") ,
                           new GeneralItemView((int) MealTypes.Supper,MealTypes.Supper.ToString("G"),"Supper") 
                       };
            }
        }
        public static List<GeneralItemView> MealTypeShortList
        {
            get
            {
                return new List<GeneralItemView>
                       {
                           new GeneralItemView((int) MealTypes.Breakfast,MealTypes.Breakfast.ToString("G"),"Brkf") ,
                           new GeneralItemView((int) MealTypes.Snack,MealTypes.Snack.ToString("G"),"Snck") ,
                           new GeneralItemView((int) MealTypes.Lunch,MealTypes.Lunch.ToString("G"),"Lnch") ,
                           new GeneralItemView((int) MealTypes.SackLunch,MealTypes.SackLunch.ToString("G"),"SaLu") ,
                           new GeneralItemView((int) MealTypes.Supper,MealTypes.Supper.ToString("G"),"Sppr") 
                       };
            }
        }
        public static List<GeneralItemView> RecordStatusList
        {
            get
            {
                return new List<GeneralItemView>
                       {
                           new GeneralItemView((int) RecordStatuses.None,RecordStatuses.None.ToString("G")," ") ,
                           new GeneralItemView((int) RecordStatuses.Active,RecordStatuses.Active.ToString("G"),"Active") ,
                           new GeneralItemView((int) RecordStatuses.Deleted,RecordStatuses.Deleted.ToString("G"),"Deleted") ,
                           new GeneralItemView((int) RecordStatuses.InActive,RecordStatuses.InActive.ToString("G"),"Passive") 
                       };
            }
        }
        public static List<GeneralItemView> MenuSchoolTypeList
        {
            get
            {
                return new List<GeneralItemView>
                       {
                           new GeneralItemView((int) MenuSchoolTypes.None,MenuSchoolTypes.None.ToString("G"),"Default") ,
                           new GeneralItemView((int) MenuSchoolTypes.Hs,MenuSchoolTypes.Hs.ToString("G"),"High Scholl") ,
                           new GeneralItemView((int) MenuSchoolTypes.K8,MenuSchoolTypes.K8.ToString("G"),"K8") 
                       };
            }
        } 
        public static Dictionary<MealTypes, List<MenuTypes>> MealMenuTypeList
        {
            get
            {
                return new Dictionary<MealTypes, List<MenuTypes>>
                       {
                        {
                            MealTypes.Breakfast,
                            new List<MenuTypes> 
                            {
                                MenuTypes.Breakfast1,
                                MenuTypes.Breakfast2,
                                MenuTypes.Breakfast3,
                                MenuTypes.Breakfast4,
                                MenuTypes.Breakfast5,
                                MenuTypes.PancakeBk,
                                MenuTypes.Vegetarian,
                                MenuTypes.Special,
                                MenuTypes.Milk,
                                MenuTypes.SoyMilk,
                                MenuTypes.ComptonBreakfast
                            }
                        },
                        {
                            MealTypes.Lunch   ,
                            new List<MenuTypes>
                            {
                                MenuTypes.LunchOption1,
                                MenuTypes.LunchOption2,
                                MenuTypes.LunchOption3,
                                MenuTypes.LunchOption4,
                                MenuTypes.LunchOption5,
                                MenuTypes.PickupStix,
                                MenuTypes.Bbq,
                                MenuTypes.Other,
                                MenuTypes.Vegetarian,
                                MenuTypes.Special,
                                MenuTypes.Milk,
                                MenuTypes.SoyMilk,
                                MenuTypes.ComptonLunch
                            }
                        },
                        {
                            MealTypes.Snack,
                            new List<MenuTypes>
                            {
                                MenuTypes.Snack1,
                                MenuTypes.Snack2,
                                MenuTypes.Vegetarian,
                                MenuTypes.Special,
                                MenuTypes.Milk,
                                MenuTypes.SoyMilk                             
                            }
                        },
                        {
                            MealTypes.SackLunch,
                            new List<MenuTypes>
                            {
                                MenuTypes.SackLunch1,
                                MenuTypes.SackLunch2,
                                MenuTypes.Vegetarian,
                                MenuTypes.Special,
                                MenuTypes.Milk,
                                MenuTypes.SoyMilk,
                                 MenuTypes.ComptonSack
                            }                            
                        },
                        {
                            MealTypes.Supper,
                            new List<MenuTypes>
                            {
                                MenuTypes.Supper1,
                                MenuTypes.Supper2,
                                MenuTypes.Vegetarian,
                                MenuTypes.Special,
                                MenuTypes.Milk,
                                MenuTypes.SoyMilk,
                                MenuTypes.ComptonSupper
                            }                                                        
                        }
                       };
            }
        }
    }
}
