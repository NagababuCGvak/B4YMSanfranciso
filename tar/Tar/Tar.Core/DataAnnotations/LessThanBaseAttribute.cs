using Tar.Globalization;

namespace Tar.Core.DataAnnotations
{
    public abstract class LessThanBaseAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
    {
        protected object Value;
        private readonly IResourceManager _resourceManager;

        protected LessThanBaseAttribute(object value)
        {
            Value = value;
        }
        protected LessThanBaseAttribute(IResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        protected LessThanBaseAttribute()
            :this(ServiceLocator.Current.Get<IResourceManager>())
        {
        }
        public override string FormatErrorMessage(string name)
        {
            var fieldName = _resourceManager.Field(name);
            return string.Format("[{0}] {1}'den k���k olmal�!", fieldName, Value);
        }
    }
}