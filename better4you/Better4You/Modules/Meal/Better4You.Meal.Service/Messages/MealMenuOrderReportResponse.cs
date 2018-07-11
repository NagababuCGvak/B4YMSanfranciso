using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class MealMenuOrderReportResponse : PageableResponse
    {
        [DataMember]
        public List<OrderReportView> Orders { get; set; }

        public MealMenuOrderReportResponse()
        {
            Orders = new List<OrderReportView>();
        }
        
    }
}