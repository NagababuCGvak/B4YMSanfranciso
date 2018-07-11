using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Tar.Core.DataAnnotations
{
    //[Compare("NewPassword","NewPasswordConfirm", ErrorMessage = "The new password and confirmation password do not match.")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CompareAttribute : ValidationAttribute //,IClientValidatable
    {
        public string Property1 { get; private set; }
        public string Property2 { get; private set; }

        public CompareAttribute(string property1, string property2)
        {
            if (property1 == null) throw new ArgumentNullException("property1");
            if (property2 == null) throw new ArgumentNullException("property2");
            Property1 = property1;
            Property2 = property2;
            ErrorMessage = string.Format("{0} and {1} do not match.", property1, property2);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) throw new ArgumentNullException("value");
            var p1 = value.GetType().GetProperties().FirstOrDefault(p => p.Name == Property1);
            if (p1 == null)
                return new ValidationResult(string.Format("{0} isimli property bulunamadı!", Property1));

            var p2 = value.GetType().GetProperties().FirstOrDefault(p => p.Name == Property2);
            if (p2 == null)
                return new ValidationResult(string.Format("{0} isimli property bulunamadı!", Property2));

            var p1Value = p1.GetValue(value, null);
            var p2Value = p2.GetValue(value, null);

            if (p1Value == null)
                return p2Value == null ? ValidationResult.Success : new ValidationResult(ErrorMessage, new[] { Property1, Property2 });

            return p1Value.Equals(p2Value) ? ValidationResult.Success : new ValidationResult(ErrorMessage, new[] { Property1, Property2 });
        }

        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //{
        //    var rule = new ModelClientValidationRule
        //    {
        //        ErrorMessage = ErrorMessage,
        //        ValidationType = "tarcompare"
        //    };
        //    rule.ValidationParameters.Add("property1", Property1);
        //    rule.ValidationParameters.Add("property2", Property2);

        //    return new[] { rule };
        //}
    }
}