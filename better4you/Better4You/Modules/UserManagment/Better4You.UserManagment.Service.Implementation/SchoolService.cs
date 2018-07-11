using System;
using System.Linq;
using System.ServiceModel.Activation;
using Better4You.UserManagment.Business;
using Better4You.UserManagment.Service.Messages;
using Better4You.UserManagment.ViewModel;
using Tar.Service.Messages;


namespace Better4You.UserManagment.Service.Implementation
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SchoolService : Tar.Service.Service<ISchoolService, SchoolService>, ISchoolService
    {
        public ISchoolFacade SchoolFacade { get; set; }

        public SchoolGetAllResponse GetAllByFilter(SchoolGetAllRequest request)
        {

            return Execute<SchoolGetAllRequest, SchoolGetAllResponse>(
                request,
                response =>
                {
                    int totalCount;
                    response.Schools = SchoolFacade.GetByFilter(request.Filter, request.PageSize, request.PageIndex,
                                                                "Name", true, out totalCount);
                    response.TotalCount = totalCount;
                }
                );

        }


        public SchoolGetResponse Get(SchoolGetRequest request)
        {
            return Execute<SchoolGetRequest, SchoolGetResponse>(
                request,
                response =>
                {
                    response.School = SchoolFacade.Get(request.Id);
                }
                );
        }

        public SchoolSaveResponse Save(SchoolSaveRequest request)
        {
            return Execute<SchoolSaveRequest, SchoolSaveResponse>(
                request,
                response =>
                {
                    response.School = SchoolFacade.Save(request.School);

                    

                }
                );
        }

        public VoidResponse Delete(SchoolDeleteRequest request)
        {
            return Execute<SchoolDeleteRequest, VoidResponse>(
                request,
                response => SchoolFacade.Delete(request.SchoolId));

        }


        public VoidResponse CreateUser(SchoolUserRequest request)
        {
            return Execute<SchoolUserRequest, VoidResponse>(
                request,
                response => SchoolFacade.CreateUser(request.SchoolId, request.UserId));
        }

        public VoidResponse DeleteUser(SchoolUserRequest request)
        {
            return Execute<SchoolUserRequest, VoidResponse>(
                request,
                response => SchoolFacade.DeleteUser(request.SchoolId, request.UserId));
        }

        public LookupResponse<UserItemView> GetSchoolUser(SchoolUserFilterRequest request)
        {
            return Execute<SchoolUserFilterRequest, LookupResponse<UserItemView>>(
                request,
                response =>
                {
                    response.List = SchoolFacade.GetUserByFilter(request.FilterString);
                }
                );
        }

        public VoidResponse CreateSchoolAnnualAgreement(SchoolAnnualAgreementRequest request)
        {
            return Execute<SchoolAnnualAgreementRequest, VoidResponse>(
                request,
                response =>
                {
                    var yearAnnuals = SchoolFacade.AnnualAgreementsByFilter(request.SchoolId,
                                                                            request.SchoolAnnualAgreement.Year,
                                                                            request.SchoolAnnualAgreement.ItemType.Id);
                    if (yearAnnuals.Any())
                    {
                        throw new Exception(string.Format("{0}, {1} Annual Agrement Exist",
                                                          request.SchoolAnnualAgreement.Year, request.SchoolAnnualAgreement.ItemType));
                    }
                    SchoolFacade.CreateAnnualAgreement(request.SchoolId, request.SchoolAnnualAgreement);
                });
        }

        public VoidResponse UpdateSchoolAnnualAgreement(SchoolAnnualAgreementRequest request)
        {
            return Execute<SchoolAnnualAgreementRequest, VoidResponse>(
                request,
                response => SchoolFacade.UpdateAnnualAgreement(request.SchoolId, request.SchoolAnnualAgreement));
        }

        public VoidResponse DeleteSchoolAnnualAgreement(SchoolAnnualAgreementRequest request)
        {
            return Execute<SchoolAnnualAgreementRequest, VoidResponse>(
                request,
                response => SchoolFacade.DeleteAnnualAgreement(request.SchoolId, request.SchoolAnnualAgreement));
        }

        public SchoolAnnualAgreementGetResponse GetAnnualAggrementByIdAndYear(SchoolAnnualAgreementRequest request)
        {
            return Execute<SchoolAnnualAgreementRequest, SchoolAnnualAgreementGetResponse>(
                request, response =>
                
                    response.SchoolAnnualAgreement = new SchoolAnnualAgreementView
                    {
                        Price =
                            SchoolFacade.AnnualAgreementsByYearAndId(request.SchoolId,
                                request.SchoolAnnualAgreement.Year).First().Value,
                        Year =
                            SchoolFacade.AnnualAgreementsByYearAndId(request.SchoolId,
                                request.SchoolAnnualAgreement.Year).First().Key
                    }
                
                );

        }



        public SchoolAnnualAgreementGetResponse GetSchoolAnnualAgreement(SchoolAnnualAgreementRequest request)
        {
            return Execute<SchoolAnnualAgreementRequest, SchoolAnnualAgreementGetResponse>(
                request,
                response => response.SchoolAnnualAgreement = SchoolFacade.GetSchoolAnnualAgreement(request.SchoolAnnualAgreement.Id));
        }

        public VoidResponse CreateContactInfo(CreateContactInfoRequest request)
        {
            return Execute<CreateContactInfoRequest, VoidResponse>(
                request,
                response => SchoolFacade.CreateContactInfo(request.ContactInfo));
        }

        public VoidResponse CreateSchoolRoute(SchoolRouteRequest request)
        {
            return Execute<SchoolRouteRequest, VoidResponse>(
                   request,
                   response =>
                   {
                       SchoolFacade.CreateSchoolRoute(request.SchoolId, request.SchoolRoute);
                   });
        }

        public VoidResponse UpdateSchoolRoute(SchoolRouteRequest request)
        {
            return Execute<SchoolRouteRequest, VoidResponse>(
                request,
                response => SchoolFacade.UpdateSchoolRoute(request.SchoolId, request.SchoolRoute));
        }

        public VoidResponse DeleteSchoolRoute(SchoolRouteRequest request)
        {
            return Execute<SchoolRouteRequest, VoidResponse>(
                request,
                response => SchoolFacade.DeleteSchoolRoute(request.SchoolId, request.SchoolRoute));
        }

        public SchoolRouteResponse GetSchoolRoute(SchoolRouteRequest request)
        {
            return Execute<SchoolRouteRequest, SchoolRouteResponse>(
                request,
                response => response.SchoolRoute = SchoolFacade.GetSchoolRoute(request.SchoolRoute.Id));
        }


        public AllAnnualAgreementsGetResponse AllAnnualAgreementsByYear(SchoolAnnualAgreementRequest request)
        {
            return Execute<SchoolAnnualAgreementRequest, AllAnnualAgreementsGetResponse>(
                request,
                response =>
                {                   
                    response.SchoolAnnualAgreements = SchoolFacade.AllAnnualAgreementsByYear(request.SchoolAnnualAgreement.Year);                     
                }
                );            
        }
    }
}
