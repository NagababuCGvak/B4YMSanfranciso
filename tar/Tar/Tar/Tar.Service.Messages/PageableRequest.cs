using System.Runtime.Serialization;

namespace Tar.Service.Messages
{
    [DataContract]
    public class PageableRequest : Request
    {
        [DataMember]
        public int PageSize { get; set; }
        [DataMember]
        public int PageIndex { get; set; }

        [DataMember]
        public string OrderByField { get; set; }
        [DataMember]
        public bool OrderByAsc { get; set; }
    }
}