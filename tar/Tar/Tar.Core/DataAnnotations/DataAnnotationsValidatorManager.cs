using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Tar.Core.Configuration;

namespace Tar.Core.DataAnnotations
{
    /// <summary>
    /// ValidationManager For Data Annotations
    /// </summary>
    public class DataAnnotationsValidatorManager : IDataAnnotationsValidatorManager
    {
        public IApplicationSettings Settings { get; set; }

        public  bool IsValid(object instance)
        {
            return GetErrors(instance).Count == 0;
        }

        public  void Validate(object instance)
        {
            var errors = GetErrors(instance);
            if (errors.Count > 0)
                throw new Exceptions.WarningException(ErrorMessage(errors));
        }

        public List<ValidationErrorInfo> GetErrors(object instance)
        {
            return GetErrors(instance,0);
        }

        public const string MaxDepthLevelSettingKey = "DataAnnotationsValidatorManager.MaxDepthLevel";
        public int DefaultMaxDepthLevel { get; set; }
        public int MaxDepthLevel
        {
            get
            {
                return Settings != null && Settings.Contains(MaxDepthLevelSettingKey)
                    ? Settings.GetSetting<int>(MaxDepthLevelSettingKey)
                    : DefaultMaxDepthLevel;
            }
        }

        private List<ValidationErrorInfo> GetErrors(object instance, int levelIndex)
        {
            var errorList = new List<ValidationErrorInfo>();
            errorList.AddRange(from attribute in TypeDescriptor.GetAttributes(instance).OfType<ValidationAttribute>()
                where !attribute.IsValid(instance)
                select new ValidationErrorInfo(instance.GetType().Name, attribute.FormatErrorMessage(
                    instance.GetType().Name), instance));

            errorList.AddRange(from prop in TypeDescriptor.GetProperties(instance).Cast<PropertyDescriptor>()
                from attribute in prop.Attributes.OfType<ValidationAttribute>()
                where !attribute.IsValid(prop.GetValue(instance))
                select
                    new ValidationErrorInfo(prop.Name,
                        attribute.FormatErrorMessage(string.IsNullOrEmpty(prop.DisplayName)
                            ? prop.Name
                            : prop.DisplayName), instance));

            levelIndex++;
            if (levelIndex < MaxDepthLevel)
            {
                TypeDescriptor.GetProperties(instance)
                    .Cast<PropertyDescriptor>()
                    .Where(x => !x.PropertyType.IsPrimitive)
                    .ToList().ForEach(x => errorList.AddRange(GetErrors(x.GetValue(instance), levelIndex)));
            }
            return errorList;
        }

        public string ErrorMessage(List<ValidationErrorInfo> errorList)
        {
            var result = new StringBuilder();

            var counter = 0;
            errorList.ForEach(error => result.AppendLine((++counter).ToString() + ". " + error.FormatErrorMessage));

            return result.ToString();
        }

        public DataAnnotationsValidatorManager()
        {
            DefaultMaxDepthLevel = 5;
        }
    }
}
