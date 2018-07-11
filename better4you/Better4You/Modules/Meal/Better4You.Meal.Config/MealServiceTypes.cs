using System.Runtime.Serialization;

namespace Better4You.Meal.Config
{
    [DataContract]
    public enum MealServiceTypes
    {
        [EnumMember]
        None=0,
        [EnumMember]
        Prepack = 1,
        [EnumMember]
        PrepackNpb = 2,
        [EnumMember]
        PrepackNp = 3,
        [EnumMember]
        PrepackNcs = 4,
        [EnumMember]
        PrepackNnut = 5,
        [EnumMember]
        Family = 6,
        [EnumMember]
        FamilyNp = 7,
        [EnumMember]
        FamilyPreW = 8
    }
}
