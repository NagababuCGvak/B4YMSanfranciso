using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class OrderReportView : IView
    {
        [DataMember]
        public long SchoolId { get; set; }

        [DataMember]
        public string SchoolName { get; set; }

        [DataMember]
        public string SchoolCode { get; set; }

        [DataMember]
        public string SchoolType { get; set; }

        [DataMember]
        public long SchoolTypeId { get; set; }

        [DataMember]
        public string FoodServiceType { get; set; }

        [DataMember]
        public string OVSType { get; set; }

        [DataMember]
        public int SchoolRoute { get; set; }

        [DataMember]
        public double? TotalCredit { get; set; }

        [DataMember]
        public double? Rate { get; set; }

        [DataMember]
        public long OrderId { get; set; }

        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        public long MealTypeId { get; set; }

        [DataMember]
        public IList<OrderReportItemView> Items { get; set; }
        /*
        public long RouteInt
        {
            get { return Int64.Parse(SchoolRoute); }
        }
        */
        public OrderReportView()
        {
            Items = new List<OrderReportItemView>();
        }
    }
}
