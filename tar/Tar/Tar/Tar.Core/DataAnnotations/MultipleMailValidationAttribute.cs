using System;
using System.Linq;
using Tar.Globalization;

namespace Tar.Core.DataAnnotations
{
    /// <summary>
    /// Mail Validation
    /// </summary>
    public class MultipleMailValidationAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
    {
        private readonly string _seperator;
        private readonly IResourceManager _resourceManager;
        /// <summary>
        /// /// Mail Validation
        /// </summary>
        public MultipleMailValidationAttribute(string seperator)
        {
            if (seperator == null) throw new ArgumentNullException("seperator");
            _seperator = seperator;
            _resourceManager = ServiceLocator.Current.Get<IResourceManager>();
        }

        public override string FormatErrorMessage(string name)
        {
            var columnResource = _resourceManager.Entity(name);
            var message = string.Format("\"{0}\" bilgisi ge�erli formatta de�il! \"{1}\" �eklinde giriniz."
                                        , columnResource
                                        , string.Format("isim@sirket.com{0}isim2@sirket.com.tr", _seperator));
            return message;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            if (string.IsNullOrEmpty(value.ToString()))
            {
                return false;
            }
            
            var emailAddress = value.ToString().Split(new[] {_seperator}, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (emailAddress.Count==0)
            {
                return false;
            }

            var mailValidator = new MailValidationAttribute();

            return emailAddress.All(mailValidator.IsValid);
        }
    }
}