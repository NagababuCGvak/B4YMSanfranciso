using System.Collections.Generic;

namespace Tar.Core.DataAnnotations
{
    public interface IDataAnnotationsValidatorManager
    {
        bool IsValid(object instance);
        void Validate(object instance);
        List<ValidationErrorInfo> GetErrors(object instance);
        string ErrorMessage(List<ValidationErrorInfo> errorList);
        int DefaultMaxDepthLevel { get; set; }
        int MaxDepthLevel { get; }
    }
}