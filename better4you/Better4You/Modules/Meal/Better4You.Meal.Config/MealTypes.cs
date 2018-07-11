using System.ComponentModel;
using System.Runtime.Serialization;

namespace Better4You.Meal.Config
{
    [DataContract]
    public enum MealTypes
    {
        [EnumMember]
        None=0,
        [EnumMember]
        Breakfast = 1,
        [EnumMember]
        Snack = 2,
        [EnumMember]
        Lunch = 3,
        [EnumMember, Description("Sack Lunch")]
        SackLunch = 4,
        [EnumMember]
        Supper = 5
    }
}
