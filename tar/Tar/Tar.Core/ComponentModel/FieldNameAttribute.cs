using Tar.Globalization;

namespace Tar.Core.ComponentModel
{
    public class FieldNameAttribute: System.ComponentModel.DisplayNameAttribute
    {
        public FieldNameAttribute(string fieldName)
            : base(ServiceLocator.Current.Get<IResourceManager>().Field(fieldName))
        {
        }
    }
}