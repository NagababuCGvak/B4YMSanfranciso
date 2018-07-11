using Better4You.Core;
using Better4You.EntityModel;

namespace Better4You.NhCodeMapping
{
    public class CommunicationTypeMap : LookupItemBaseMap<CommunicationType>
    {
        public CommunicationTypeMap()
            : base(LookupGroups.CommunicationType)
        {
        }
    }    
}
