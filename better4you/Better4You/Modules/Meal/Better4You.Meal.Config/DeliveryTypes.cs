using System.Runtime.Serialization;

namespace Better4You.Meal.Config
{
    [DataContract]
    public enum DeliveryTypes
    {
        [EnumMember]
        None=0,
        [EnumMember]
        Breakfast=1,
        [EnumMember]
        Lunch=2
    }
}
