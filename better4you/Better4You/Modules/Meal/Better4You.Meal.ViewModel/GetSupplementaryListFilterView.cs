using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class GetSupplementaryListFilterView:IView
    {
        [DataMember]
        public long MealTypeId { get; set; }

        [DataMember]
        public long SchoolId { get; set; }
    }
}