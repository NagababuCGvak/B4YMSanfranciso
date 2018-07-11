using System.Runtime.Serialization;

namespace Better4You.Meal.Config
{
    [DataContract]
    public enum FoodTypes
    {
        [EnumMember]
        None=0,
        [EnumMember]
        Milk = 1,
        [EnumMember]
        Fruit = 2,
        [EnumMember]
        Grain= 3,
        [EnumMember]
        MeatAlternative= 4
    }
}
