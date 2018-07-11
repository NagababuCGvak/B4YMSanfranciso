using System.Collections.Generic;
using Better4You.UserManagment.ViewModel;

namespace Better4You.UserManagment.Business
{
    public interface ISchoolFacade
    {
        List<SchoolListItemView> GetByFilter(SchoolFilterView filter, int pageSize, int pageIndex, string orderByField, bool orderByAsc, out int totalCount);
        List<KeyValuePair<int, double>> AnnualAgreementsByYearAndId(long schoolId, int? year);
        List<SchoolAnnualAgreementView> AllAnnualAgreementsByYear(int year);

        SchoolView Get(long id);

        SchoolView Save(SchoolView view);

        void Delete(long id);

        void CreateUser(long schoolId, long userId);

        void DeleteUser(long schoolId, long userId);

        List<UserItemView> GetUserByFilter(string filter);

        List<KeyValuePair<int, double>> AnnualAgreementsByFilter(long schoolId, int? year, long mealTypeId);
        
        void CreateAnnualAgreement(long schoolId, SchoolAnnualAgreementView view);

        void DeleteAnnualAgreement(long schoolId, SchoolAnnualAgreementView view);

        void UpdateAnnualAgreement(long schoolId, SchoolAnnualAgreementView view);

        SchoolAnnualAgreementView GetSchoolAnnualAgreement(long id);

        void CreateContactInfo(ContactInfoView view);

        void CreateSchoolRoute(long schoolId, SchoolRouteView view);

        void DeleteSchoolRoute(long schoolId, SchoolRouteView view);

        void UpdateSchoolRoute(long schoolId, SchoolRouteView view);

        SchoolRouteView GetSchoolRoute(long id);

    }   
    
}
