using System;
using System.ComponentModel;
using System.Linq;
using Tar.Globalization;

namespace Tar.Core.DataAnnotations
{
    /// <summary>
    /// Required Field.
    /// </summary>
    public class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        private readonly IResourceManager _resourceManager;

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            if (value is DateTime)
                return Convert.ToDateTime(value) != DateTime.MinValue;

            if (value is Int64)
                return Convert.ToInt64(value) != 0;

            if (value is Int32)
                return Convert.ToInt32(value) != 0;

            if (value is Int16)
                return Convert.ToInt16(value) != 0;

            if (value is decimal)
                return Convert.ToDecimal(value) != 0.0m;

            if (value is double)
                return !Convert.ToDouble(value).Equals(0);
            var type = value.GetType();
            if (type.Name == "GeneralItemView" && type.Namespace == "Tar.ViewModel" && type.IsClass
                && TypeDescriptor.GetProperties(type)
                .Cast<PropertyDescriptor>()
                .Any(x => x.Name == "Id"))
            {
                var idField = TypeDescriptor.GetProperties(value.GetType())
                .Cast<PropertyDescriptor>()
                .First(x => x.Name == "Id");
                var fieldValue = idField.GetValue(value);
                if (fieldValue is Int64)
                    return Convert.ToInt64(fieldValue) != 0;
                if (fieldValue is Int32)
                    return Convert.ToInt32(fieldValue) != 0;
                if (fieldValue is Int16)
                    return Convert.ToInt16(fieldValue) != 0;
            }
            
            return base.IsValid(value);
        }

        public override string FormatErrorMessage(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            return string.Format("{0} : [{1}]",
                                 _resourceManager.Field("Attribute.ErrorMessage.Required"),
                                 !string.IsNullOrWhiteSpace(ErrorMessage) ? _resourceManager.Field(ErrorMessage) : name);
        }
        
        public RequiredAttribute()
        {
            _resourceManager = ServiceLocator.Current.Get<IResourceManager>();
        }
    }
}