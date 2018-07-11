using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Tar.ViewModel;

namespace Better4You.Meal.ViewModel
{
    [DataContract]
    public class OrderReportItemView : IView

    {
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public IList<OrderReportMenuView> Menus { get; set; }

        public long DeliveryType { get; set; }

        public OrderReportItemView()
        {
            Menus = new List<OrderReportMenuView>();
        }
    }
}