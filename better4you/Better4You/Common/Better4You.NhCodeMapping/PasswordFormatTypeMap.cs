using Better4You.Core;
using Better4You.EntityModel;

namespace Better4You.NhCodeMapping
{
    public class PasswordFormatTypeMap : LookupItemBaseMap<PasswordFormatType>
    {
        public PasswordFormatTypeMap()
            : base(LookupGroups.PasswordFormatType)
        {
        }
    }
}