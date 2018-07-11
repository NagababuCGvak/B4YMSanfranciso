using Better4You.Core;
using Better4You.EntityModel;

namespace Better4You.NhCodeMapping
{
    public class AlgorithmTypeMap : LookupItemBaseMap<AlgorithmType>
    {
        public AlgorithmTypeMap()
            : base(LookupGroups.AlgorithmType)
        {
        }
    }
}