using System.IO;
using System.Runtime.Serialization;
using Tar.Service.Messages;

namespace Better4You.Meal.Service.Messages
{
    [DataContract]
    public class ReportResponse:Response
    {
        [DataMember]
        public string FileName { get; set; }
    }
}
