using System.Runtime.Serialization;

namespace Tar.ViewModel
{
    [DataContract]
    public class PaginationInfoView
    {
        [DataMember]
        public bool OrderByAsc { get; set; }
        [DataMember]
        public string OrderByField { get; set; }
        [DataMember]
        public int PageIndex { get; set; }
        [DataMember]
        public int PageSize { get; set; }
    }
}
