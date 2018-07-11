using System.ServiceModel;
using Better4You.UserManagment.Service.Messages;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service
{
    [ServiceContract]
    public interface IUserService
    {

        [OperationContract]
        UpdateUserResponse UpdateUser(UpdateUserRequest request);

        [OperationContract]
        ResetPasswordResponse ResetPassword(ResetPasswordRequest request);
        [OperationContract]
        ChangePasswordResponse ChangePassword(ChangePasswordRequest request);
        [OperationContract]
        ForgotPasswordResponse ForgotPassword(ForgotPasswordRequest request);

        [OperationContract]
        AddRoleToUserResponse AddRoleToUser(AddRoleToUserRequest request);
        [OperationContract]
        GetUserRolesResponse GetUserRoles(GetUserRolesRequest request);

        [OperationContract]
        GetAllRolesResponse GetAllRoles(GetAllRolesRequest request);

        [OperationContract]
        GetUserResponse GetUser(GetUserRequest request);
        [OperationContract]
        LoginResponse Login(LoginRequest request);
        [OperationContract]
        GetUserApplicationsResponse GetUserApplications(GetUserApplicationsRequest request);
        [OperationContract]
        GetApplicationUsersResponse GetApplicationUsers(GetApplicationUsersRequest request);

        [OperationContract]
        GetApplicationUsersResponse GetApplicationUsersByFilter(GetApplicationUsersRequest request);
        [OperationContract]
        VoidResponse LockUser(LockUserRequest request);
        [OperationContract]
        VoidResponse UnLockUser(UnLockUserRequest request);
    }
}
