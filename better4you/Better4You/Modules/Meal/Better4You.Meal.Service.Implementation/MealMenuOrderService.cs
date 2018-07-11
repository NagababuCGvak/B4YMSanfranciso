using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading;
using Better4You.Core;
using Better4You.Meal.Business;
using Better4You.Meal.Config;
using Better4You.Meal.Service.Messages;
using Better4You.Meal.ViewModel;
using Better4You.UserManagment.Business;
using Tar.Core.Configuration;
using Tar.Core.Mail;
using Tar.Security;
using Tar.Service;
using Tar.Service.Messages;
//using Better4You.Core;

namespace Better4You.Meal.Service.Implementation
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]    
    public class MealMenuOrderService:Service<IMealMenuOrderService, MealMenuOrderService>, IMealMenuOrderService
    {
        public IMealMenuOrderFacade MealMenuOrderFacade { get; set; }
        public ISchoolFacade SchoolFacade { get; set; }
        public IUserFacade UserFacade { get; set; }
        public ISettings Settings { get; set; }
        public IMailService MailService { get; set; }


        public MealMenuOrderGetAllResponse GetAllByFilter(MealMenuOrderGetAllRequest request)
        {
            return Execute<MealMenuOrderGetAllRequest, MealMenuOrderGetAllResponse>(
                request,
                response =>response.Order= MealMenuOrderFacade.GetByFilter(request.Filter));
        }

        public SchoolOrderGetResponse GetSchoolOrder(SchoolOrderGetRequest request)
        {
            return Execute<SchoolOrderGetRequest, SchoolOrderGetResponse>(
                request,
                response => response.Order = MealMenuOrderFacade.GetSchoolOrder(request.Filter));
        }

        public MealMenuOrderGetResponse Get(MealMenuOrderGetRequest request)
        {
            return Execute<MealMenuOrderGetRequest, MealMenuOrderGetResponse>(
                request,
                response => response.Order = MealMenuOrderFacade.Get(request.Id));
        }
        public MealMenuOrderGetResponse GetMealMenuOrderById(MealMenuOrderGetRequest request)
        {
            return Execute<MealMenuOrderGetRequest, MealMenuOrderGetResponse>(
                request,
                response => response.Order = MealMenuOrderFacade.GetMealMenuOrderById(request.Id));
        }

        public MealMenuOrderSubmitOrderResponse SubmitOrder(MealMenuOrderSubmitOrderRequest request)
        {
            return Execute<MealMenuOrderSubmitOrderRequest, MealMenuOrderSubmitOrderResponse>(
                request,
                response =>  response.IsOrderSubmitted= MealMenuOrderFacade.SubmitOrder(request.Filter,request.UserId)
                );
        }

        public MealMenuOrderItemGetAllResponse GetOrderItemByFilter(MealMenuOrderItemGetAllRequest request)
        {
            return Execute<MealMenuOrderItemGetAllRequest, MealMenuOrderItemGetAllResponse>(
                request,
                response => response.OrderItem = MealMenuOrderFacade.GetOrderItemByFilter(request.Filter));
        }

        public MealMenuOrderMenuItemsResponse GetOrderMenuItems(MealMenuOrderMenuItemsRequest request)
        {
            return Execute<MealMenuOrderMenuItemsRequest, MealMenuOrderMenuItemsResponse>(
                request,
                response => response.Menus = MealMenuOrderFacade.GetOrderMenuItems(request.Filter));
        }

        public MealMenuOrderItemSaveResponse SaveOrderItem(MealMenuOrderItemSaveRequest request)
        {
            return Execute<MealMenuOrderItemSaveRequest, MealMenuOrderItemSaveResponse>(
                request,
                response =>
                    {
                        var annualAgreements = SchoolFacade.AnnualAgreementsByFilter(request.SchoolId, DateTime.Now.Year, request.MealTypeId);
                        response.OrderItem = MealMenuOrderFacade.SaveOrderItem(request.OrderItem, request.SchoolId, annualAgreements.FirstOrDefault().Value);
                        
                        //if (request.OrderItem.Id > 0 && request.SchoolId>0)
                        if (request.OrderItem.Id>0 && request.SchoolId > 0 && (response.OrderItem.RefId??0)>0)
                        {
                            int totalCount;
                            var invoice = MealMenuOrderFacade
                                .GetInvoicesByFilter(new InvoiceFilterView
                                                     {
                                                         OrderDate = new DateTime(response.OrderItem.MealMenuValidDate.Year, response.OrderItem.MealMenuValidDate.Month,1),
                                                         SchoolId = request.SchoolId,
                                                         RecordCount = 1
                                                     }, 1, 1, "", false, out totalCount).FirstOrDefault();
                            if (invoice == null || invoice.OrderStatus.Id == (long)OrderStatuses.InitialState) return;
                            var orderItem = response.OrderItem;
                            var mailAddress = Settings.GetSetting<string>("EmailContactRegardsTo");
                            var school = SchoolFacade.Get(request.SchoolId);
                           // var emails = school.Users.Select(k => k.UserName).ToList();
                            var emails = new List<string>();
                               // UserFacade.GetActiveSchoolUsers(request.SchoolId).Select(c => c.UserName).ToList();
                            emails.Insert(0, mailAddress);
                            var toEmails = string.Join(",", emails);
                            var changedList = MealMenuOrderFacade.GetOrderItemHistory(response.OrderItem.RefId ?? 0);
                            var body = new StringBuilder();
                            body.AppendLine(
                                string.Format("<br/>{0} menu updates occured in {1} - {2} school {3} date order",
                                    orderItem.MenuName, school.Code, school.Name, orderItem.MealMenuValidDate));
                            changedList.ForEach(
                                d => body.AppendLine(string.Format("<br/><br/>Date : {0}", d.ModifiedAt))
                                    .AppendLine(string.Format("<br/>Count : {0}", d.TotalCount))
                                    .AppendLine(string.Format("<br/>Modified By : {0}", d.ModifiedByFullName))
                                    .AppendLine(string.Format("<br/>Reason : {0}", d.ModifiedReason))
                                    .AppendLine().AppendLine());

                            var subject = string.Format("{0} - {1} school {2} order {3} Menu Updates", school.Code,
                                school.Name, orderItem.MealMenuValidDate, orderItem.MenuName);

                            MailService.SendMail(toEmails, subject, body.ToString());
                        }
                    });
        }

        public VoidResponse DeleteOrderItem(MealMenuOrderItemSaveRequest request)
        {

            return Execute<MealMenuOrderItemSaveRequest, VoidResponse>(
                request,
                response => MealMenuOrderFacade.DeleteOrderItem(request.OrderItem));

        }

        public MealMenuOrderReportResponse GetOrderReport(MealMenuOrderReportRequest request)
        {
            return Execute<MealMenuOrderReportRequest, MealMenuOrderReportResponse>(
                request,
                response =>
                    {
                        int totalCount;
                        response.Orders=MealMenuOrderFacade.GetOrderReport(request.Filter, request.PageSize,
                                                                                    request.PageIndex,
                                                                                    request.OrderByField,
                                                                                    request.OrderByAsc, out totalCount);
                        response.TotalCount = totalCount;
                         
                    });
        }

        public InvoiceGetAllResponse SchoolInvoicesGetAllByFilter(InvoiceGetAllRequest request)
        {
            return Execute<InvoiceGetAllRequest, InvoiceGetAllResponse>(
                request,
                response =>
                {
                    int totalCount;
                    response.SchoolInvoices = MealMenuOrderFacade.GetSchoolInvoiceListByFilter(request.Filter, request.PageSize, request.PageIndex,
                                                            request.OrderByField, request.OrderByAsc, out totalCount);
                    response.TotalCount = totalCount;
                });
        }

        public InvoiceGetAllResponse InvoicesGetAllByFilter(InvoiceGetAllRequest request)
        {
            return Execute<InvoiceGetAllRequest, InvoiceGetAllResponse>(
                request,
                response =>
                {
                    int totalCount;
                    response.Invoices = MealMenuOrderFacade.GetInvoicesByFilter(request.Filter, request.PageSize, request.PageIndex,
                                                            request.OrderByField, request.OrderByAsc, out totalCount);
                    response.TotalCount = totalCount;
                });

        }

        public InvoiceSummaryGetAllResponse InvoicesSummaryGetAllByFilter(InvoiceSummaryGetAllRequest request)
        {
            return Execute<InvoiceSummaryGetAllRequest, InvoiceSummaryGetAllResponse>(
                request,
                response =>
                    {
                        int totalCount;
                        response.Invoices = MealMenuOrderFacade.GetInvoicesSummaryByFilter(request.Filter, request.PageSize,
                                                                                    request.PageIndex,
                                                                                    request.OrderByField,
                                                                                    request.OrderByAsc, out totalCount);
                        response.TotalCount = totalCount;
                    });
        }

        public InvoiceUpdateResponse InvoiceUpdate(InvoiceUpdateRequest request)
        {
            return Execute<InvoiceUpdateRequest, InvoiceUpdateResponse>(
                request,
                response => response.Invoice = MealMenuOrderFacade.InvoiceUpdate(request.Invoice,request.UserId));

        }

        public MealMenuOrderItemGetHistoryResponse GetOrderItemHistory(MealMenuOrderItemGetHistoryRequest request)
        {
            return Execute<MealMenuOrderItemGetHistoryRequest, MealMenuOrderItemGetHistoryResponse>(
                request,
                response => response.OrderItems = MealMenuOrderFacade.GetOrderItemHistory(request.OrderItemId));
        }

        public MealMenuOrderDailyChangesResponse GetDailyChanges(MealMenuOrderDailyChangesRequest request)
        {
            return Execute<MealMenuOrderDailyChangesRequest, MealMenuOrderDailyChangesResponse>(
                request,
                response =>
                    {
                        int totalCount;
                        response.OrderItems = MealMenuOrderFacade.GetDailyChanges(request.Filter,
                                                                                  request.PageSize,
                                                                                  request.PageIndex,
                                                                                  request.OrderByField,
                                                                                  request.OrderByAsc, out totalCount);
                        response.TotalCount = totalCount;
                    }
                );
        }

        public SchoolInvoiceDocumentSaveResponse SaveSchoolInvoiceDocument(SchoolInvoiceDocumentSaveRequest request)
        {
            return Execute<SchoolInvoiceDocumentSaveRequest, SchoolInvoiceDocumentSaveResponse>(
                request,
                response =>
                {
                    MealMenuOrderFacade.SaveSchoolInvoiceDocument(request.SchoolInvoice);
                    response.Message = "The file uploaded succesfully.";
                });
        }

        public SchoolInvoiceDocumentGetByIdResponse GetSchoolInvoiceDocumentById(SchoolInvoiceDocumentGetByIdRequest request)
        {
            return Execute<SchoolInvoiceDocumentGetByIdRequest, SchoolInvoiceDocumentGetByIdResponse>(
                request,
                response =>
                    {

                        response.SchoolInvoiceDocument = MealMenuOrderFacade.GetSchoolInvoiceDocumentById(request.SchoolInvoiceDocumentGuid);
                    }
                );
        }
        
        public SaveOrderForDayResponse SaveOrderForDay(SaveOrderForDayRequest request)
        {
            return Execute<SaveOrderForDayRequest, SaveOrderForDayResponse>(
                request,
                response =>
                {
                    var isUpdated = request.Day.Items.Any(d => d.Id > 0);
                    var currentUser = Thread.CurrentPrincipal.AsCurrentUser();
                    request.Day.Items.ForEach(d =>
                    {
                        d.ModifiedBy = currentUser.Name;
                        d.ModifiedByFullName = currentUser.FullName;
                        d.ModifiedReason = (currentUser.UserTypeId() == (int)UserTypes.Company)?"Modified By Company":String.Empty;
                    });

                    var annualAgreements = SchoolFacade.AnnualAgreementsByFilter(request.Day.SchoolId, DateTime.Now.Year, request.Day.MealTypeId);
                    request.Day.OrderRate = annualAgreements.FirstOrDefault().Value;
                    response.Day = MealMenuOrderFacade.SaveOrderForDay(request.Day);

                    var mailAddress = Settings.GetSetting<string>("EmailContactRegardsTo");
                    var school = SchoolFacade.Get(request.Day.SchoolId);
                    var emails = new List<string>();
                    emails.Insert(0, mailAddress);
                    var toEmails = string.Join(",", emails);
                    var body = new StringBuilder();

                    if (isUpdated && currentUser.UserTypeId() == (int)UserTypes.School)
                    {
                        int totalCount;
                        var invoice = MealMenuOrderFacade
                            .GetInvoicesByFilter(new InvoiceFilterView
                            {
                                OrderDate = new DateTime(response.Day.Date.Year, response.Day.Date.Month, 1),
                                SchoolId = request.Day.SchoolId,
                                RecordCount = 1
                            }, 1, 1, "", false, out totalCount).FirstOrDefault();
                        if (invoice == null || invoice.OrderStatus.Id == (long)OrderStatuses.InitialState) return;


                        body.AppendLine(
                            string.Format("<br/>Updates occured in {0} - {1} school {2} date order",
                                school.Code, school.Name, response.Day.Date));
                        response.Day.Items.ForEach(d =>                        
                            body.AppendLine(string.Format("<br/><br/>Menu : {0}", d.MenuName))
                                .AppendLine(string.Format("<br/><br/>Date : {0}", d.ModifiedAt))
                                .AppendLine(string.Format("<br/>Count : {0}", d.Count))
                                .AppendLine(string.Format("<br/>Modified By : {0}", d.ModifiedByFullName))
                                .AppendLine(string.Format("<br/>Reason : {0}", d.ModifiedReason))
                                .AppendLine().AppendLine()
                        );
                        var subject = string.Format("{0} - {1} school {2} order updates", school.Code,
                            school.Name, request.Day.Date);

                        MailService.SendMail(toEmails, subject, body.ToString());
                    }

                    var fruitCount = request.Day.Items.Where(d=>d.HasAdditionalFruit).Sum(d=>d.Count);
                    var vegetableCount = request.Day.Items.Where(d => d.HasAdditionalVegetable).Sum(d => d.Count);
                    if (fruitCount > (request.Day.FruitCount*1.25) || vegetableCount > (request.Day.VegetableCount*1.25))
                    {
                        body.Length = 0;

                        body.AppendLine(string.Format("<br/>Fruit/Vegetable count exceeded the threshold value in {0} - {1} school {2} date order",
                            school.Code, school.Name, response.Day.Date));
                        response.Day.Items.ForEach(d =>
                            body.AppendLine(string.Format("<br/><br/>Fruit Count: {0}, Treshhold Value: {1}*1.25", fruitCount, request.Day.FruitCount))
                                .AppendLine(string.Format("<br/><br/>Vegetable Count: {0}, Treshhold Value: {1}*1.25", vegetableCount, request.Day.VegetableCount))
                                .AppendLine().AppendLine()
                        );
                        var subject = string.Format("{0} - {1} school {2} order updates", school.Code,
                            school.Name, request.Day.Date);

                        MailService.SendMail(toEmails, subject, body.ToString());

                    }
                });
        }

        public DateRangeOrderItemResponse DateRangeOrderItem(DateRangeOrderItemRequest request)
        {
            return Execute<DateRangeOrderItemRequest, DateRangeOrderItemResponse>(
                request,
                response =>
                {
                    response.OrderItems = MealMenuOrderFacade.GetDateRenageOrderItems(request.Filter);

    }
                );
        }

        public GetSupplementaryListResponse GetSupplementaryList(GetSupplementaryListRequest request)
        {
            return Execute<GetSupplementaryListRequest, GetSupplementaryListResponse>(request, response =>
            {
                response.List = MealMenuOrderFacade.GetSupplementaryList(request.Filter);
            });
        }

        public SaveSupplementaryListResponse SaveSupplementaryList(SaveSupplementaryListRequest request)
        {
            return Execute<SaveSupplementaryListRequest, SaveSupplementaryListResponse>(request, response =>
            {
                MealMenuOrderFacade.SaveSupplementaryList(request.List);
            });

        }
    }
}
