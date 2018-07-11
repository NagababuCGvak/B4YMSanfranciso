using System;
using System.ServiceModel.Activation;
using Better4You.UserManagment.Business;
using Better4You.UserManagment.Service.Messages;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Implementation
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class UserService : Tar.Service.Service<IUserService, UserService>, IUserService
    {
        private readonly IUserFacade _userFacade;

        public UserService(IUserFacade userFacade)
        {
            if (userFacade == null) throw new ArgumentNullException("userFacade");
            _userFacade = userFacade;
        }
        public UpdateUserResponse UpdateUser(UpdateUserRequest request)
        {
            return Execute<UpdateUserRequest, UpdateUserResponse>(
                request,
                response =>
                {
                    var userId = request.User.UserId;
                    _userFacade.SaveUser(request.User);
                    
                    response.UserId = request.User.UserId;
                    if (userId == 0)
                    {
                        if (request.ApplicationId > 0)
                            _userFacade.AddUserToApplication(request.ApplicationId, response.UserId);
                        _userFacade.ResetPassword(response.UserId);
                    }
                });
        }

        public ResetPasswordResponse ResetPassword(ResetPasswordRequest request)
        {
            return Execute<ResetPasswordRequest, ResetPasswordResponse>(
                request, response => _userFacade.ResetPassword(request.UserId));
        }

        public ChangePasswordResponse ChangePassword(ChangePasswordRequest request)
        {
            return Execute<ChangePasswordRequest, ChangePasswordResponse>(
                request,
                response => _userFacade.ChangePassword(request.UserName, request.ActivationCode, request.Password));
        }

        public ForgotPasswordResponse ForgotPassword(ForgotPasswordRequest request)
        {
            return Execute<ForgotPasswordRequest, ForgotPasswordResponse>(
                request,
                response => _userFacade.ForgotPassword(request.UserName));
        }

        public AddRoleToUserResponse AddRoleToUser(AddRoleToUserRequest request)
        {
            return Execute<AddRoleToUserRequest, AddRoleToUserResponse>(
                request,
                response => _userFacade.AddRoleToUser(request.RoleId, request.UserId));
        }

        public GetUserRolesResponse GetUserRoles(GetUserRolesRequest request)
        {
            return Execute<GetUserRolesRequest, GetUserRolesResponse>(
                request,
                response => response.List = _userFacade.GetUserRoles(request.UserId));
        }

        public GetUserResponse GetUser(GetUserRequest request)
        {
            return Execute<GetUserRequest, GetUserResponse>(
                request, response =>
                             {
                                 response.User = request.UserId > 0
                                                     ? _userFacade.GetUser(request.UserId)
                                                     : _userFacade.GetUser(request.UserName);
                             });
        }

        public LoginResponse Login(LoginRequest request)
        {
            return Execute<LoginRequest, LoginResponse>(
                request,
                response => _userFacade.ValidateUser(request.UserName, request.Password));
        }

        public GetUserApplicationsResponse GetUserApplications(GetUserApplicationsRequest request)
        {
            return Execute<GetUserApplicationsRequest, GetUserApplicationsResponse>(
                request, response => response.Applications = _userFacade.GetUserApplications(request.UserId));
        }
        
        public GetApplicationUsersResponse GetApplicationUsers(GetApplicationUsersRequest request)
        {
            return Execute<GetApplicationUsersRequest, GetApplicationUsersResponse>(
                request, response => response.Users = _userFacade.GetApplicationUsers(request.ApplicationId));
        }

        public GetApplicationUsersResponse GetApplicationUsersByFilter(GetApplicationUsersRequest request)
        {
            return Execute<GetApplicationUsersRequest, GetApplicationUsersResponse>(
                request, response => response.Users = _userFacade.GetApplicationUsersByFilter(request.ApplicationId,request.Filter.FirstName,request.Filter.LastName,request.Filter.UserName));
        }
        
        public VoidResponse LockUser(LockUserRequest request)
        {
            return Execute<LockUserRequest, VoidResponse>(
                request, response => _userFacade.LockUser(request.UserId));
        }

        public VoidResponse UnLockUser(UnLockUserRequest request)
        {
            return Execute<UnLockUserRequest, VoidResponse>(
                request, response => _userFacade.UnLockUser(request.UserId));
        }

        public GetAllRolesResponse GetAllRoles(GetAllRolesRequest request)
        {
            return Execute<GetAllRolesRequest, GetAllRolesResponse>(
                request, response => response.List = _userFacade.GetAllRoles());
        }
    }
}
