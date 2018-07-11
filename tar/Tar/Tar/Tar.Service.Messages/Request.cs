using System.Runtime.Serialization;
using Tar.Core;
using Tar.Core.Configuration;
using Tar.Core.DataAnnotations;

namespace Tar.Service.Messages
{
    [DataContract]
    public abstract class Request
    {
        public virtual void Validate()
        {
            new DataAnnotationsValidatorManager
            {
                Settings = ServiceLocator.Current.Get<IApplicationSettings>()
            }.Validate(this);
        }
    }
}