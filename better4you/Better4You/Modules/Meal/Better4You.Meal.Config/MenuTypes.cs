using System.Runtime.Serialization;

namespace Better4You.Meal.Config
{
    [DataContract]
    public enum MenuTypes
    {
        [EnumMember]
        None=0,
        [EnumMember]
        Breakfast1 = 1,
        [EnumMember]
        Vegetarian = 2,
        [EnumMember]
        Special = 3,
        [EnumMember]
        Milk = 4,
        [EnumMember]
        LunchOption1 = 5,
        [EnumMember]
        LunchOption2 = 6,
        [EnumMember]
        Snack1 = 7,
        [EnumMember]
        SackLunch1 =8,
        [EnumMember]
        Supper1 = 9,
        [EnumMember]
        PickupStix = 10,
        [EnumMember]
        Bbq = 11,
        [EnumMember]
        PancakeBk = 12,
        [EnumMember]
        Other = 13,
        [EnumMember]
        SoyMilk = 14,
        [EnumMember]
        ComptonBreakfast=15,
        [EnumMember]
        ComptonLunch=16,
        [EnumMember]
        ComptonSack=17,
        [EnumMember]
        ComptonSupper=18,
        [EnumMember]
        LunchOption3 = 19,
        [EnumMember]
        Breakfast2 = 20,
        [EnumMember]
        Breakfast3 = 21,
        [EnumMember]
        Breakfast4 = 22,
        [EnumMember]
        Breakfast5 = 23,
        [EnumMember]
        LunchOption4 = 24,
        [EnumMember]
        LunchOption5 = 25,
        [EnumMember]
        Snack2 =26,
        [EnumMember]
        SackLunch2 = 27,
        [EnumMember]
        Supper2 = 28,



    }
}
