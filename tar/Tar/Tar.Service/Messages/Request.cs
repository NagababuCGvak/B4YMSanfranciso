using System.Runtime.Serialization;

namespace Tar.Service.Messages
{
    [DataContract]
    public abstract class Request
    {
        public virtual void Validate()
        {
        }
    }
}