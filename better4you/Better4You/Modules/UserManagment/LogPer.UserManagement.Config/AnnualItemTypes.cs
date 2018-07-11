using System.ComponentModel;
using System.Runtime.Serialization;

namespace Better4You.UserManagement.Config
{
    [DataContract]
    public enum AnnualItemTypes
    {
        [EnumMember] None=0,
        [EnumMember] Breakfast = 1,
        [EnumMember] Snack = 2,
        [EnumMember] Lunch = 3,
        [EnumMember, Description("Sack Lunch")] SackLunch = 4,
        [EnumMember] Supper = 5,
        [EnumMember] SoyMilk = 14
    }
}
