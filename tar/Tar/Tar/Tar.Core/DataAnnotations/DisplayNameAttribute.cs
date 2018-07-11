using Tar.Globalization;

namespace Tar.Core.DataAnnotations
{
    public class DisplayNameAttribute : System.ComponentModel.DisplayNameAttribute
    {
        public DisplayNameAttribute(string fieldName)
            :base(ServiceLocator.Current.Get<IResourceManager>().Field(fieldName))
        {
        }
    }
}