using Better4You.Core;
using Better4You.EntityModel;

namespace Better4You.NhCodeMapping
{
    public class RecordStatusMap : LookupItemBaseMap<RecordStatus>
    {
        public RecordStatusMap()
            : base(LookupGroups.RecordStatus)
        {
        }
    }
}