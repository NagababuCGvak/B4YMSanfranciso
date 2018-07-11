using System.ServiceModel;
using Better4You.Meal.Service.Messages;

namespace Better4You.Meal.Service
{
    [ServiceContract]
    public interface IReportService
    {
        [OperationContract]
        ReportResponse MonthlyExport(MonthlyExportRequest request);
        [OperationContract]
        ReportResponse DateRangeOrderItemExport(DateRangeOrderItemRequest request);
        [OperationContract]
        ReportResponse SchoolMenuExport(SchoolMenuExportRequest request);
        [OperationContract]
        ReportResponse MontlyMilkExport(MontlyMilkExportRequest request);
        [OperationContract]
        ReportResponse OrderDayPropItemExport(OrderDayPropItemRequest request);
    }
}
