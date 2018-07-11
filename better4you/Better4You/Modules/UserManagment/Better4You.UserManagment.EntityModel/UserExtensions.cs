using Better4You.Core;

namespace Better4You.UserManagment.EntityModel
{
    public static class UserExtensions
    {
        public static bool IsCompanyUser(this User user)
        {
            return user.UserType.Id.IsCompanyUser();
        }
        public static bool IsSchoolUser(this User user)
        {
            return user.UserType.Id.IsSchoolUser();
        }
    }
}
