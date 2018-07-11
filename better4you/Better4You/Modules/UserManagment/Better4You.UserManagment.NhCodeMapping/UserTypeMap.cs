using Better4You.Core;
using Better4You.NhCodeMapping;
using Better4You.UserManagment.EntityModel;

namespace Better4You.UserManagment.NhCodeMapping
{
    public class UserTypeMap : LookupItemBaseMap<UserType>
    {
        public UserTypeMap()
            : base(LookupGroups.UserType)
        {
        }
    }
}
