using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class MenuFilterView : IView
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public long MenuTypeId { get; set; }

        [DataMember]
        public long RecordStatusId { get; set; }

        [DataMember]
        public long MealTypeId { get; set; }
    }
}
