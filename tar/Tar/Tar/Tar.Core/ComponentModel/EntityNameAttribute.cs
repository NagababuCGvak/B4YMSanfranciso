using Tar.Globalization;

namespace Tar.Core.ComponentModel
{
    /// <summary />
    public class EntityNameAttribute : System.ComponentModel.DisplayNameAttribute
    {
        public EntityNameAttribute(string entityName)
            : base(ServiceLocator.Current.Get<IResourceManager>().Entity(entityName))
        {
        }
    }
}
