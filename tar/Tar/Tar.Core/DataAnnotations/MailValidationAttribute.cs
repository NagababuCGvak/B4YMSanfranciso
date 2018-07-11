using Tar.Globalization;

namespace Tar.Core.DataAnnotations
{
    /// <summary>
    /// Mail Validation
    /// </summary>
    public class MailValidationAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute
    {
        private readonly IResourceManager _resourceManager;

        /// <summary>
        /// /// Mail Validation
        /// </summary>
        public MailValidationAttribute()
            : base(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")
        {
            _resourceManager = ServiceLocator.Current.Get<IResourceManager>();
        }
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            if (string.IsNullOrEmpty(value.ToString()))
                return false;

            var result = base.IsValid(value);
            return result;
        }
        public override string FormatErrorMessage(string name)
        {
            var columnResource = _resourceManager.Field(name);

            var message = string.Format("[{0}] bilgisi ge�erli formatta de�il! \"{1}\" �eklinde giriniz.",
                                        columnResource, "isim@sirket.com");
            return message;
        }
    }
}