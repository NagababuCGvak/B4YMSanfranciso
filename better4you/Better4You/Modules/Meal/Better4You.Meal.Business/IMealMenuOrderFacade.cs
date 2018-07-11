using System;
using System.Collections.Generic;
using Better4You.Meal.EntityModel;
using Better4You.Meal.ViewModel;

namespace Better4You.Meal.Business
{
    
    public interface IMealMenuOrderFacade
    {
        MealOrderManageView GetSchoolOrder(MealMenuOrderFilterView filter);

        MealMenuOrderView GetByFilter(MealMenuOrderFilterView filter);
        
        List<DailyItemsReportView> GetDailyItemsReport(MealMenuOrderFilterView filter);
        
        MealMenuOrderView Get(long id);
        
        MealMenuOrderView GetMealMenuOrderById(long id);
        
        List<MealMenuOrderMenuView> GetOrderMenuItems(MealMenuOrderItemFilterView filter);
        
        MealMenuOrderItemView GetOrderItemByFilter(MealMenuOrderItemFilterView filter);

        List<MealMenuOrderItemHistoricalView> GetOrderItemHistory(long id);

        bool SubmitOrder(MealMenuOrderFilterView filter, long userId);

        MealMenuOrderItemView SaveOrderItem(MealMenuOrderItemView orderItem, long schoolId, double? orderRate);

        void DeleteOrderItem(MealMenuOrderItemView orderItem);

        List<SchoolInvoiceListItemView> GetSchoolInvoiceListByFilter(InvoiceFilterView filter, int pageSize, int pageIndex, string orderByField, bool orderByAsc, out int totalCount);

        List<InvoiceListItemView> GetInvoicesByFilter(InvoiceFilterView filter, int pageSize, int pageIndex, string orderByField, bool orderByAsc, out int totalCount);

        List<InvoiceSummaryView> GetInvoicesSummaryByFilter(InvoiceFilterView filter, int pageSize, int pageIndex,string orderByField, bool orderByAsc, out int totalCount);

        InvoiceListItemView InvoiceUpdate(InvoiceListItemView view, long userId);

        List<OrderReportView> GetOrderReport(OrderReportFilterView filter, int pageSize, int pageIndex, string orderByField, bool orderByAsc, out int totalCount);

        List<DailyChangesItemView> GetDailyChanges(DailyChangesFilterView filter, int pageSize, int pageIndex, string orderByField, bool orderByAsc, out int totalCount);

        void SaveSchoolInvoiceDocument(SchoolInvoiceDocumentView schoolInvoiceView);
        
        SchoolInvoiceDocumentView GetSchoolInvoiceDocumentById(Guid id);

        List<DateRangeReportOrderItemView> GetDateRenageOrderItems(DateRangeOrderItemFilterView filter);
        
        MealOrderManageDayView SaveOrderForDay(MealOrderManageDayView day);

        List<MealMenuSupplementaryView> GetSupplementaryList(GetSupplementaryListFilterView filter);
        
        void SaveSupplementaryList(List<MealMenuSupplementaryView> list);
    }
}
