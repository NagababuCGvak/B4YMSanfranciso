using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class MealMenuSupplementaryView : IView
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long SchoolId { get; set; }
        [DataMember]
        public long MenuId { get; set; }
        [DataMember]
        public long MealType { get; set; }
        [DataMember]
        public int Monday { get; set; }
        [DataMember]
        public int Tuesday { get; set; }
        [DataMember]
        public int Wednesday { get; set; }
        [DataMember]
        public int Thursday { get; set; }
        [DataMember]
        public int Friday { get; set; }
    }
}