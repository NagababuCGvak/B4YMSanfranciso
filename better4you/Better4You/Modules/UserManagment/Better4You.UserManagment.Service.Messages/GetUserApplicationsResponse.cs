using System.Collections.Generic;
using System.Runtime.Serialization;
using Better4You.Core;
using Tar.Service.Messages;

namespace Better4You.UserManagment.Service.Messages
{
    [DataContract]
    public class GetUserApplicationsResponse : Response
    {
        [DataMember]
        public IList<GeneralItemView> Applications { get; set; }

        public GetUserApplicationsResponse()
        {
            Applications = new List<GeneralItemView>();
        }
    }
}