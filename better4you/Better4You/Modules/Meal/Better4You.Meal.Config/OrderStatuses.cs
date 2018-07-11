using System.Runtime.Serialization;

namespace Better4You.Meal.Config
{
    [DataContract]
    public enum OrderStatuses
    {
        [EnumMember]
        None=0,
        [EnumMember]
        InitialState = 1,
        [EnumMember]
        InvoiceSent = 2,
        [EnumMember]
        Pending = 3,
        [EnumMember]
        Paid = 4,
        [EnumMember]
        Submitted = 5,
    }
}
