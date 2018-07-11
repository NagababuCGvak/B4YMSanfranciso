using System.ServiceModel;
using Better4You.Meal.Service.Messages;
using Tar.Service.Messages;

namespace Better4You.Meal.Service
{
    [ServiceContract]
    public interface IMealMenuOrderService
    {

                
        [OperationContract]
        MealMenuOrderGetAllResponse GetAllByFilter(MealMenuOrderGetAllRequest request);

        [OperationContract]
        SchoolOrderGetResponse GetSchoolOrder(SchoolOrderGetRequest request);


        [OperationContract]
        MealMenuOrderGetResponse Get(MealMenuOrderGetRequest request);
        
        [OperationContract]
        MealMenuOrderGetResponse GetMealMenuOrderById(MealMenuOrderGetRequest request);

        [OperationContract]
        MealMenuOrderSubmitOrderResponse SubmitOrder(MealMenuOrderSubmitOrderRequest request);

        [OperationContract]
        MealMenuOrderItemGetAllResponse GetOrderItemByFilter(MealMenuOrderItemGetAllRequest request);
        
        [OperationContract]
        MealMenuOrderMenuItemsResponse GetOrderMenuItems(MealMenuOrderMenuItemsRequest request);

        [OperationContract]
        MealMenuOrderItemSaveResponse SaveOrderItem(MealMenuOrderItemSaveRequest request);

        [OperationContract]
        VoidResponse DeleteOrderItem(MealMenuOrderItemSaveRequest request);

        [OperationContract]
        MealMenuOrderReportResponse GetOrderReport(MealMenuOrderReportRequest request);

        [OperationContract]
        InvoiceGetAllResponse SchoolInvoicesGetAllByFilter(InvoiceGetAllRequest request);

        [OperationContract]
        InvoiceGetAllResponse InvoicesGetAllByFilter(InvoiceGetAllRequest request);

        [OperationContract]
        InvoiceSummaryGetAllResponse InvoicesSummaryGetAllByFilter(InvoiceSummaryGetAllRequest request);

        [OperationContract]
        InvoiceUpdateResponse InvoiceUpdate(InvoiceUpdateRequest request);

        [OperationContract]
        MealMenuOrderItemGetHistoryResponse GetOrderItemHistory(MealMenuOrderItemGetHistoryRequest request);

        [OperationContract]
        MealMenuOrderDailyChangesResponse GetDailyChanges(MealMenuOrderDailyChangesRequest request);

        [OperationContract]
        SchoolInvoiceDocumentSaveResponse SaveSchoolInvoiceDocument(SchoolInvoiceDocumentSaveRequest request);

        [OperationContract]
        SchoolInvoiceDocumentGetByIdResponse GetSchoolInvoiceDocumentById(SchoolInvoiceDocumentGetByIdRequest request);

        [OperationContract]
        SaveOrderForDayResponse SaveOrderForDay(SaveOrderForDayRequest request);

        [OperationContract]
        DateRangeOrderItemResponse DateRangeOrderItem(DateRangeOrderItemRequest request);

        [OperationContract]
        GetSupplementaryListResponse GetSupplementaryList(GetSupplementaryListRequest request);
        [OperationContract]
        SaveSupplementaryListResponse SaveSupplementaryList(SaveSupplementaryListRequest request); 
    }
}
