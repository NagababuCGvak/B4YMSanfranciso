using Better4You.Meal.Config;
using Better4You.Meal.Service.Messages;
using Better4You.UserManagment.ViewModel;
using System.Collections.Generic;

namespace Better4You.UI.Mvc.Models
{
    public class InvoiceExportViewModel
    {
        public List<MealMenuOrderReportResponse> orderReports = new List<MealMenuOrderReportResponse>();
        //public List<SchoolAnnualAgreementView> SchoolAnnualAgreements = new List<SchoolAnnualAgreementView>();
        public SchoolView School = new SchoolView();
        public Dictionary<MealTypes, string> Notes = new Dictionary<MealTypes, string>();

    }
}
