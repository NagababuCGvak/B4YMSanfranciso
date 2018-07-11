using Tar.Core.DataAnnotations;
using Tar.ViewModel;

namespace Tar.Tests.Core.DataAnnotations
{
    public class GeneralItemViewValidationSampleObject
    {
        [Required]
        public GeneralItemView Invoice { get; set; }
    }
}