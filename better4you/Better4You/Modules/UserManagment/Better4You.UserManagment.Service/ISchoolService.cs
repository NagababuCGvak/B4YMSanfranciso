using System.ServiceModel;
using Better4You.UserManagment.Service.Messages;
using Better4You.UserManagment.ViewModel;
using Tar.Service.Messages;
using System.Collections.Generic;

namespace Better4You.UserManagment.Service
{
    [ServiceContract]
    public interface ISchoolService
    {
        [OperationContract]
        SchoolGetAllResponse GetAllByFilter(SchoolGetAllRequest request);
        
        [OperationContract]
        SchoolGetResponse Get(SchoolGetRequest request);
        
        [OperationContract]
        SchoolSaveResponse Save(SchoolSaveRequest request);
        

        [OperationContract]
        VoidResponse Delete(SchoolDeleteRequest request);
        

        [OperationContract]
        VoidResponse CreateUser(SchoolUserRequest request);

        [OperationContract]
        VoidResponse DeleteUser(SchoolUserRequest request);
        
        [OperationContract]
        LookupResponse<UserItemView> GetSchoolUser(SchoolUserFilterRequest request);

        [OperationContract]
        VoidResponse CreateSchoolAnnualAgreement(SchoolAnnualAgreementRequest request);
        
        [OperationContract]
        VoidResponse UpdateSchoolAnnualAgreement(SchoolAnnualAgreementRequest request);
        
        [OperationContract]
        VoidResponse DeleteSchoolAnnualAgreement(SchoolAnnualAgreementRequest request);
        
        [OperationContract]
        SchoolAnnualAgreementGetResponse GetSchoolAnnualAgreement(SchoolAnnualAgreementRequest request);

        [OperationContract]
        VoidResponse CreateContactInfo(CreateContactInfoRequest request);

        [OperationContract]
        VoidResponse CreateSchoolRoute(SchoolRouteRequest request);

        [OperationContract]
        VoidResponse UpdateSchoolRoute(SchoolRouteRequest request);

        [OperationContract]
        VoidResponse DeleteSchoolRoute(SchoolRouteRequest request);

        [OperationContract]
        SchoolRouteResponse GetSchoolRoute(SchoolRouteRequest request);

        [OperationContract]
        SchoolAnnualAgreementGetResponse GetAnnualAggrementByIdAndYear(SchoolAnnualAgreementRequest request);

        [OperationContract]
        AllAnnualAgreementsGetResponse AllAnnualAgreementsByYear(SchoolAnnualAgreementRequest request);
    
    }
}
