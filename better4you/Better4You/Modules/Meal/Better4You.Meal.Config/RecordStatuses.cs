using System.Runtime.Serialization;

namespace Better4You.Meal.Config
{
    [DataContract]
    public enum RecordStatuses
    {
        [EnumMember]
        None=0,
        [EnumMember]
        Active = 1,
        [EnumMember]
        InActive = 2,
        [EnumMember]
        Deleted = 3
    }
}
