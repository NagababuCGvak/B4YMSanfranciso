using Tar.Globalization;

namespace Tar.Core.DataAnnotations
{
    public abstract class GreaterThanBaseAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
    {
        private readonly IResourceManager _resourceManager;
        protected object Value;

        protected GreaterThanBaseAttribute(object value)
        {
            Value = value;
        }

        public override string FormatErrorMessage(string name)
        {
            var columnResource = _resourceManager.Field(name);
            return string.Format(_resourceManager.Warning("GreaterThanBaseAttribute_Warning"), columnResource, Value);
        }

        protected GreaterThanBaseAttribute(IResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        protected GreaterThanBaseAttribute()
            :this(ServiceLocator.Current.Get<IResourceManager>())
        {
        }
    }
}