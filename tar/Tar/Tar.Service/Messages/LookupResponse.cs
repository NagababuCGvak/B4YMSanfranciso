using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Tar.Service.Messages
{
    [DataContract]
    public class LookupResponse<T> : Response
    {
        [DataMember]
        public IEnumerable<T> List { get; set; }

        public LookupResponse()
        {
            List = new List<T>();
        }
    }
}