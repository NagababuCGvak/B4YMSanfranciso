using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.Meal.ViewModel;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class DateRangeOrderItemResponse:Response
    {
        [DataMember]
        public List<DateRangeReportOrderItemView> OrderItems { get; set; }

        public DateRangeOrderItemResponse()
        {
            OrderItems=new List<DateRangeReportOrderItemView>();
        }
    }
}