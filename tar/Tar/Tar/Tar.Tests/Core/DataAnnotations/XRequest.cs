using Tar.Core.DataAnnotations;
using Tar.ViewModel;

namespace Tar.Tests.Core.DataAnnotations
{
    public class XRequest
    {
        public XChild1 XChild1 { get; set; }
        [Required]
        public GeneralItemView GeneralItemView { get; set; }
    }
}