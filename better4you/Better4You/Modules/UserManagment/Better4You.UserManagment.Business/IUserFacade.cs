using System.Collections.Generic;
using Better4You.Core;
using Better4You.UserManagment.ViewModel;

namespace Better4You.UserManagment.Business
{
    public interface IUserFacade
    {
        void ValidateUser(string userName, string password);
        UserViewModel GetUser(long userId);
        UserViewModel GetUser(string userName);
        void SaveUser(UserViewModel user);
        List<Tar.ViewModel.GeneralItemView> GetUserSchools(long userId);

        void AddUserToApplication(long applicationId, long userId);
        IList<GeneralItemView> GetUserApplications(long userId);
        IList<UserViewModel> GetActiveSchoolUsers(long schoolId);
        string GenerateActivationCode(long userId);
        IList<UserViewModel> GetApplicationUsers(long applicationId);
        IList<UserViewModel> GetApplicationUsersByFilter(long applicationId, string firstName, string lastName, string userName);
        IList<Tar.ViewModel.GeneralItemView> GetUserRoles(long userId);
        void ResetPassword(long userId);
        void ChangePassword(string userName, string activationCode, string newPassword);
        void ForgotPassword(string userName);
        void AddRoleToUser(long roleId, long userId);
        IList<GeneralItemView> GetAllRoles();
        void LockUser(long userId);
        void UnLockUser(long userId);
    }
}
