using Better4You.Core;
using Better4You.EntityModel;

namespace Better4You.NhCodeMapping
{
    public class AddressTypeMap:LookupItemBaseMap<AddressType>
    {
        public AddressTypeMap() : base(LookupGroups.AddressType)
        {
        }
    }
}