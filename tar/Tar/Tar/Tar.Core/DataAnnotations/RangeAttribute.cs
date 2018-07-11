using System;
using Tar.Globalization;

namespace Tar.Core.DataAnnotations
{
    /// <summary>
    /// Zorunlu alan.
    /// </summary>
    public class RangeAttribute : System.ComponentModel.DataAnnotations.RangeAttribute
    {
        private readonly IResourceManager _resourceManager;
        private bool _isConverted = false;
        public override string FormatErrorMessage(string name)
        {
            if(!_isConverted)
            {
                ErrorMessage = _resourceManager.Warning(ErrorMessage);
                _isConverted = true;
            }
            return base.FormatErrorMessage(name);
        }
        // Summary:
        //     Initializes a new instance of the System.ComponentModel.DataAnnotations.RangeAttribute
        //     class by using the specified minimum and maximum values.
        //
        // Parameters:
        //   minimum:
        //     Specifies the minimum value allowed for the data field value.
        //
        //   maximum:
        //     Specifies the maximum value allowed for the data field value.
        public RangeAttribute(double minimum, double maximum)
            : base(minimum, maximum)
        {
            _resourceManager = ServiceLocator.Current.Get<IResourceManager>();
        }

        //
        // Summary:
        //     Initializes a new instance of the System.ComponentModel.DataAnnotations.RangeAttribute
        //     class by using the specified minimum and maximum values.
        //
        // Parameters:
        //   minimum:
        //     Specifies the minimum value allowed for the data field value.
        //
        //   maximum:
        //     Specifies the maximum value allowed for the data field value.
        public RangeAttribute(int minimum, int maximum) :
            base(minimum, maximum)
        {
            _resourceManager = ServiceLocator.Current.Get<IResourceManager>();
        }

        //
        // Summary:
        //     Initializes a new instance of the System.ComponentModel.DataAnnotations.RangeAttribute
        //     class by using the specified minimum and maximum values and the specific
        //     type.
        //
        // Parameters:
        //   type:
        //     Specifies the type of the object to test.
        //
        //   minimum:
        //     Specifies the minimum value allowed for the data field value.
        //
        //   maximum:
        //     Specifies the maximum value allowed for the data field value.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     type is null.
        public RangeAttribute(Type type, string minimum, string maximum) :
            base(type, minimum, maximum)
        {
            _resourceManager = ServiceLocator.Current.Get<IResourceManager>();
        }
    }

}