using System.Runtime.Serialization;

namespace Tar.Service.Messages
{
    [DataContract]
    public class PageableResponse : Response
    {
        [DataMember]
        public int TotalCount { get; set; }
    }
}