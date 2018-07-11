using System.Runtime.Serialization;

namespace Better4You.Meal.Config
{
    [DataContract]
    public enum MenuSchoolTypes
    {
        [EnumMember]
        None = 0,
        
        [EnumMember]
        Hs = 5,

        [EnumMember]
        K8 = 7
    
    }
}
