using System.Runtime.Serialization;

namespace Better4You.Meal.Config
{
    [DataContract]
    public enum IngredientTypes
    {
        [EnumMember]
        None=0,
        [EnumMember]
        Size = 1,
        [EnumMember]
        Calories = 2,
        [EnumMember]
        Cholesterol= 3,
        [EnumMember]
        Fiber= 5,
        [EnumMember]
        Iron = 6,
        [EnumMember]
        Calcium = 7,
        [EnumMember]
        VitaminA= 8,
        [EnumMember]
        VitaminB= 9,
        [EnumMember]
        VitaminC = 10,
        [EnumMember]
        Protein = 11,
        [EnumMember]
        Carbs= 12,
        [EnumMember]
        TotalFat= 13,
        [EnumMember]
        SatFat = 14,
        [EnumMember]
        TransFat = 15
    }
}
